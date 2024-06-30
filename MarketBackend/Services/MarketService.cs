using MarketBackend.Services.Interfaces;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Payment;
using NLog;
using MarketBackend.Domain.Shipping;
using MarketBackend.Services.Models;

namespace MarketBackend.Services
{
    public class MarketService : IMarketService
    {
        private static MarketService _marketService = null;
        private MarketManagerFacade marketManagerFacade;
        private Logger logger;
        private MarketService(IShippingSystemFacade shippingSystemFacade, IPaymentSystemFacade paymentSystem){
            marketManagerFacade = MarketManagerFacade.GetInstance(shippingSystemFacade, paymentSystem);
            logger = MyLogger.GetLogger();
        }

        public static MarketService GetInstance(IShippingSystemFacade shippingSystemFacade, IPaymentSystemFacade paymentSystem){
            if (_marketService == null){
                _marketService = new MarketService(shippingSystemFacade, paymentSystem);
            }
            return _marketService;
        }

        public void Dispose(){
            MarketManagerFacade.Dispose();
            _marketService = null;
        }
        
        public Response AddManger(string identifier, int storeId, string toAddUserName)
        {
            try
            {
                marketManagerFacade.AddManger(identifier, storeId, toAddUserName);
                logger.Info($"client {identifier} added {toAddUserName} as manager.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in adding {toAddUserName} by {identifier} as a manager. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response AddOwner(string identifier, int storeId, string toAddUserName)
        {
            try
            {
                marketManagerFacade.AddOwner(identifier, storeId, toAddUserName);
                logger.Info($"client {identifier} added {toAddUserName} as owner.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in adding {toAddUserName} by {identifier} as a owner. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response AddPermission(string identifier, int storeId, string toAddUserName, string permission)
        {
            try
            {
                marketManagerFacade.AddPermission(identifier, storeId, toAddUserName, permission.StringToPermission());
                logger.Info($"Client {identifier} added permission {permission} to client {toAddUserName} in store {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in adding permission {permission} by client {identifier} to client {toAddUserName} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response<int> AddProduct(int storeId, string identifier, string name, string sellMethod, string description, double price, string category, int quantity, bool ageLimit)
        {
            try
            {
                var product = marketManagerFacade.AddProduct(storeId, identifier, name, sellMethod, description, price, category, quantity, ageLimit);
                logger.Info($"Client {identifier} added product {name} store {storeId} with sellmethod {sellMethod}, description {description}, category {category}, price {price}, quantity {quantity}, ageLimit {ageLimit}.");
                return Response<int>.FromValue(product._productid);
            }
            catch (Exception e)
            {
                logger.Error($"Error in adding product {name} to store {storeId} by client {identifier}, with  sellmethod {sellMethod}, description {description}, category {category}, price {price}, quantity {quantity}, ageLimit {ageLimit}. Error message: {e.Message}");
                return Response<int>.FromError(e.Message);
            }
        }

        public Response CloseStore(string identifier, int storeId)
        {
            try
            {
                marketManagerFacade.CloseStore(identifier, storeId);
                logger.Info($"Client {identifier} closed store {storeId}");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in closing store {storeId} by client {identifier}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response EditPurchasePolicy(int storeId)
        {
            try
            {
                marketManagerFacade.EditPurchasePolicy(storeId);
                logger.Info($"Purchase policy was edited in store {storeId}");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in editing purchase policy in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response<Member> GetFounder(int storeId)
        {
            try
            {
                Member founder = marketManagerFacade.GetFounder(storeId);
                logger.Info($"Founder for store {storeId} got.");
                return Response<Member>.FromValue(founder);
            }
            catch (Exception e)
            {
                logger.Error($"Error in getting founfer for store {storeId}. Error message: {e.Message}");
                return Response<Member>.FromError(e.Message);
            }
        }

        public Response<List<Member>> GetMangers(int storeId)
        {
            try
            {
                List<Member> managers = marketManagerFacade.GetMangers(storeId);
                logger.Info($"Managers for store {storeId} got.");
                return Response<List<Member>>.FromValue(managers);
            }
            catch (Exception e)
            {
                logger.Error($"Error in getting managers for store {storeId}. Error message: {e.Message}");
                return Response<List<Member>>.FromError(e.Message);
            }
        }

        public Response<List<Member>> GetOwners(int storeId)
        {
            try
            {
                List<Member> owners = marketManagerFacade.GetOwners(storeId);
                logger.Info($"Owners for store {storeId} got.");
                return Response<List<Member>>.FromValue(owners);
            }
            catch (Exception e)
            {
                logger.Error($"Error in getting owners for store {storeId}. Error message: {e.Message}");
                return Response<List<Member>>.FromError(e.Message);
            }
        }

        public Response<bool> IsAvailable(int productId)
        {
            try
            {
                bool ans = marketManagerFacade.IsAvailable(productId);
                logger.Info($"product {productId} available: {ans}.");
                return Response<bool>.FromValue(ans);
            }
            catch (Exception e)
            {
                logger.Error($"Error in checking available of product {productId}. Error message: {e.Message}");
                return Response<bool>.FromError(e.Message);
            }
        }

        public Response OpenStore(string identifier, int storeId)
        {
            try
            {
                marketManagerFacade.OpenStore(identifier, storeId);
                logger.Info($"client {identifier} opened store {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in open store {storeId} by client {identifier}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response RemoveManger(string identifier, int storeId, string toRemoveUserName)
        {
            try
            {
                marketManagerFacade.RemoveManger(identifier, storeId, toRemoveUserName);
                logger.Info($"client {identifier} removed manager of client {toRemoveUserName} in store {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in removing manager of client {toRemoveUserName} by client {identifier} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response RemoveOwner(string identifier, int storeId, string toRemoveUserName)
        {
            try
            {
                marketManagerFacade.RemoveOwner(identifier, storeId, toRemoveUserName);
                logger.Info($"client {identifier} removed owner of client {toRemoveUserName} in store {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in removing owner of client {toRemoveUserName} by client {identifier} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response RemovePermission(string identifier, int storeId, string toRemoveUserName, string permission)
        {
            try
            {
                marketManagerFacade.RemovePermission(identifier, storeId, toRemoveUserName, permission.StringToPermission());
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response RemoveProduct(int storeId,string identifier, int productId)
        {
            try
            {
                marketManagerFacade.RemoveProduct(storeId, identifier, productId);
                logger.Info($"Client {identifier} removed product {productId} from store {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in removing product {productId} from store {storeId} by client {identifier}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response RemoveStaffMember(int storeId, string identifier, string toRemoveUserName)
        {
            try
            {
                marketManagerFacade.RemoveStaffMember(storeId, identifier, toRemoveUserName);
                logger.Info($"client {identifier} removed client {toRemoveUserName} from store {storeId} staff");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in removing client {toRemoveUserName} by client {identifier} from store {storeId} staff. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response AddStaffMember(int storeId, string identifier, string roleName, string toAddUserName)
        {
            try
            {
                marketManagerFacade.AddStaffMember(storeId, identifier, roleName, toAddUserName);
                logger.Info($"client {identifier} added role {roleName} for client {toAddUserName} in store {storeId}");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in adding role {roleName} for client {toAddUserName} by client {identifier} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        // public Response UpdateProductDiscount(int productId, double discount)
        // {
        //     try
        //     {
        //         marketManagerFacade.UpdateProductDiscount(productId, discount);
        //         logger.Info($"Product {productId} discount was updated to {discount}.");
        //         return new Response();
        //     }
        //     catch (Exception e)
        //     {
        //         logger.Error($"Error in updating product {productId} discount to {discount}. Error message: {e.Message}");
        //         return new Response(e.Message);
        //     }
        // }

        public Response UpdateProductPrice(int storeId, string identifier,  int productId, double price)
        {
            try
            {
                marketManagerFacade.UpdateProductPrice(storeId, identifier, productId, price);
                logger.Info($"Product {productId} price was updated to {price}.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in updating product {productId} price to {price}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response UpdateProductQuantity(int storeId, string identifier, int productId, int quantity)
        {
            try
            {
                marketManagerFacade.UpdateProductQuantity(storeId, identifier,productId, quantity);
                logger.Info($"Product {productId} quantity was updated to {quantity}.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in updating product {productId} quantoty to {quantity}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response<List<ProductResultDto>> SearchByKeywords(string keywords)
        {
            try
            {
                var products = marketManagerFacade.SearchByKeyWords(keywords);
                
                // logger.Info($"Search by keyWords {keywords} succeed.");
                return Response<List<ProductResultDto>>.FromValue(products.Select(product => new ProductResultDto(product)).ToList());
            }
            catch (Exception e)
            {
                // logger.Error($"Error in search by keyword {keywords}. Error message: {e.Message}");
                return Response<List<ProductResultDto>>.FromError(e.Message);
            }
        }

        public Response<List<ProductResultDto>> SearchByName(string name)
        {
            string lowerName = name.ToLower();
            try
            {
                HashSet<Product> products = marketManagerFacade.SearchByName(name);
                // logger.Info($"Search by name {name} succeed.");
                return Response<List<ProductResultDto>>.FromValue(products.Select(product => new ProductResultDto(product)).ToList());
            }
            catch (Exception e)
            {
                // logger.Error($"Error in search by name {name}. Error message: {e.Message}");
                return Response<List<ProductResultDto>>.FromError(e.Message);
            }
        }

        public Response<List<ProductResultDto>> SearchByCategory(string category)
        {
            try
            {
                HashSet<Product> products = marketManagerFacade.SearchByCategory(category);
                // logger.Info($"Search by category {category} succeed.");
                return Response<List<ProductResultDto>>.FromValue(products.Select(product => new ProductResultDto(product)).ToList());
            }
            catch (Exception e)
            {
                // logger.Error($"Error in search by category {category}. Error message: {e.Message}");
                return Response<List<ProductResultDto>>.FromError(e.Message);
            }
        }

        public Response<string> GetInfo(int storeId){
            try
            {
                string info = marketManagerFacade.GetInfo(storeId);
                logger.Info($"Info got for store {storeId}");
                return Response<string>.FromValue(info);
            }
            catch (Exception e)
            {
                logger.Error($"Error in getting info for store {storeId}. Error message: {e.Message}");
                return Response<string>.FromError(e.Message);
            }
        }

        public Response<string> GetProductInfo(int storeId, int productId){
            try
            {
                string info = marketManagerFacade.GetProductInfo(storeId, productId);
                logger.Info($"Info got for product {productId} in store {storeId}");
                return Response<string>.FromValue(info);
            }
            catch (Exception e)
            {
                logger.Error($"Error in getting info for product {productId} in store {storeId}. Error message: {e.Message}");
                return Response<string>.FromError(e.Message);
            }
        }

        public Response<Product> GetProduct(int storeId, int productId){
            try
            {
                Product product = marketManagerFacade.GetProduct(storeId, productId);
                logger.Info($"Product {productId} in store {storeId} got");
                return Response<Product>.FromValue(product);
            }
            catch (Exception e)
            {
                logger.Error($"Error in getting product {productId} in store {storeId}. Error message: {e.Message}");
                return Response<Product>.FromError(e.Message);
            }
        }

        public Response PurchaseCart(string identifier, PaymentDetails paymentDetails, ShippingDetails shippingDetails)
        {
             try
            {
                marketManagerFacade.PurchaseCart(identifier, paymentDetails, shippingDetails);
                logger.Info($"Purchase cart for client {identifier} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in purchase cart for client {identifier}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response AddKeyWord(string identifier, string keyWord, int storeId, int productId)
        {
             try
            {
                marketManagerFacade.AddKeyWord(keyWord, storeId, productId);
                logger.Info($"Add keyWord for product {productId} in store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in Adding keyWord for product {productId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response<List<PurchaseResultDto>> GetPurchaseHistoryByStore(int storeId, string identifier)
        {
             try
            {
                
                List<Purchase> purchases = marketManagerFacade.GetPurchaseHistoryByStore(storeId, identifier);
                //log
                return Response<List<PurchaseResultDto>>.FromValue(purchases.Select(purchase => new PurchaseResultDto(purchase)).ToList());
            }
            catch (Exception e)
            {
                logger.Error($"Error in getting purchase history for store {storeId}, client {identifier}, Error message: {e.Message}");
                return Response<List<PurchaseResultDto>>.FromError(e.Message);
            }
        }
        public Response RemovePolicy (string identifier, int storeId, int policyID,string type)
        {
            try
            {
                marketManagerFacade.RemovePolicy(identifier, storeId, policyID, type);
                logger.Info($"Remove policy {policyID} for store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in removing policy {policyID} for store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public Response<int> AddSimpleRule(string identifier, int storeId,string subject)
        {
            try
            {
                int rule = marketManagerFacade.AddSimpleRule(identifier, storeId, subject);
                logger.Info($"Add simple rule for store {storeId} succeed.");
                return Response<int>.FromValue(rule);
            }
            catch (Exception e)
            {
                logger.Error($"Error in adding simple rule for store {storeId}. Error message: {e.Message}");
                return Response<int>.FromError(e.Message);
            }
        }
        public Response<int> AddQuantityRule(string identifier, int storeId, string subject, int minQuantity, int maxQuantity)
        {
            try
            {
                int rule = marketManagerFacade.AddQuantityRule(identifier, storeId, subject, minQuantity, maxQuantity);
                logger.Info($"Add quantity rule for store {storeId} succeed.");
                return Response<int>.FromValue(rule);
            }
            catch (Exception e)
            {
                logger.Error($"Error in adding quantity rule for store {storeId}. Error message: {e.Message}");
                return Response<int>.FromError(e.Message);
            }
        }
        public Response<int> AddTotalPriceRule(string identifier, int storeId, string subject, int targetPrice)
        {
            try
            {
                int rule = marketManagerFacade.AddTotalPriceRule(identifier, storeId, subject, targetPrice);
                logger.Info($"Add total price rule for store {storeId} succeed.");
                return Response<int>.FromValue(rule);
            }
            catch (Exception e)
            {
                logger.Error($"Error in adding total price rule for store {storeId}. Error message: {e.Message}");
                return Response<int>.FromError(e.Message);
            }
        }
        public Response<int> AddCompositeRule(string identifier, int storeId, int Operator, List<int> rules)
        {
            try
            {
                int rule = marketManagerFacade.AddCompositeRule(identifier, storeId, Operator, rules);
                logger.Info($"Add composite rule for store {storeId} succeed.");
                return Response<int>.FromValue(rule);
            }
            catch (Exception e)
            {
                logger.Error($"Error in adding composite rule for store {storeId}. Error message: {e.Message}");
                return Response<int>.FromError(e.Message);
            }
        }
        public Response UpdateRuleSubject(string identifier, int storeId, int ruleId, string subject)
        {
            try
            {
                marketManagerFacade.UpdateRuleSubject(identifier, storeId, ruleId, subject);
                logger.Info($"Update rule subject for rule {ruleId} in store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in updating rule subject for rule {ruleId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public Response UpdateRuleQuantity(string identifier, int storeId, int ruleId, int minQuantity, int maxQuantity)
        {
            try
            {
                marketManagerFacade.UpdateRuleQuantity(identifier, storeId, ruleId, minQuantity, maxQuantity);
                logger.Info($"Update rule quantity for rule {ruleId} in store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in updating rule quantity for rule {ruleId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public Response UpdateRuleTargetPrice(string identifier, int storeId, int ruleId, int targetPrice)
        {
            try
            {
                marketManagerFacade.UpdateRuleTargetPrice(identifier, storeId, ruleId, targetPrice);
                logger.Info($"Update rule target price for rule {ruleId} in store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in updating rule target price for rule {ruleId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public Response UpdateCompositeOperator(string identifier, int storeId, int ruleId, int Operator)
        {
            try
            {
                marketManagerFacade.UpdateCompositeOperator(identifier, storeId, ruleId, Operator);
                logger.Info($"Update composite operator for rule {ruleId} in store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in updating composite operator for rule {ruleId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public Response UpdateCompositeRules(string identifier, int storeId, int ruleId, List<int> rules)
        {
            try
            {
                marketManagerFacade.UpdateCompositeRules(identifier, storeId, ruleId, rules);
                logger.Info($"Update composite rules for rule {ruleId} in store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in updating composite rules for rule {ruleId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public Response<int> AddPurchasePolicy(string identifier, int storeId, DateTime expirationDate, string subject, int ruleId)
        {
            try
            {
                int policy = marketManagerFacade.AddPurchasePolicy(identifier, storeId, expirationDate, subject, ruleId);
                logger.Info($"Add purchase policy for store {storeId} succeed.");
                return Response<int>.FromValue(policy);
            }
            catch (Exception e)
            {
                logger.Error($"Error in adding purchase policy for store {storeId}. Error message: {e.Message}");
                return Response<int>.FromError(e.Message);
            }
        }
        public Response<int> AddDiscountPolicy(string identifier, int storeId, DateTime expirationDate, string subject, int ruleId, double precentage)
        {
            try
            {
                int policy = marketManagerFacade.AddDiscountPolicy(identifier, storeId, expirationDate, subject, ruleId, precentage);
                logger.Info($"Add discount policy for store {storeId} succeed.");
                return Response<int>.FromValue(policy);
            }
            catch (Exception e)
            {
                logger.Error($"Error in adding discount policy for store {storeId}. Error message: {e.Message}");
                return Response<int>.FromError(e.Message);
            }
        }
        public Response<int> AddCompositePolicy(string identifier, int storeId, DateTime expirationDate, string subject, int Operator, List<int> policies)
        {
            try
            {
                var policy = marketManagerFacade.AddCompositePolicy(identifier, storeId, expirationDate, subject, Operator, policies);
                logger.Info($"Add composite policy for store {storeId} succeed.");
                return Response<int>.FromValue(policy);
            }
            catch (Exception e)
            {
                logger.Error($"Error in adding composite policy for store {storeId}. Error message: {e.Message}");
                return Response<int>.FromError(e.Message);
            }
        }

        public Response<string> GetStoreById(int storeId)
        {
            try
            {
                var store = marketManagerFacade.GetStore(storeId);
                logger.Info($"Get store {storeId} succeed.");
                return Response<string>.FromValue(store.Name);
            }
            catch (Exception e)
            {
                logger.Error($"Error in fetching store {storeId}. Error message: {e.Message}");
                return Response<string>.FromError(e.Message);
            }
        }

        public Response<List<RuleResultDto>> GetStoreRules(int storeId, string identifier)
        {
            try
            {
                var store = marketManagerFacade.GetStore(storeId);
                logger.Info($"Get store {storeId} succeed.");
                return Response<List<RuleResultDto>>.FromValue(store._rules.Values.Select(rule => new RuleResultDto(rule)).ToList());
            }
            catch (Exception e)
            {
                logger.Error($"Error in fetching store {storeId}. Error message: {e.Message}");
                return Response<List<RuleResultDto>>.FromError(e.Message);
            }
        }

        public Response<List<DiscountPolicyResultDto>> GetStoreDiscountPolicies(int storeId, string identifier)
        {
            try
            {
                var store = marketManagerFacade.GetStore(storeId);
                logger.Info($"Get store {storeId} succeed.");
                return Response<List<DiscountPolicyResultDto>>.FromValue(store._discountPolicyManager.Policies.Values.Select(policy => new DiscountPolicyResultDto((DiscountPolicy)policy)).ToList());
            }
            catch (Exception e)
            {
                logger.Error($"Error in fetching store {storeId}. Error message: {e.Message}");
                return Response<List<DiscountPolicyResultDto>>.FromError(e.Message);
            }
        }

        public Response<List<PolicyResultDto>> GetStorePurchacePolicies(int storeId, string identifier)
        {
            try
            {
                var store = marketManagerFacade.GetStore(storeId);
                logger.Info($"Get store {storeId} succeed.");
                return Response<List<PolicyResultDto>>.FromValue(store._purchasePolicyManager.Policies.Values.Select(policy => new PolicyResultDto(policy)).ToList());
            }
            catch (Exception e)
            {
                logger.Error($"Error in fetching store {storeId}. Error message: {e.Message}");
                return Response<List<PolicyResultDto>>.FromError(e.Message);
            }
        }

        public Response<List<StoreResultDto>> GetStores()
        {
            try
            {
                var stores = marketManagerFacade.GetStores();
                logger.Info($"Get stores succeed.");
                return Response<List<StoreResultDto>>.FromValue(stores.Select(store => new StoreResultDto(store)).ToList());
            }
            catch (Exception e)
            {
                logger.Error($"Error in fetching stores. Error message: {e.Message}");
                return Response<List<StoreResultDto>>.FromError(e.Message);
            }
        }
    }
}
