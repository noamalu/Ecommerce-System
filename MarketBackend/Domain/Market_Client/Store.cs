using MarketBackend.DAL;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Data;
using MarketBackend.Domain.Market_Client;
using System;
using System.Text;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using MarketBackend.Domain.Models;



namespace MarketBackend.Domain.Market_Client
{
    public class Store
    {
        private readonly object _lock = new object();
         public int _storeId {get; set;}
         public string _storeName {get; set;}
         public bool _active {get; set;}
         public History _history {get; set;}
         private ConcurrentDictionary<int, IRule> _rules {get; set;}

         private StoreRuleFactory _storeRuleFactory {get; set;}

         public string _storePhoneNum {get; set;}

         public string _storeEmailAdd {get; set;}

         public SynchronizedCollection<Product> _products {get; set;}
         public DiscountPolicyManager _discountPolicyManager {get; set;}
         public PurchasePolicyManager _purchasePolicyManager {get; set;}
        public ConcurrentDictionary<int, Role> roles {get; set;}
         public long _productIdCounter {get; set;}
        public int _purchaseIdCounter {get; set;}

        public int _policyIdFactory {get; set;}

        public double _raiting {get; set;}

        private EventManager _eventManager {get; set;}

        public Store(int Id, string name, string email, string phoneNum)
        {
            _storeId = Id;
            _storeName = name;
            _storeEmailAdd=email;
            _storePhoneNum=phoneNum;
            _active = true;
            _products = ProductRepositoryRAM.GetInstance().GetShopProducts(_storeId);
            roles = RoleRepositoryRAM.GetInstance().getShopRoles(_storeId);
            _history = new History(_storeId);
            _discountPolicyManager = new DiscountPolicyManager(_storeId);
            _purchasePolicyManager = new PurchasePolicyManager(_storeId);
            _raiting = 0;
            _productIdCounter = 1;
            _policyIdFactory = 1;
            _eventManager = new EventManager(_storeId);
            _rules = new ConcurrentDictionary<int, IRule>();
            _storeRuleFactory = new StoreRuleFactory(_storeId);

        }

         public int StoreId { get => _storeId; }
        public SynchronizedCollection<Product> Products { get => _products; set => _products = value; }
        public bool Active { get => _active; set => _active = value; }
        public string Name { get => _storeName; set => _storeName = value; }
        
    

        public Product AddProduct(int userId, string name, string sellMethod, string description, double price, string category, int quantity, bool ageLimit)
        {
                    if(getRole(userId)!=null && getRole(userId).canAddProduct()) {

                        int productId = GenerateUniqueProductId();
                        Product newProduct = new Product(productId, this._storeId, name, sellMethod, description, price, category, quantity, ageLimit);
                        AddProduct(newProduct);
                        return newProduct;
                    }
                    else throw new Exception($"Permission exception for userId: {userId}");
            
        }

        public void AddProduct(Product p)
        {
            lock (_lock)
            {
            _products.Add(p);
            ProductRepositoryRAM.GetInstance().Add(p);
            }
        }

        private int GenerateUniqueProductId()
        {
            return int.Parse($"{_storeId}{_productIdCounter++}");
        }
        private int GenerateUniquePurchaseId()
        {
            return int.Parse($"{_storeId}{_purchaseIdCounter++}");
        }

        public void RemoveProduct(int userId, int productId)
        {
                if(getRole(userId)!=null && getRole(userId).canRemoveProduct()) {
                    Product productToRemove = GetProduct(productId);
                    RemoveProduct(productToRemove);  
                }
                else throw new Exception($"Permission exception for userId: {userId}");
               
        }
        private void RemoveProduct(Product p)
        {
            lock (_lock)
            {

            _products.Remove(p);
            ProductRepositoryRAM.GetInstance().Delete(p);
            }
        }

        public Product GetProduct(int productId)
        {
        lock (_lock)
            {
            return _products.ToList().Find((p) => p._productid == productId);
            }
        }

        private Role getRole(int userId){

        if (roles.TryGetValue(userId, out Role role))
        {
            return role;
        }
        else
        {
            return null;
        }
        }

        public void AddDiscountPolicy(int userId, DateTime expirationDate, string subject, int ruledId, double precantage)
        {
           
             if(getRole(userId)!=null && getRole(userId).canUpdateProductPrice())
                {
                    IRule rule = GetRule(ruledId);
                    _discountPolicyManager.AddPolicy(_policyIdFactory++, expirationDate, CastProductOrCategory(subject), rule, precantage);
                }
                else throw new Exception($"Permission exception for userId: {userId}");
            
            
        }

        public void AddCompositePolicy(int userId, DateTime expirationDate, string subject, NumericOperator Operator, List<int> policies)
        {
            
                 if(getRole(userId)!=null && getRole(userId).canUpdateProductPrice())
                {
                    _discountPolicyManager.AddCompositePolicy(_policyIdFactory, expirationDate, CastProductOrCategory(subject), Operator, policies);
                }
                else throw new Exception($"Permission exception for userId: {userId}");
           
        }

        public void RemovePolicy(int userId, int policyId, string type)
        {
            
                if(getRole(userId)!=null && getRole(userId).canUpdateProductPrice())
                {
                    switch (type)
                    {
                        case "DiscountPolicy": _discountPolicyManager.RemovePolicy(policyId); break;
                        case "PurchasePolicy": _purchasePolicyManager.RemovePolicy(policyId); break;
                    }
                }
                else throw new Exception($"Permission exception for userId: {userId}");
            
        }

        public void RemoveDiscountPolicy(int userId, int policyId)
        {
           
                if(getRole(userId)!=null && getRole(userId).canUpdateProductPrice())
                {
                    _discountPolicyManager.RemovePolicy(policyId);
                }
                else throw new Exception($"Permission exception for userId: {userId}");
            
        }

        public void AddPurchasePolicy(int userId, DateTime expirationDate, string subject, int ruledId)
        {
             if(getRole(userId)!=null && getRole(userId).canUpdateProductPrice())
            {
                IRule rule = GetRule(ruledId);
                _purchasePolicyManager.AddPolicy(_policyIdFactory++, expirationDate, CastProductOrCategory(subject), rule);
            }
            else throw new Exception($"Permission exception for userId: {userId}");
        }
        public void RemovePurchasePolicy(int userId, int policyId)
        {
            if(getRole(userId)!=null && getRole(userId).canUpdateProductPrice())
            {
                _purchasePolicyManager.RemovePolicy(policyId);
            }
                else throw new Exception($"Permission exception for userId: {userId}");
            
        }


        private RuleSubject CastProductOrCategory(string subject)
        {
            if (subject == _storeName){
                return new RuleSubject(subject, StoreId);
            }
            
            foreach (Product p in _products){
                if (p.Name.ToLower().Equals(subject.ToLower()))
                    {
                        return new RuleSubject(p);
                    }
                if (p.Category.ToLower().Equals(subject.ToLower()))
                    {
                        return new RuleSubject(subject);  
                    }
                else throw new Exception("could not find subject");
            }
         throw new Exception("could not find subject");
        }
        

        private IRule GetRule(int ruleId)
        {

            if (_rules.TryGetValue(ruleId, out IRule rule))
            {
                return rule;
            }
            else
                throw new Exception($"No Rule matches ruleId: {ruleId}");

        }

        public void CloseStore(int userId)
        {
            if(getRole(userId)!=null && getRole(userId).canCloseStore()) {
                if (_active){
                _active = false;
                Event e = new StoreClosedEvent(this, userId);
                _eventManager.NotifySubscribers(e);
                }
                else {
                    throw new Exception("Store already closed.");
                }
            }
             else throw new Exception($"Permission exception for userId: {userId}");
        }

        public void OpenStore(int userId)
        {
            if(getRole(userId)!=null && getRole(userId).canOpenStore()) {
                if (!_active){
                _active = true;
                Event e = new StoreOpenEvent(this, userId);
                _eventManager.NotifySubscribers(e);
                }
                else {
                    throw new Exception("Store already opened.");
                }
            }   
             else throw new Exception($"Permission exception for userId: {userId}");
        }

         public void UpdateProductPrice(int userId, int productID, double price)
        {
           if(getRole(userId)!=null && getRole(userId).canUpdateProductPrice())
            {
                Product productToUpdate = GetProduct(productID);
                if (productToUpdate != null)
                {
                    productToUpdate.updatePrice(price);
                    ProductRepositoryRAM.GetInstance().Update(productToUpdate);
                }
                else throw new Exception("Invalid product Id");
            }
            else throw new Exception($"Permission exception for userId: {userId}");
        }

        public void UpdateProductQuantity(int userId, int productID, int qauntity)
        {
            if(getRole(userId)!=null && getRole(userId).canUpdateProductQuantity())
            {
                Product productToUpdate = GetProduct(productID);
                if (productToUpdate != null)
                {
                    productToUpdate.updateQuantity(qauntity);
                    ProductRepositoryRAM.GetInstance().Update(productToUpdate);
                }
                else throw new Exception("Invalid product Id");
            }
            else throw new Exception(String.Format("Permission exception for userId: %d", userId));
        }


         public Purchase PurchaseBasket(int userId, Basket basket)
        {
            if (!_active)
                throw new Exception($"Shop: {_storeName} is not active anymore");      

            basket.resetDiscount();
            _purchasePolicyManager.Apply(basket);
            _discountPolicyManager.Apply(basket);  
            RemoveBasketProductsFromSupply(basket);                
            double basketPrice = CalculateBasketPrice(basket);
            Purchase pendingPurchase = new Purchase(GenerateUniquePurchaseId(), _storeId, userId, Basket.Clone(basket), basketPrice);
            _history.AddPurchase(pendingPurchase);
            Event e = new ProductSellEvent(this, pendingPurchase);
            _eventManager.NotifySubscribers(e);
            return pendingPurchase;
            
        }

        public double CalculateBasketPrice(Basket basket){
            double totalPrice =0;
            basket.resetDiscount();
            _discountPolicyManager.Apply(basket);
            foreach(BasketItem basketItem in basket.BasketItems)
            {
                double productPrice = basketItem.PriceAfterDiscount;
                int quantity = basketItem.Quantity;
                totalPrice += productPrice * quantity;
            }
            return totalPrice;
        }


    
        public bool checkBasketInSupply(Basket basket)
        {
            foreach (KeyValuePair<int, int> product in basket.products)
            {
                Product productToBuy = GetProduct(product.Key);
                int quantity = product.Value;
                if (!ProductInSupply(productToBuy, quantity))
                {
                    throw new ArgumentException($"Product {productToBuy.Name}: In supply: {productToBuy.Quantity}, You required: {quantity}");
                }
            }
            return true;
        }

        private bool ProductInSupply(Product product, int quantity)
        {
            if (_products.Contains(product))
                return quantity <= product.Quantity;
            throw new Exception($"Product Name: \'{product.Name}\' Id: {product._productid} not exist in shop.");
        }

        private void RemoveBasketProductsFromSupply(Basket basket)
        {
            foreach (KeyValuePair<int, int> product in basket.products)
            {
                Product productToBuy = GetProduct(product.Key);
                int quantity = product.Value;
                if (ProductInSupply(productToBuy, quantity))
                {
                    productToBuy.updateQuantity(productToBuy.Quantity - quantity);
                    ProductRepositoryRAM.GetInstance().Update(productToBuy);
                }
                else throw new Exception("This should not happened");
            }
        }

        public void CheckBasket(Basket basket){

        }

        public HashSet<Product> SearchByKeywords(string keywords)
        {
            return _products.ToList().FindAll((p) => p.ContainKeyword(keywords)).ToHashSet();
        }

        public HashSet<Product> SearchByName(string name)
        {
            string lowerName = name.ToLower();
            return _products.ToList().FindAll((p) => p.Name.ToLower().Contains(lowerName) || lowerName.Contains(p.Name.ToLower())).ToHashSet();
        }

        public HashSet<Product> SearchByCategory(string category)
        {
            return _products.ToList().FindAll((p) =>  p.Category == category).ToHashSet();
        }

        public List<Purchase> getHistory(int userId)
        {
            if(getRole(userId)!=null && getRole(userId).canGetHistory())
            {
                return _history._purchases.ToList();
            }
            else throw new Exception($"Permission exception for userId: {userId}");
        }

        public string getInfo()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Product product in _products)
            {
                sb.AppendLine(product.GetInfo());
            }
            return sb.ToString();
        }

        public string getProductInfo(int productId)
        {
            Product product = GetProduct(productId);
            if (product != null)
            {
                return product.GetInfo();
            }
            else
            {
                throw new Exception("Product not found");
            }
        }

        public void AddStaffMember(int roleUserId ,Role role, int userId){
            if ((getRole(userId)!=null && getRole(userId).canAddStaffMember(role.getRoleName())) || (role.getRoleName() == RoleName.Founder && !checkForFounders() ))
            {
                roles.TryAdd(roleUserId, role);
                Event e = new AddAppointmentEvent(this, userId, roleUserId, role);
                _eventManager.NotifySubscribers(e);
                //add to active user appointees list the newly appointed staff member
            }
            else throw new Exception($"Permission exception for userId: {userId}");

        }

        public void SubscribeStaffMember(Member appoint, Member appointe)
        {
            _eventManager.Listeners["Add Appointment Event"].Add(appoint);
            _eventManager.Listeners["Add Appointment Event"].Add(appointe);
            _eventManager.Listeners["Remove Appointment Event"].Add(appoint);
            _eventManager.Listeners["Remove Appointment Event"].Add(appointe);
        }

        public void RemoveStaffMember(int roleUserId, int userId){
            if(getRole(userId)!=null && getRole(userId).canRemoveStaffMember(getRole(roleUserId).getRoleName()))
            {
                if (roles.ContainsKey(roleUserId))
                {
                    roles.TryRemove(new KeyValuePair<int, Role>(roleUserId, roles[roleUserId]));
                    Event e = new RemoveAppointmentEvent(this, userId, roleUserId);
                    _eventManager.NotifySubscribers(e);
                    //remove from active user appointees list the removed staff member
                }
            }
            else throw new Exception($"Permission exception for userId: {userId}");

        }

        public void AddKeyword(int productId, string keyWord){
            GetProduct(productId).AddKeyword(keyWord);
        }

        public void Remove(int productId, string keyWord){
            GetProduct(productId).RemoveKeyword(keyWord);
        }

        public void AddPermission(int userId, int toAddId, Permission permission)
        {
            if (getRole(userId) != null && getRole(userId).canEditPermissions())
            {
                getRole(toAddId).addPermission(permission);
            }
            else throw new Exception($"Permission exception for userId: {userId}");

        }

        public void RemovePermission(int userId, int toRemoveId, Permission permission)
        {
            if (getRole(userId) != null && getRole(userId).canEditPermissions())
            {
                getRole(toRemoveId).addPermission(permission);
            }
            else throw new Exception($"Permission exception for userId: {userId}");

        }

        public bool checkForFounders ()
        {
            foreach (Role role in roles.Values)
            {
                if (role.getRoleName() == RoleName.Founder)
                {
                    return true;
                }
            }
            return false;
        }

        public bool checklegalBasket(Basket basket, bool IsAbove18){
            foreach (KeyValuePair<int, int> product in basket.products)
            {
                Product productToBuy = GetProduct(product.Key);
                if (productToBuy._ageLimit && !IsAbove18){
                    return false;
                }
            }
            return true;
        }

        public int AddSimpleRule(int userId, string subject)
        {
            if(getRole(userId)!=null && getRole(userId).canUpdateProductPrice())
            {
                _storeRuleFactory.setFeatures(CastProductOrCategory(subject));
                IRule newRule = _storeRuleFactory.makeRule(typeof(SimpleRule));
                _rules.TryAdd(newRule.Id, newRule);
                RuleRepositoryRAM.GetInstance().Add(newRule);
                return newRule.Id;

            }
            else throw new Exception($"Permission exception for userId: {userId}");
               
        }

        public int AddQuantityRule(int userId, string subject, int minQuantity, int maxQuantity)
        {
            if(getRole(userId)!=null && getRole(userId).canUpdateProductPrice())
            {
                _storeRuleFactory.setFeatures(CastProductOrCategory(subject), minQuantity, maxQuantity);
                IRule newRule = _storeRuleFactory.makeRule(typeof(QuantityRule));
                _rules.TryAdd(newRule.Id, newRule);
                RuleRepositoryRAM.GetInstance().Add(newRule);
                return newRule.Id;

            }
            else throw new Exception($"Permission exception for userId: {userId}");
           
        }
         public int AddTotalPriceRule(int userId, string subject, int targetPrice)
        {
            if(getRole(userId)!=null && getRole(userId).canUpdateProductPrice())
            {
                _storeRuleFactory.setFeatures(CastProductOrCategory(subject), targetPrice);
                IRule newRule = _storeRuleFactory.makeRule(typeof(TotalPriceRule));
                _rules.TryAdd(newRule.Id, newRule);
                RuleRepositoryRAM.GetInstance().Add(newRule);
                return newRule.Id;

            }
            else throw new Exception($"Permission exception for userId: {userId}");
            
        }

        public int AddCompositeRule(int userId, LogicalOperator Operator, List<int> rules)
        {
            if(getRole(userId)!=null && getRole(userId).canUpdateProductPrice())
            {
                List<IRule> rulesToAdd = new List<IRule>();
                foreach (int id in rules)
                {
                    rulesToAdd.Add(GetRule(id));
                }
                _storeRuleFactory.setFeatures(Operator, rulesToAdd);
                IRule newRule = _storeRuleFactory.makeRule(typeof(CompositeRule));
                _rules.TryAdd(newRule.Id, newRule);
                RuleRepositoryRAM.GetInstance().Add(newRule);
                return newRule.Id;
            }
                else throw new Exception($"Permission exception for userId: {userId}");
        }
        public void UpdateRuleSubject(int userId, int ruleId, string subject)
        {
             if(getRole(userId)!=null && getRole(userId).canUpdateProductPrice())
            {
                IRule rule = GetRule(ruleId);
                rule.Subject = CastProductOrCategory(subject);
                RuleRepositoryRAM.GetInstance().Update(rule);
            }
            else throw new Exception($"Permission exception for userId: {userId}");
        }
        public void UpdateRuleTargetPrice(int userId, int ruleId, int targetPrice)
        {
            if(getRole(userId)!=null && getRole(userId).canUpdateProductPrice())
            {
                IRule rule = GetRule(ruleId);
                ((TotalPriceRule)rule).TotalPrice = targetPrice;
                RuleRepositoryRAM.GetInstance().Update(rule);
            }
            else throw new Exception($"Permission exception for userId: {userId}");   
        }
        public void UpdateCompositeOperator(int userId, int ruleId, LogicalOperator Operator)
        {
            if(getRole(userId)!=null && getRole(userId).canUpdateProductPrice())
            {
                IRule rule = GetRule(ruleId);
                ((CompositeRule)rule).Operator = Operator;
                RuleRepositoryRAM.GetInstance().Update(rule);
            }
            else throw new Exception($"Permission exception for userId: {userId}");
           
        }

        public void UpdateRuleQuantity(int userId, int ruleId, int minQuantity, int maxQuantity)
        {
            if(getRole(userId)!=null && getRole(userId).canUpdateProductPrice())
            {
                IRule rule = GetRule(ruleId);
                ((QuantityRule)rule).MinQuantity = minQuantity;
                ((QuantityRule)rule).MaxQuantity = maxQuantity;
                RuleRepositoryRAM.GetInstance().Update(rule);
            }
            else throw new Exception($"Permission exception for userId: {userId}");
        }

         public void UpdateCompositeRules(int userId, int ruleId, List<int> rules)
        {
            if(getRole(userId)!=null && getRole(userId).canUpdateProductPrice())
            {
                IRule rule = GetRule(ruleId);
                ((CompositeRule)rule).Rules.Clear();
                foreach (int id in rules)
                {
                    ((CompositeRule)rule).AddRule(GetRule(ruleId));
                }
                RuleRepositoryRAM.GetInstance().Update(rule);

                }
                else throw new Exception($"Permission exception for userId: {userId}");
        }


        public void SubscribeStoreOwner(Member member)
        {
            _eventManager.Listeners["Store Closed Event"].Add(member);
            _eventManager.Listeners["Store Open Event"].Add(member);
            _eventManager.Listeners["Product Sell Event"].Add(member);
            _eventManager.Listeners["Remove Appointment Event"].Add(member);
            _eventManager.Listeners["Add Appointment Event"].Add(member);
        }
    }
}