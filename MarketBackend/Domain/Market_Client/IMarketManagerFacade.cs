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
        Task Register(string username, string password, string email, int age);
        Task EnterAsGuest(string identifier);
        Task PurchaseCart(string identifier, PaymentDetails paymentDetails, ShippingDetails shippingDetails);
        Task<int> CreateStore(string identifier, string storeName, string email, string phoneNum);
        Task<bool> ResToStoreManageReq(string identifier);
        Task<bool> ResToStoreOwnershipReq(string identifier); //respond to store ownership request
        Task LogoutClient(string identifier);
        Task RemoveFromCart(string identifier, int productId, int basketId, int quantity);
        Task<ShoppingCart> ViewCart(string identifier);
        Task AddToCart(string identifier, int storeId, int productId, int quantity);
        Task<string> LoginClient(string username, string password);
        Task ExitGuest(string identifier);
        // void UpdateProductDiscount(int productId, double discount);
        Task<List<ShoppingCartHistory>> GetPurchaseHistoryByClient(string userName);
        Task<List<Purchase>> GetPurchaseHistoryByStore(int storeId, string userName);
        Task<Product> AddProduct(int storeId, string identifier, string name, string sellMethod, string description, double price, string category, int quantity, bool ageLimit);
        Task RemoveProduct(int storeId,string identifier, int productId);
        Task RemoveStaffMember(int storeId, string identifier, string toRemoveUserName);
        Task AddManger(string identifier, int storeId, string toAddUserName);
        Task RemoveManger(string identifier, int storeId, string toRemoveUserName);
        Task AddOwner(string identifier, int storeId, string toAddUserName);
        Task RemoveOwner(string identifier, int storeId, string toRemoveUserName);
        Task<List<Member>> GetOwners(int storeId);
        Task<List<Member>> GetMangers(int storeId);
        Task<Member> GetFounder(int storeId);

        Task UpdateProductQuantity(int storeId, string identifier, int productId, int quantity); 
        Task UpdateProductPrice(int storeId, string identifier,  int productId, double price);
        Task CloseStore(string identifier, int storeId);
        Task OpenStore(string identifier, int storeId);
        Task<bool> IsAvailable(int storeId);
        Task RemovePermission(string identifier, int storeId, string toRemoveUserName, Permission permission);
        Task AddPermission(string identifier, int storeId, string toAddUserName, Permission permission);
        Task EditPurchasePolicy(int storeId);
        Task<HashSet<Product>> SearchByKeyWords(string keywords);
        Task<HashSet<Product>> SearchByName(string name);
        Task<HashSet<Product>> SearchByCategory(string category);
        Task<HashSet<Product>> SearchByCategoryWithStore(int storeId, string category);
        Task<HashSet<Product>> SearchByKeyWordsWithStore(int storeId, string keywords);
        Task<HashSet<Product>> SearchByNameWithStore(int storeId, string name);
        Task Filter (HashSet<Product> products, string category, double lowPrice, double highPrice, double lowProductRate, double highProductRate, double lowStoreRate, double highStoreRate);
        Task<string> GetProductInfo(int storId, int productId);
        public Task AddStaffMember(int storeId, string identifier, string roleName, string toAddUserName);   
        public Task<string> GetInfo(int storeId);    
        public Task RemovePolicy(string identifier, int storeId, int policyID,string type);
        public Task<int> AddSimpleRule(string identifier, int storeId,string subject);
        public Task<int> AddQuantityRule(string identifier, int storeId, string subject, int minQuantity, int maxQuantity);
        public Task<int> AddTotalPriceRule(string identifier, int storeId, string subject, int targetPrice);
        public Task<int> AddCompositeRule(string identifier, int storeId, int Operator, List<int> rules);
        public Task UpdateRuleSubject(string identifier, int storeId, int ruleId, string subject);
        public Task UpdateRuleQuantity(string identifier, int storeId, int ruleId, int minQuantity, int maxQuantity);
        public Task UpdateRuleTargetPrice(string identifier, int storeId, int ruleId, int targetPrice);
        public Task UpdateCompositeOperator(string identifier, int storeId, int ruleId, int Operator);
        public Task UpdateCompositeRules(string identifier, int storeId, int ruleId, List<int> rules);
        public Task<int> AddPurchasePolicy(string identifier, int storeId, DateTime expirationDate, string subject, int ruleId);
        public Task<int> AddDiscountPolicy(string identifier, int storeId, DateTime expirationDate, string subject, int ruleId, double precentage);
        public Task<int> AddCompositePolicy(string identifier, int storeId, DateTime expirationDate, string subject, int Operator, List<int> policies);
        public Task NotificationOn(string identifier);
        public Task NotificationOff(string identifier);
    }
}
