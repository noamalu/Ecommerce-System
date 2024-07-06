using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Payment;
using MarketBackend.Domain.Shipping;
using MarketBackend.Services.Models;

namespace MarketBackend.Services.Interfaces
{
    public interface IMarketService
    {
        public Task<Response<int>> AddProduct(int storeId, string identifier, string name, string sellMethod, string description, double price, string category, int quantity, bool ageLimit);
        public Task<Response> RemoveProduct(int storeId,string identifier, int productId);
        public Task<Response> RemoveStaffMember(int storeId, string identifier, string toAddUserName);            
        public Task<Response> AddStaffMember(int storeId, string identifier, string roleName, string toAddUserName);   
        public Task<Response> AddManger(string identifier, int storeId, string toAddUserName);
        public Task<Response> RemoveManger(string identifier, int storeId, string toRemoveUserName);
        public Task<Response> AddOwner(string identifier, int storeId, string toAddUserName);
        public Task<Response> RemoveOwner(string identifier, int storeId, string toRemoveUserName);
        public Task<Response<List<Member>>> GetOwners(int storeId);
        public Task<Response<List<Member>>> GetMangers(int storeId);
        public Task<Response<Member>> GetFounder(int storeId);
        public Task<Response> UpdateProductQuantity(int storeId, string identifier, int productId, int quantity); 
        public Task<Response> UpdateProductPrice(int storeId, string identifier,  int productId, double price);
        public Task<Response> CloseStore(string identifier, int storeId);
        public Task<Response> OpenStore(string identifier, int storeId);
        public Task<Response<bool>> IsAvailable(int productId);
        public Task<Response> RemovePermission(string identifier, int storeId, string toRemoveUserName, string permission);
        public Task<Response> AddPermission(string identifier, int storeId, string toAddUserName, string permission);
        public Task<Response> EditPurchasePolicy(int storeId);
        public Task<Response<List<ProductResultDto>>> SearchByKeywords(string keywords);
        public Task<Response<List<ProductResultDto>>> SearchByName(string name);
        public Task<Response<List<ProductResultDto>>> SearchByCategory(string category);
        public Task<Response<string>> GetInfo(int storeId);
        public Task<Response<string>> GetProductInfo(int storeId, int productId);
        public Task<Response> PurchaseCart(string identifier, PaymentDetails paymentDetails, ShippingDetails shippingDetails);
        public Task<Response<List<PurchaseResultDto>>> GetPurchaseHistoryByStore(int storeId, string identifier);
        public Task<Response> RemovePolicy(string identifier, int storeId, int policyID,string type);
        public Task<Response<int>> AddSimpleRule(string identifier, int storeId,string subject);
        public Task<Response<int>> AddQuantityRule(string identifier, int storeId, string subject, int minQuantity, int maxQuantity);
        public Task<Response<int>> AddTotalPriceRule(string identifier, int storeId, string subject, int targetPrice);
        public Task<Response<int>> AddCompositeRule(string identifier, int storeId, int Operator, List<int> rules);
        public Task<Response> UpdateRuleSubject(string identifier, int storeId, int ruleId, string subject);
        public Task<Response> UpdateRuleQuantity(string identifier, int storeId, int ruleId, int minQuantity, int maxQuantity);
        public Task<Response> UpdateRuleTargetPrice(string identifier, int storeId, int ruleId, int targetPrice);
        public Task<Response> UpdateCompositeOperator(string identifier, int storeId, int ruleId, int Operator);
        public Task<Response> UpdateCompositeRules(string identifier, int storeId, int ruleId, List<int> rules);
        public Task<Response<int>> AddPurchasePolicy(string identifier, int storeId, DateTime expirationDate, string subject, int ruleId);
        public Task<Response<int>> AddDiscountPolicy(string identifier, int storeId, DateTime expirationDate, string subject, int ruleId, double precentage);
        public Task<Response<int>> AddCompositePolicy(string identifier, int storeId, DateTime expirationDate, string subject, int Operator, List<int> policies);
        public Task<Response<string>> GetStoreById(int storeId);
        public Task<Response<List<RuleResultDto>>> GetStoreRules(int storeId, string identifier);
        public Task<Response<List<DiscountPolicyResultDto>>> GetStoreDiscountPolicies(int storeId, string identifier);
        public Task<Response<List<PolicyResultDto>>> GetStorePurchacePolicies(int storeId, string identifier);
        public Task<Response> AddKeyWord(string identifier, string keyWord, int storeId, int productId);
        public Task<Response<List<StoreResultDto>>> GetStores();
    }
}
