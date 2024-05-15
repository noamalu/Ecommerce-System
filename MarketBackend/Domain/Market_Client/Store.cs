
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Data;
using MarketBackend.Domain.Market_Client;
using System;
using System.Text;
using System;
using System.Collections.Generic;
using System.Collections.Concurrent;



namespace MarketBackend.Domain.Models
{
    public class Store
    {

         private readonly int _storeId;
         private string _storeName;
         private bool _active;

         private int _storePhoneNum;

         private string _storeEmailAdd;

         private SynchronizedCollection<Product> _products;
         private SynchronizedCollection<Purchase> _purchases;
        // private DiscountPolicyManager _discountPolicyManager;
        // private PurchasePolicyManager _purchasePolicyManager;
        private Dictionary<int, Role> roles;
         private long _productIdCounter;
        private int _purchaseIdCounter;

        public Store(int Id, string name, string email, int phoneNum)
        {
            _storeId = Id;
            _storeName = name;
            _storeEmailAdd=email;
            _storePhoneNum=phoneNum;
            _active = true;
            _products = ProductRepo.GetInstance().GetShopProducts(_storeId);
            roles = RoleRepo.GetInstance().GetShopRoles(_storeId);
            _purchases = PurchaseRepo.GetInstance().GetShopPurchaseHistory(_storeId);
            //_discountPolicyManager = new DiscountPolicyManager(shopId);
            //_purchasePolicyManager = new PurchasePolicyManager(shopId);

        }

        public Product AddProduct(int userId, string name, string sellMethod, string description, double price, string category, int quantity, Collection<string> keywords)
        {
                    if(getRole(userId).role.canAddProduct()) {

                        int productId = GenerateUniqueProductId();
                        Product newProduct = new Product(productId, this._storeId, name, sellMethod, description, price, category, quantity, keywords);
                        AddProduct(newProduct);
                        return newProduct;
                    }
                    else throw new Exception($"Permission exception for userId: {userId}");
            
        }

        private void AddProduct(Product p)
        {
            _products.Add(p);
            ProductRepo.GetInstance().Add(p);
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
                if(getRole(userId)!=null && getRole(userId).role.canRemoveProduct()) {
                    Product productToRemove = GetProduct(productId);
                    RemoveProduct(productToRemove);  
                }
                else throw new Exception($"Permission exception for userId: {userId}");
               
        }
        private void RemoveProduct(Product p)
        {
            _products.Remove(p);
            ProductRepo.GetInstance().Delete(p.productId);
        }

        private Product GetProduct(int productId)
        {
            return _products.ToList().Find((p) => p.productId == productId);
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
            if(getRole(userId)!=null && getRole(userId).role.canCloseStore()) {
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
            if(getRole(userId)!=null && getRole(userId).role.canOpenStore()) {
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
           if(getRole(userId)!=null && getRole(userId).role.canOpenStore())
            {
                Product productToUpdate = GetProduct(productID);
                if (productToUpdate != null && price > 0)
                {
                    productToUpdate.price = price;
                    ProductRepo.GetInstance().Update(productToUpdate);
                }
                else throw new Exception("Invalid product Id");
            }
            else throw new Exception($"Permission exception for userId: {userId}");
        }

        public void UpdateProductQuantity(int userId, int productID, int qauntity)
        {
            if(getRole(userId)!=null && getRole(userId).role.canOpenStore())
            {
                Product productToUpdate = GetProduct(productID);
                if (productToUpdate != null && qauntity > 0)
                {
                    productToUpdate.qauntity=qauntity;
                    ProductRepo.GetInstance().Update(productToUpdate);
                }
                else throw new Exception("Invalid product Id");
            }
            else throw new Exception(String.Format("Permission exception for userId: %d", userId));
        }

        public void UpdateProductDiscount(int userId, int productID, int discount)
        {
            if(getRole(userId)!= null && getRole(userId).role.canOpenStore())
            {
                Product productToUpdate = GetProduct(productID);
                if (productToUpdate != null && discount > 0)
                {
                    productToUpdate.discount = discount;
                    ProductRepo.GetInstance().Update(productToUpdate);
                }
                else throw new Exception("Invalid product Id");
            }
            else throw new Exception(String.Format("Permission exception for userId: %d", userId));
        }

         public Purchase PurchaseBasket(int userId, Basket basket)
        {
            if (!_active)
                throw new Exception($"Shop: {_storeName} is not active anymore");
            
                if (checkBasketInSupply(basket))
                {
                    RemoveBasketProductsFromSupply(basket);
                }
                Purchase pendingPurchase = new Purchase(GenerateUniquePurchaseId(), _storeId, userId, basket.Clone());
                AddPurchase(pendingPurchase);
                return pendingPurchase;
            
        }

         private void AddPurchase(Purchase p)
        {
                _purchases.Add(p);
                PurchaseRepo.GetInstance().Add(p);
        }

        private bool checkBasketInSupply(Basket basket)
        {
            foreach (KeyValuePair<int, int> product in basket.products())
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
                return quantity <= product.quantity;
            throw new Exception($"Product Name: \'{product.Name}\' Id: {product.Id} not exist in shop.");
        }

        private void RemoveBasketProductsFromSupply(Basket basket)
        {
            foreach (KeyValuePair<int, int> product in basket.products())
            {
                Product productToBuy = GetProduct(product.Key);
                int quantity = product.Value;
                if (ProductInSupply(productToBuy, quantity))
                {
                    productToBuy.quantity -= quantity;
                    ProductRepo.GetInstance().Update(productToBuy);
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
            return _products.ToList().FindAll((p) => ((category & p.Category) == category));
        }

        public List<Purchase> getHistory(int userId)
        {
            if(getRole(userId)!=null && getRole(userId).role.canGetHistoey())
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
                sb.AppendLine(product.getProductInfo());
            }
            return sb.ToString();
        }

        public void AddStaffMember(int roleUserId ,Role role, int userId){
            if(getRole(userId)!=null && getRole(userId).role.canAddStaffMember())
            {
                roles.Add(roleUserId, role);
            }
            else throw new Exception($"Permission exception for userId: {userId}");

        }

        public void RemoveStaffMember(int roleUserId, int userId){
            if(getRole(userId)!=null && getRole(userId).role.canAddStaffMember())
            {
                 if (roles.ContainsKey(roleUserId))
                    {
                        roles.Remove(roleUserId);
                    }  
            }
            else throw new Exception($"Permission exception for userId: {userId}");

        }
    
    }
}