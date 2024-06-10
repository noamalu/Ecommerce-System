using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MarketBackend.Domain.Models;
using MarketBackend.Domain.Payment;
using MarketBackend.Domain.Shipping;

namespace MarketBackend.Domain.Market_Client
{
    public interface IMarketManagerFacade
    {
        void Register(string identifier, string username, string password, string email, int age);
        void EnterAsGuest(string identifier);
        void PurchaseCart(string identifier, PaymentDetails paymentDetails, ShippingDetails shippingDetails);
        int CreateStore(string identifier, string storeName, string email, string phoneNum);
        bool ResToStoreManageReq(string identifier);
        bool ResToStoreOwnershipReq(string identifier); //respond to store ownership request
        void LogoutClient(string identifier);
        void RemoveFromCart(int clientId, int productId, int basketId, int quantity);
        ShoppingCart ViewCart(string identifier);
        void AddToCart(int clientId, int storeId, int productId, int quantity);

        void LoginClient(string identifier, string username, string password);
        void ExitGuest(string identifier);
        // void UpdateProductDiscount(int productId, double discount);
        List<ShoppingCartHistory> GetPurchaseHistoryByClient(string identifier);
        List<Purchase> GetPurchaseHistoryByStore(int storeId, int userId);
        Product AddProduct(int storeId, int userId, string name, string sellMethod, string description, double price, string category, int quantity, bool ageLimit);
        void RemoveProduct(int storeId,int userId, int productId);
        void RemoveStaffMember(int storeId, int activeId, Role role, int toRemoveId);
        void AddManger(int activeId, int storeId, int toAddId);
        void RemoveManger(int activeId, int storeId, int toRemoveId);
        void AddOwner(int activeId, int storeId, int toAddId);
        void RemoveOwner(int activeId, int storeId, int toRemoveId);
        List<Member> GetOwners(int storeId);
        List<Member> GetMangers(int storeId);
        Member GetFounder(int storeId);

        void UpdateProductQuantity(int storeId, int userId, int productId, int quantity); 
        void UpdateProductPrice(int storeId, int userId,  int productId, double price);
        void CloseStore(int userId, int storeId);
        void OpenStore(int clientId, int storeId);
        bool IsAvailable(int storeId);
        void RemovePermission(int activeId, int storeId, int toRemoveId, Permission permission);
        void AddPermission(int activeId, int storeId, int toAddId, Permission permission);
        void EditPurchasePolicy(int storeId);
        HashSet<Product> SearchByKeyWords(string keywords);
        HashSet<Product> SearchByName(string name);
        HashSet<Product> SearchByCategory(string category);
        HashSet<Product> SearchByCategoryWithStore(int storeId, string category);
        HashSet<Product> SearchByKeyWordsWithStore(int storeId, string keywords);
        HashSet<Product> SearchByNameWithStore(int storeId, string name);
        void Filter (HashSet<Product> products, string category, double lowPrice, double highPrice, double lowProductRate, double highProductRate, double lowStoreRate, double highStoreRate);
        string GetProductInfo(int storId, int productId);
        public void AddStaffMember(int storeId, int activeId, Role role, int toAddId);   
        public string GetInfo(int storeId);    
        public void RemovePolicy(int clientId, int storeId, int policyID,string type);
        public int AddSimpleRule(int clientId, int storeId,string subject);
        public int AddQuantityRule(int clientId, int storeId, string subject, int minQuantity, int maxQuantity);
        public int AddTotalPriceRule(int clientId, int storeId, string subject, int targetPrice);
        public int AddCompositeRule(int clientId, int storeId, int Operator, List<int> rules);
        public void UpdateRuleSubject(int clientId, int storeId, int ruleId, string subject);
        public void UpdateRuleQuantity(int clientId, int storeId, int ruleId, int minQuantity, int maxQuantity);
        public void UpdateRuleTargetPrice(int clientId, int storeId, int ruleId, int targetPrice);
        public void UpdateCompositeOperator(int clientId, int storeId, int ruleId, int Operator);
        public void UpdateCompositeRules(int clientId, int storeId, int ruleId, List<int> rules);
        public void AddPurchasePolicy(int clientId, int storeId, DateTime expirationDate, string subject, int ruleId);
        public int AddDiscountPolicy(int clientId, int storeId, DateTime expirationDate, string subject, int ruleId, double precentage);
        public void AddCompositePolicy(int clientId, int storeId, DateTime expirationDate, string subject, int Operator, List<int> policies);

    }
}
