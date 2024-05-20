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

         public int _storeId {get; set;}
         public string _storeName {get; set;}
         public bool _active {get; set;}

         public string _storePhoneNum {get; set;}

         public string _storeEmailAdd {get; set;}

         public SynchronizedCollection<Product> _products {get; set;}
         public SynchronizedCollection<Purchase> _purchases {get; set;}
        // public DiscountPolicyManager _discountPolicyManager {get; set;}
        // public PurchasePolicyManager _purchasePolicyManager {get; set;}
        public Dictionary<int, Role> roles {get; set;}
         public long _productIdCounter {get; set;}
        public int _purchaseIdCounter {get; set;}

        public double _raiting {get; set;}

        public Store(int Id, string name, string email, string phoneNum)
        {
            _storeId = Id;
            _storeName = name;
            _storeEmailAdd=email;
            _storePhoneNum=phoneNum;
            _active = true;
            _products = ProductRepositoryRAM.GetInstance().GetShopProducts(_storeId);
            roles = RoleRepositoryRAM.GetInstance().getShopRoles(_storeId);
            _purchases = PurchaseRepositoryRAM.GetInstance().GetShopPurchaseHistory(_storeId);
            //_discountPolicyManager = new DiscountPolicyManager(shopId);
            //_purchasePolicyManager = new PurchasePolicyManager(shopId);
            _raiting = 0;
        }

         public int StoreId { get => _storeId; }
        public SynchronizedCollection<Product> Products { get => _products; set => _products = value; }
        public bool Active { get => _active; set => _active = value; }
        public string Name { get => _storeName; set => _storeName = value; }
        public SynchronizedCollection<Purchase> Purchases { get => _purchases; set => _purchases = value; }
    

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
            _products.Add(p);
            ProductRepositoryRAM.GetInstance().Add(p);
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
            _products.Remove(p);
            ProductRepositoryRAM.GetInstance().Delete(p);
        }

        private Product GetProduct(int productId)
        {
            return _products.ToList().Find((p) => p._productid == productId);
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

        public void CloseStore(int userId)
        {
            if(getRole(userId)!=null && getRole(userId).canCloseStore()) {
                if (_active){
                _active = false;
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
                }
                else {
                    throw new Exception("Store already opened.");
                }
            }   
             else throw new Exception($"Permission exception for userId: {userId}");
        }

         public void UpdateProductPrice(int userId, int productID, double price)
        {
           if(getRole(userId)!=null && getRole(userId).canOpenStore())
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
            if(getRole(userId)!=null && getRole(userId).canOpenStore())
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
                RemoveBasketProductsFromSupply(basket);                
                double basketPrice = CalculateBasketPrice(basket);
                Purchase pendingPurchase = new Purchase(GenerateUniquePurchaseId(), _storeId, userId, Basket.Clone(basket), basketPrice);
                AddPurchase(pendingPurchase);
                return pendingPurchase;
            
        }

        public double CalculateBasketPrice(Basket basket){
            double totalPrice =0;
            foreach (KeyValuePair<int, int> product in basket.products)
            {
                Product productToBuy = GetProduct(product.Key);
                int quantity = product.Value;
                totalPrice += quantity*productToBuy.Price;
            }
            return totalPrice;
        }

         private void AddPurchase(Purchase p)
        {
                _purchases.Add(p);
                PurchaseRepositoryRAM.GetInstance().Add(p);
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

        public List<Product> SearchByKeywords(string keywords)
        {
            return _products.ToList().FindAll((p) => p.ContainKeyword(keywords));
        }

        public List<Product> SearchByName(string name)
        {
            string lowerName = name.ToLower();
            return _products.ToList().FindAll((p) => p.Name.ToLower().Contains(lowerName) || lowerName.Contains(p.Name.ToLower()));
        }

        public List<Product> SearchByCategory(string category)
        {
            return _products.ToList().FindAll((p) =>  p.Category == category);
        }

        public List<Purchase> getHistory(int userId)
        {
            if(getRole(userId)!=null && getRole(userId).canGetHistory())
            {
                return _purchases.ToList();
            }
            else throw new Exception($"Permission exception for userId: {userId}");
        }

        public string GetInfo()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Product product in _products)
            {
                sb.AppendLine(product.GetInfo());
            }
            return sb.ToString();
        }

        public void AddStaffMember(int roleUserId ,Role role, int userId){
            if(getRole(userId)!=null && getRole(userId).canAddStaffMember())
            {
                roles.Add(roleUserId, role);
            }
            else throw new Exception($"Permission exception for userId: {userId}");

        }

        public void RemoveStaffMember(int roleUserId, int userId){
            if(getRole(userId)!=null && getRole(userId).canAddStaffMember())
            {
                 if (roles.ContainsKey(roleUserId))
                    {
                        roles.Remove(roleUserId);
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
    
    }
}