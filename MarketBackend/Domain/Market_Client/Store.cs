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
         public ConcurrentDictionary<int, IRule> _rules {get; set;}

         private StoreRuleFactory _storeRuleFactory {get; set;}

         public string _storePhoneNum {get; set;}

         public string _storeEmailAdd {get; set;}

         public SynchronizedCollection<Product> _products {get; set;}
         public DiscountPolicyManager _discountPolicyManager {get; set;}
         public PurchasePolicyManager _purchasePolicyManager {get; set;}
        public ConcurrentDictionary<string, Role> roles {get; set;}
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
        
    

        public Product AddProduct(string userName, string name, string sellMethod, string description, double price, string category, int quantity, bool ageLimit)
        {
                    if(getRole(userName)!=null && getRole(userName).canAddProduct()) {

                        int productId = GenerateUniqueProductId();
                        Product newProduct = new Product(productId, this._storeId, name, sellMethod, description, price, category, quantity, ageLimit);
                        AddProduct(newProduct);
                        return newProduct;
                    }
                    else throw new Exception($"Permission exception for userName: {userName}");
            
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

        public void RemoveProduct(string userName, int productId)
        {
                if(getRole(userName)!=null && getRole(userName).canRemoveProduct()) {
                    Product productToRemove = GetProduct(productId);
                    RemoveProduct(productToRemove);  
                }
                else throw new Exception($"Permission exception for userName: {userName}");
               
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
            return _products.ToList().Find((p) => p._productId == productId);
            }
        }

        private Role getRole(string userName){

        if (roles.TryGetValue(userName, out Role role))
        {
            return role;
        }
        else
        {
            return null;
        }
        }

        public int AddDiscountPolicy(string userName, DateTime expirationDate, string subject, int ruledId, double precantage)
        {
           
            if(getRole(userName)!=null && getRole(userName).canUpdateProductPrice())
            {
                IRule rule = GetRule(ruledId);                    
                return _discountPolicyManager.AddPolicy(_policyIdFactory++, expirationDate, CastProductOrCategory(subject), rule, precantage);
            }
            else throw new Exception($"Permission exception for userName: {userName}");                        
        }

        public int AddCompositePolicy(string userName, DateTime expirationDate, string subject, NumericOperator Operator, List<int> policies)
        {
            
            if(getRole(userName)!=null && getRole(userName).canUpdateProductPrice())
            {
                return _discountPolicyManager.AddCompositePolicy(_policyIdFactory, expirationDate, CastProductOrCategory(subject), Operator, policies);
            }
            else throw new Exception($"Permission exception for userName: {userName}");
           
        }

        public void RemovePolicy(string userName, int policyId, string type)
        {
            
            if(getRole(userName)!=null && getRole(userName).canUpdateProductPrice())
            {
                switch (type)
                {
                    case "DiscountPolicy": _discountPolicyManager.RemovePolicy(policyId); break;
                    case "PurchasePolicy": _purchasePolicyManager.RemovePolicy(policyId); break;
                }
            }
            else throw new Exception($"Permission exception for userName: {userName}");
            
        }

        public void RemoveDiscountPolicy(string userName, int policyId)
        {
           
            if(getRole(userName)!=null && getRole(userName).canUpdateProductPrice())
            {
                _discountPolicyManager.RemovePolicy(policyId);
            }
            else throw new Exception($"Permission exception for userName: {userName}");
            
        }

        public int AddPurchasePolicy(string userName, DateTime expirationDate, string subject, int ruledId)
        {
             if(getRole(userName)!=null && getRole(userName).canUpdateProductPrice())
            {
                IRule rule = GetRule(ruledId);               
                return  _purchasePolicyManager.AddPolicy(_policyIdFactory++, expirationDate, CastProductOrCategory(subject), rule);
            }
            else throw new Exception($"Permission exception for userName: {userName}");
        }
        public void RemovePurchasePolicy(string userName, int policyId)
        {
            if(getRole(userName)!=null && getRole(userName).canUpdateProductPrice())
            {
                _purchasePolicyManager.RemovePolicy(policyId);
            }
                else throw new Exception($"Permission exception for userName: {userName}");
            
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

        public void CloseStore(string userName)
        {
            if(getRole(userName)!=null && getRole(userName).canCloseStore()) {
                if (_active){
                _active = false;
                Event e = new StoreClosedEvent(this, userName);
                _eventManager.NotifySubscribers(e);
                }
                else {
                    throw new Exception("Store already closed.");
                }
            }
             else throw new Exception($"Permission exception for userName: {userName}");
        }

        public void OpenStore(string userName)
        {
            if(getRole(userName)!=null && getRole(userName).canOpenStore()) {
                if (!_active){
                _active = true;
                Event e = new StoreOpenEvent(this, userName);
                _eventManager.NotifySubscribers(e);
                }
                else {
                    throw new Exception("Store already opened.");
                }
            }   
             else throw new Exception($"Permission exception for userName: {userName}");
        }

         public void UpdateProductPrice(string userName, int productID, double price)
        {
           if(getRole(userName)!=null && getRole(userName).canUpdateProductPrice())
            {
                Product productToUpdate = GetProduct(productID);
                if (productToUpdate != null)
                {
                    productToUpdate.updatePrice(price);
                    ProductRepositoryRAM.GetInstance().Update(productToUpdate);
                }
                else throw new Exception("Invalid product Id");
            }
            else throw new Exception($"Permission exception for userName: {userName}");
        }

        public void UpdateProductQuantity(string userName, int productID, int qauntity)
        {
            if(getRole(userName)!=null && getRole(userName).canUpdateProductQuantity())
            {
                Product productToUpdate = GetProduct(productID);
                if (productToUpdate != null)
                {
                    productToUpdate.updateQuantity(qauntity);
                    ProductRepositoryRAM.GetInstance().Update(productToUpdate);
                }
                else throw new Exception("Invalid product Id");
            }
            else throw new Exception(String.Format("Permission exception for userName: %d", userName));
        }


         public Purchase PurchaseBasket(string identifier, Basket basket)
        {
            if (!_active)
                throw new Exception($"Shop: {_storeName} is not active anymore");      

            basket.resetDiscount();
            _purchasePolicyManager.Apply(basket);
            _discountPolicyManager.Apply(basket);  
            RemoveBasketProductsFromSupply(basket);                
            double basketPrice = CalculateBasketPrice(basket);
            Purchase pendingPurchase = new Purchase(GenerateUniquePurchaseId(), _storeId, identifier, Basket.Clone(basket), basketPrice);
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
            throw new Exception($"Product Name: \'{product.Name}\' Id: {product._productId} not exist in shop.");
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

        public List<Purchase> getHistory(string userName)
        {
            if(getRole(userName)!=null && getRole(userName).canGetHistory())
            {
                return _history._purchases.ToList();
            }
            else throw new Exception($"Permission exception for userName: {userName}");
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

        public void AddStaffMember(string roleuserName ,Role role, string userName){
            if ((getRole(userName)!=null && getRole(userName).canAddStaffMember(role.getRoleName())) || (role.getRoleName() == RoleName.Founder && !checkForFounders() ))
            {
                roles.TryAdd(roleuserName, role);
                Event e = new AddAppointmentEvent(this, userName, roleuserName, role);
                _eventManager.NotifySubscribers(e);
                //add to active user appointees list the newly appointed staff member
            }
            else throw new Exception($"Permission exception for userName: {userName}");

        }

        public void SubscribeStaffMember(Member appoint, Member appointe)
        {
            _eventManager.Listeners["Add Appointment Event"].Add(appoint);
            _eventManager.Listeners["Add Appointment Event"].Add(appointe);
            _eventManager.Listeners["Remove Appointment Event"].Add(appoint);
            _eventManager.Listeners["Remove Appointment Event"].Add(appointe);
        }

        public void RemoveStaffMember(string roleuserName, string userName){
            if(getRole(userName)!=null && getRole(userName).canRemoveStaffMember(getRole(roleuserName).getRoleName()))
            {
                if (roles.ContainsKey(roleuserName))
                {
                    roles.TryRemove(new KeyValuePair<string, Role>(roleuserName, roles[roleuserName]));
                    Event e = new RemoveAppointmentEvent(this, userName, roleuserName);
                    _eventManager.NotifySubscribers(e);
                    //remove from active user appointees list the removed staff member
                }
            }
            else throw new Exception($"Permission exception for userName: {userName}");

        }

        public void AddKeyword(int productId, string keyWord){
            GetProduct(productId).AddKeyword(keyWord);
        }

        public void Remove(int productId, string keyWord){
            GetProduct(productId).RemoveKeyword(keyWord);
        }

        public void AddPermission(string userName, string toAddUserName, Permission permission)
        {
            if (getRole(userName) != null && getRole(userName).canEditPermissions())
            {
                getRole(toAddUserName).addPermission(permission);
            }
            else throw new Exception($"Permission exception for userName: {userName}");

        }

        public void RemovePermission(string userName, string toRemoveUserName, Permission permission)
        {
            if (getRole(userName) != null && getRole(userName).canEditPermissions())
            {
                getRole(toRemoveUserName).removePermission(permission);
            }
            else throw new Exception($"Permission exception for userName: {userName}");

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

        public int AddSimpleRule(string userName, string subject)
        {
            if(getRole(userName)!=null && getRole(userName).canUpdateProductPrice())
            {
                _storeRuleFactory.setFeatures(CastProductOrCategory(subject));
                IRule newRule = _storeRuleFactory.makeRule(typeof(SimpleRule));
                _rules.TryAdd(newRule.Id, newRule);
                RuleRepositoryRAM.GetInstance().Add(newRule);
                return newRule.Id;

            }
            else throw new Exception($"Permission exception for userName: {userName}");
               
        }

        public int AddQuantityRule(string userName, string subject, int minQuantity, int maxQuantity)
        {
            if(getRole(userName)!=null && getRole(userName).canUpdateProductPrice())
            {
                if(minQuantity >= maxQuantity || minQuantity < 0 || maxQuantity < 0){
                    throw new Exception($"Illegal quantities");
                }
                _storeRuleFactory.setFeatures(CastProductOrCategory(subject), minQuantity, maxQuantity);
                IRule newRule = _storeRuleFactory.makeRule(typeof(QuantityRule));
                _rules.TryAdd(newRule.Id, newRule);
                RuleRepositoryRAM.GetInstance().Add(newRule);
                return newRule.Id;

            }
            else throw new Exception($"Permission exception for userName: {userName}");
           
        }
         public int AddTotalPriceRule(string userName, string subject, int targetPrice)
        {
            if(getRole(userName)!=null && getRole(userName).canUpdateProductPrice())
            {
                if(targetPrice < 0){
                    throw new Exception($"Negtive price.");
                }
                _storeRuleFactory.setFeatures(CastProductOrCategory(subject), targetPrice);
                IRule newRule = _storeRuleFactory.makeRule(typeof(TotalPriceRule));
                _rules.TryAdd(newRule.Id, newRule);
                RuleRepositoryRAM.GetInstance().Add(newRule);
                return newRule.Id;

            }
            else throw new Exception($"Permission exception for userName: {userName}");
            
        }

        public int AddCompositeRule(string userName, LogicalOperator Operator, List<int> rules)
        {
            if(getRole(userName)!=null && getRole(userName).canUpdateProductPrice())
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
                else throw new Exception($"Permission exception for userName: {userName}");
        }
        public void UpdateRuleSubject(string userName, int ruleId, string subject)
        {
             if(getRole(userName)!=null && getRole(userName).canUpdateProductPrice())
            {
                IRule rule = GetRule(ruleId);
                rule.Subject = CastProductOrCategory(subject);
                RuleRepositoryRAM.GetInstance().Update(rule);
            }
            else throw new Exception($"Permission exception for userName: {userName}");
        }
        public void UpdateRuleTargetPrice(string userName, int ruleId, int targetPrice)
        {
            if(getRole(userName)!=null && getRole(userName).canUpdateProductPrice())
            {
                IRule rule = GetRule(ruleId);
                ((TotalPriceRule)rule).TotalPrice = targetPrice;
                RuleRepositoryRAM.GetInstance().Update(rule);
            }
            else throw new Exception($"Permission exception for userName: {userName}");   
        }
        public void UpdateCompositeOperator(string userName, int ruleId, LogicalOperator Operator)
        {
            if(getRole(userName)!=null && getRole(userName).canUpdateProductPrice())
            {
                IRule rule = GetRule(ruleId);
                ((CompositeRule)rule).Operator = Operator;
                RuleRepositoryRAM.GetInstance().Update(rule);
            }
            else throw new Exception($"Permission exception for userName: {userName}");
           
        }

        public void UpdateRuleQuantity(string userName, int ruleId, int minQuantity, int maxQuantity)
        {
            if(getRole(userName)!=null && getRole(userName).canUpdateProductPrice())
            {
                IRule rule = GetRule(ruleId);
                ((QuantityRule)rule).MinQuantity = minQuantity;
                ((QuantityRule)rule).MaxQuantity = maxQuantity;
                RuleRepositoryRAM.GetInstance().Update(rule);
            }
            else throw new Exception($"Permission exception for userName: {userName}");
        }

         public void UpdateCompositeRules(string userName, int ruleId, List<int> rules)
        {
            if(getRole(userName)!=null && getRole(userName).canUpdateProductPrice())
            {
                IRule rule = GetRule(ruleId);
                ((CompositeRule)rule).Rules.Clear();
                foreach (int id in rules)
                {
                    ((CompositeRule)rule).AddRule(GetRule(ruleId));
                }
                RuleRepositoryRAM.GetInstance().Update(rule);

                }
                else throw new Exception($"Permission exception for userName: {userName}");
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