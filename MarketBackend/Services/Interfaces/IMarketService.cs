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
        public Response<int> AddProduct(int storeId, int userId, string name, string sellMethod, string description, double price, string category, int quantity, bool ageLimit);
        public Response RemoveProduct(int storeId,int userId, int productId);
        public Response RemoveStaffMember(int storeId, int activeId, Role role, int toRemoveId);
        public Response AddStaffMember(int storeId, int activeId, Role role, int toAddId);
        public Response AddManger(int activeId, int storeId, int toAddId);
        public Response RemoveManger(int activeId, int storeId, int toRemoveId);
        public Response AddOwner(int activeId, int storeId, int toAddId);
        public Response RemoveOwner(int activeId, int storeId, int toRemoveId);
        public Response<List<Member>> GetOwners(int storeId);
        public Response<List<Member>> GetMangers(int storeId);
        public Response<Member> GetFounder(int storeId);
        public Response UpdateProductQuantity(int storeId, int userId, int productId, int quantity);
        public Response UpdateProductPrice(int storeId, int userId,  int productId, double price);
        public Response CloseStore(int clientId, int storeId);
        public Response OpenStore(int clientId, int storeId);
        public Response<bool> IsAvailable(int productId);
        public Response RemovePermission(int activeId, int storeId, int toRemoveId);
        public Response AddPermission(int activeId, int storeId, int toAddId, Permission permission);
        public Response EditPurchasePolicy(int storeId);
        public Response<List<ProductResultDto>> SearchByKeywords(string keywords);
        public Response<List<ProductResultDto>> SearchByName(string name);
        public Response<List<ProductResultDto>> SearchByCategory(string category);
        public Response<string> GetInfo(int storeId);
        public Response<string> GetProductInfo(int storeId, int productId);
        public Response PurchaseCart(int id, PaymentDetails paymentDetails, ShippingDetails shippingDetails);
        public Response<List<PurchaseResultDto>> GetPurchaseHistory(int storeId, int clientId);
        public Response RemovePolicy(int clientId, int storeId, int policyID,string type);
        public Response AddSimpleRule(int clientId, int storeId,string subject);
        public Response AddQuantityRule(int clientId, int storeId, string subject, int minQuantity, int maxQuantity);
        public Response AddTotalPriceRule(int clientId, int storeId, string subject, int targetPrice);
        public Response AddCompositeRule(int clientId, int storeId, int Operator, List<int> rules);
        public Response UpdateRuleSubject(int clientId, int storeId, int ruleId, string subject);
        public Response UpdateRuleQuantity(int clientId, int storeId, int ruleId, int minQuantity, int maxQuantity);
        public Response UpdateRuleTargetPrice(int clientId, int storeId, int ruleId, int targetPrice);
        public Response UpdateCompositeOperator(int clientId, int storeId, int ruleId, int Operator);
        public Response UpdateCompositeRules(int clientId, int storeId, int ruleId, List<int> rules);
        public Response AddPurchasePolicy(int clientId, int storeId, DateTime expirationDate, string subject, int ruleId);
        public Response AddDiscountPolicy(int clientId, int storeId, DateTime expirationDate, string subject, int ruleId, double precentage);
        public Response AddCompositePolicy(int clientId, int storeId, DateTime expirationDate, string subject, int Operator, List<int> policies);


    }
}
