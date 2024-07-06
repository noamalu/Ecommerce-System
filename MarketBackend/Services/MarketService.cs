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
        private MarketManagerFacade _marketManagerFacade;
        private Logger _logger;

        // for testing external systems
        // IShippingSystemFacade shippingSystem = new RealShippingSystem("https://damp-lynna-wsep-1984852e.koyeb.app/");

        // IPaymentSystemFacade paymentSystem = new RealPaymentSystem("https://damp-lynna-wsep-1984852e.koyeb.app/");

        public MarketService(IShippingSystemFacade shippingSystemFacade, IPaymentSystemFacade paymentSystem)
        {
            _marketManagerFacade = MarketManagerFacade.GetInstance(shippingSystemFacade, paymentSystem);
            _logger = MyLogger.GetLogger();
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
        
        public async Task<Response> AddManger(string identifier, int storeId, string toAddUserName)
        {
            try
            {
                await _marketManagerFacade.AddManger(identifier, storeId, toAddUserName);
                _logger.Info($"client {identifier} added {toAddUserName} as manager.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in adding {toAddUserName} by {identifier} as a manager. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public async Task<Response> AddOwner(string identifier, int storeId, string toAddUserName)
        {
            try
            {
                await _marketManagerFacade.AddOwner(identifier, storeId, toAddUserName);
                _logger.Info($"client {identifier} added {toAddUserName} as owner.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in adding {toAddUserName} by {identifier} as a owner. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public async Task<Response> AddPermission(string identifier, int storeId, string toAddUserName, string permission)
        {
            try
            {
                await _marketManagerFacade.AddPermission(identifier, storeId, toAddUserName, permission.StringToPermission());
                _logger.Info($"Client {identifier} added permission {permission} to client {toAddUserName} in store {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in adding permission {permission} by client {identifier} to client {toAddUserName} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public async Task<Response<int>> AddProduct(int storeId, string identifier, string name, string sellMethod, string description, double price, string category, int quantity, bool ageLimit)
        {
            try
            {
                var product = await _marketManagerFacade.AddProduct(storeId, identifier, name, sellMethod, description, price, category, quantity, ageLimit);
                _logger.Info($"Client {identifier} added product {name} store {storeId} with sellmethod {sellMethod}, description {description}, category {category}, price {price}, quantity {quantity}, ageLimit {ageLimit}.");
                return Response<int>.FromValue(product._productId);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in adding product {name} to store {storeId} by client {identifier}, with  sellmethod {sellMethod}, description {description}, category {category}, price {price}, quantity {quantity}, ageLimit {ageLimit}. Error message: {e.Message}");
                return Response<int>.FromError(e.Message);
            }
        }

        public async Task<Response> CloseStore(string identifier, int storeId)
        {
            try
            {
                await _marketManagerFacade.CloseStore(identifier, storeId);
                _logger.Info($"Client {identifier} closed store {storeId}");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in closing store {storeId} by client {identifier}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public async Task<Response> EditPurchasePolicy(int storeId)
        {
            try
            {
                await _marketManagerFacade.EditPurchasePolicy(storeId);
                _logger.Info($"Purchase policy was edited in store {storeId}");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in editing purchase policy in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public async Task<Response<Member>> GetFounder(int storeId)
        {
            try
            {
                Member founder = await _marketManagerFacade.GetFounder(storeId);
                _logger.Info($"Founder for store {storeId} got.");
                return Response<Member>.FromValue(founder);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in getting founfer for store {storeId}. Error message: {e.Message}");
                return Response<Member>.FromError(e.Message);
            }
        }

        public async Task<Response<List<Member>>> GetMangers(int storeId)
        {
            try
            {
                List<Member> managers = await _marketManagerFacade.GetMangers(storeId);
                _logger.Info($"Managers for store {storeId} got.");
                return Response<List<Member>>.FromValue(managers);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in getting managers for store {storeId}. Error message: {e.Message}");
                return Response<List<Member>>.FromError(e.Message);
            }
        }

        public async Task<Response<List<Member>>> GetOwners(int storeId)
        {
            try
            {
                List<Member> owners = await _marketManagerFacade.GetOwners(storeId);
                _logger.Info($"Owners for store {storeId} got.");
                return Response<List<Member>>.FromValue(owners);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in getting owners for store {storeId}. Error message: {e.Message}");
                return Response<List<Member>>.FromError(e.Message);
            }
        }

        public async Task<Response<bool>> IsAvailable(int productId)
        {
            try
            {
                bool ans = await _marketManagerFacade.IsAvailable(productId);
                _logger.Info($"product {productId} available: {ans}.");
                return Response<bool>.FromValue(ans);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in checking available of product {productId}. Error message: {e.Message}");
                return Response<bool>.FromError(e.Message);
            }
        }

        public async Task<Response> OpenStore(string identifier, int storeId)
        {
            try
            {
                await _marketManagerFacade.OpenStore(identifier, storeId);
                _logger.Info($"client {identifier} opened store {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in open store {storeId} by client {identifier}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public async Task<Response> RemoveManger(string identifier, int storeId, string toRemoveUserName)
        {
            try
            {
                await _marketManagerFacade.RemoveManger(identifier, storeId, toRemoveUserName);
                _logger.Info($"client {identifier} removed manager of client {toRemoveUserName} in store {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in removing manager of client {toRemoveUserName} by client {identifier} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public async Task<Response> RemoveOwner(string identifier, int storeId, string toRemoveUserName)
        {
            try
            {
                await _marketManagerFacade.RemoveOwner(identifier, storeId, toRemoveUserName);
                _logger.Info($"client {identifier} removed owner of client {toRemoveUserName} in store {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in removing owner of client {toRemoveUserName} by client {identifier} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public async Task<Response> RemovePermission(string identifier, int storeId, string toRemoveUserName, string permission)
        {
            try
            {
                await _marketManagerFacade.RemovePermission(identifier, storeId, toRemoveUserName, permission.StringToPermission());
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public async Task<Response> RemoveProduct(int storeId,string identifier, int productId)
        {
            try
            {
                await _marketManagerFacade.RemoveProduct(storeId, identifier, productId);
                _logger.Info($"Client {identifier} removed product {productId} from store {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in removing product {productId} from store {storeId} by client {identifier}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public async Task<Response> RemoveStaffMember(int storeId, string identifier, string toRemoveUserName)
        {
            try
            {
                await _marketManagerFacade.RemoveStaffMember(storeId, identifier, toRemoveUserName);
                _logger.Info($"client {identifier} removed client {toRemoveUserName} from store {storeId} staff");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in removing client {toRemoveUserName} by client {identifier} from store {storeId} staff. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public async Task<Response> AddStaffMember(int storeId, string identifier, string roleName, string toAddUserName)
        {
            try
            {
                await _marketManagerFacade.AddStaffMember(storeId, identifier, roleName, toAddUserName);
                _logger.Info($"client {identifier} added role {roleName} for client {toAddUserName} in store {storeId}");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in adding role {roleName} for client {toAddUserName} by client {identifier} in store {storeId}. Error message: {e.Message}");
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

        public async Task<Response> UpdateProductPrice(int storeId, string identifier,  int productId, double price)
        {
            try
            {
                await _marketManagerFacade.UpdateProductPrice(storeId, identifier, productId, price);
                _logger.Info($"Product {productId} price was updated to {price}.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in updating product {productId} price to {price}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public async Task<Response> UpdateProductQuantity(int storeId, string identifier, int productId, int quantity)
        {
            try
            {
                await _marketManagerFacade.UpdateProductQuantity(storeId, identifier,productId, quantity);
                _logger.Info($"Product {productId} quantity was updated to {quantity}.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in updating product {productId} quantoty to {quantity}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public async Task<Response<List<ProductResultDto>>> SearchByKeywords(string keywords)
        {
            try
            {
                var products = await _marketManagerFacade.SearchByKeyWords(keywords);
                
                // logger.Info($"Search by keyWords {keywords} succeed.");
                return Response<List<ProductResultDto>>.FromValue(products.Select(product => new ProductResultDto(product)).ToList());
            }
            catch (Exception e)
            {
                // logger.Error($"Error in search by keyword {keywords}. Error message: {e.Message}");
                return Response<List<ProductResultDto>>.FromError(e.Message);
            }
        }

        public async Task<Response<List<ProductResultDto>>> SearchByName(string name)
        {
            string lowerName = name.ToLower();
            try
            {
                HashSet<Product> products = await _marketManagerFacade.SearchByName(name);
                // logger.Info($"Search by name {name} succeed.");
                return Response<List<ProductResultDto>>.FromValue(products.Select(product => new ProductResultDto(product)).ToList());
            }
            catch (Exception e)
            {
                // logger.Error($"Error in search by name {name}. Error message: {e.Message}");
                return Response<List<ProductResultDto>>.FromError(e.Message);
            }
        }

        public async Task<Response<List<ProductResultDto>>> SearchByCategory(string category)
        {
            try
            {
                HashSet<Product> products = await _marketManagerFacade.SearchByCategory(category);
                // logger.Info($"Search by category {category} succeed.");
                return Response<List<ProductResultDto>>.FromValue(products.Select(product => new ProductResultDto(product)).ToList());
            }
            catch (Exception e)
            {
                // logger.Error($"Error in search by category {category}. Error message: {e.Message}");
                return Response<List<ProductResultDto>>.FromError(e.Message);
            }
        }

        public async Task<Response<string>> GetInfo(int storeId){
            try
            {
                string info = await _marketManagerFacade.GetInfo(storeId);
                _logger.Info($"Info got for store {storeId}");
                return Response<string>.FromValue(info);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in getting info for store {storeId}. Error message: {e.Message}");
                return Response<string>.FromError(e.Message);
            }
        }

        public async Task<Response<string>> GetProductInfo(int storeId, int productId){
            try
            {
                string info = await _marketManagerFacade.GetProductInfo(storeId, productId);
                _logger.Info($"Info got for product {productId} in store {storeId}");
                return Response<string>.FromValue(info);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in getting info for product {productId} in store {storeId}. Error message: {e.Message}");
                return Response<string>.FromError(e.Message);
            }
        }

        public async Task<Response<Product>> GetProduct(int storeId, int productId){
            try
            {
                Product product = await _marketManagerFacade.GetProduct(storeId, productId);
                _logger.Info($"Product {productId} in store {storeId} got");
                return Response<Product>.FromValue(product);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in getting product {productId} in store {storeId}. Error message: {e.Message}");
                return Response<Product>.FromError(e.Message);
            }
        }

        public async Task<Response> PurchaseCart(string identifier, PaymentDetails paymentDetails, ShippingDetails shippingDetails)
        {
             try
            {
                await _marketManagerFacade.PurchaseCart(identifier, paymentDetails, shippingDetails);
                _logger.Info($"Purchase cart for client {identifier} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in purchase cart for client {identifier}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public async Task<Response> AddKeyWord(string identifier, string keyWord, int storeId, int productId)
        {
             try
            {
                await _marketManagerFacade.AddKeyWord(keyWord, storeId, productId);
                _logger.Info($"Add keyWord for product {productId} in store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in Adding keyWord for product {productId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public async Task<Response<List<PurchaseResultDto>>> GetPurchaseHistoryByStore(int storeId, string identifier)
        {
             try
            {
                
                List<Purchase> purchases = await _marketManagerFacade.GetPurchaseHistoryByStore(storeId, identifier);
                //log
                return Response<List<PurchaseResultDto>>.FromValue(purchases.Select(purchase => new PurchaseResultDto(purchase)).ToList());
            }
            catch (Exception e)
            {
                _logger.Error($"Error in getting purchase history for store {storeId}, client {identifier}, Error message: {e.Message}");
                return Response<List<PurchaseResultDto>>.FromError(e.Message);
            }
        }
        public async Task<Response> RemovePolicy (string identifier, int storeId, int policyID,string type)
        {
            try
            {
                await _marketManagerFacade.RemovePolicy(identifier, storeId, policyID, type);
                _logger.Info($"Remove policy {policyID} for store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in removing policy {policyID} for store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public async Task<Response<int>> AddSimpleRule(string identifier, int storeId,string subject)
        {
            try
            {
                int rule = await _marketManagerFacade.AddSimpleRule(identifier, storeId, subject);
                _logger.Info($"Add simple rule for store {storeId} succeed.");
                return Response<int>.FromValue(rule);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in adding simple rule for store {storeId}. Error message: {e.Message}");
                return Response<int>.FromError(e.Message);
            }
        }
        public async Task<Response<int>> AddQuantityRule(string identifier, int storeId, string subject, int minQuantity, int maxQuantity)
        {
            try
            {
                int rule = await _marketManagerFacade.AddQuantityRule(identifier, storeId, subject, minQuantity, maxQuantity);
                _logger.Info($"Add quantity rule for store {storeId} succeed.");
                return Response<int>.FromValue(rule);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in adding quantity rule for store {storeId}. Error message: {e.Message}");
                return Response<int>.FromError(e.Message);
            }
        }
        public async Task<Response<int>> AddTotalPriceRule(string identifier, int storeId, string subject, int targetPrice)
        {
            try
            {
                int rule = await _marketManagerFacade.AddTotalPriceRule(identifier, storeId, subject, targetPrice);
                _logger.Info($"Add total price rule for store {storeId} succeed.");
                return Response<int>.FromValue(rule);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in adding total price rule for store {storeId}. Error message: {e.Message}");
                return Response<int>.FromError(e.Message);
            }
        }
        public async Task<Response<int>> AddCompositeRule(string identifier, int storeId, int Operator, List<int> rules)
        {
            try
            {
                int rule = await _marketManagerFacade.AddCompositeRule(identifier, storeId, Operator, rules);
                _logger.Info($"Add composite rule for store {storeId} succeed.");
                return Response<int>.FromValue(rule);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in adding composite rule for store {storeId}. Error message: {e.Message}");
                return Response<int>.FromError(e.Message);
            }
        }
        public async Task<Response> UpdateRuleSubject(string identifier, int storeId, int ruleId, string subject)
        {
            try
            {
                await _marketManagerFacade.UpdateRuleSubject(identifier, storeId, ruleId, subject);
                _logger.Info($"Update rule subject for rule {ruleId} in store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in updating rule subject for rule {ruleId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public async Task<Response> UpdateRuleQuantity(string identifier, int storeId, int ruleId, int minQuantity, int maxQuantity)
        {
            try
            {
                await _marketManagerFacade.UpdateRuleQuantity(identifier, storeId, ruleId, minQuantity, maxQuantity);
                _logger.Info($"Update rule quantity for rule {ruleId} in store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in updating rule quantity for rule {ruleId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public async Task<Response> UpdateRuleTargetPrice(string identifier, int storeId, int ruleId, int targetPrice)
        {
            try
            {
                await _marketManagerFacade.UpdateRuleTargetPrice(identifier, storeId, ruleId, targetPrice);
                _logger.Info($"Update rule target price for rule {ruleId} in store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in updating rule target price for rule {ruleId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public async Task<Response> UpdateCompositeOperator(string identifier, int storeId, int ruleId, int Operator)
        {
            try
            {
                await _marketManagerFacade.UpdateCompositeOperator(identifier, storeId, ruleId, Operator);
                _logger.Info($"Update composite operator for rule {ruleId} in store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in updating composite operator for rule {ruleId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public async Task<Response> UpdateCompositeRules(string identifier, int storeId, int ruleId, List<int> rules)
        {
            try
            {
                await _marketManagerFacade.UpdateCompositeRules(identifier, storeId, ruleId, rules);
                _logger.Info($"Update composite rules for rule {ruleId} in store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in updating composite rules for rule {ruleId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public async Task<Response<int>> AddPurchasePolicy(string identifier, int storeId, DateTime expirationDate, string subject, int ruleId)
        {
            try
            {
                int policy = await _marketManagerFacade.AddPurchasePolicy(identifier, storeId, expirationDate, subject, ruleId);
                _logger.Info($"Add purchase policy for store {storeId} succeed.");
                return Response<int>.FromValue(policy);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in adding purchase policy for store {storeId}. Error message: {e.Message}");
                return Response<int>.FromError(e.Message);
            }
        }
        public async Task<Response<int>> AddDiscountPolicy(string identifier, int storeId, DateTime expirationDate, string subject, int ruleId, double precentage)
        {
            try
            {
                int policy = await _marketManagerFacade.AddDiscountPolicy(identifier, storeId, expirationDate, subject, ruleId, precentage);
                _logger.Info($"Add discount policy for store {storeId} succeed.");
                return Response<int>.FromValue(policy);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in adding discount policy for store {storeId}. Error message: {e.Message}");
                return Response<int>.FromError(e.Message);
            }
        }
        public async Task<Response<int>> AddCompositePolicy(string identifier, int storeId, DateTime expirationDate, string subject, int Operator, List<int> policies)
        {
            try
            {
                var policy = await _marketManagerFacade.AddCompositePolicy(identifier, storeId, expirationDate, subject, Operator, policies);
                _logger.Info($"Add composite policy for store {storeId} succeed.");
                return Response<int>.FromValue(policy);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in adding composite policy for store {storeId}. Error message: {e.Message}");
                return Response<int>.FromError(e.Message);
            }
        }

        public async Task<Response<string>> GetStoreById(int storeId)
        {
            try
            {
                var store = await _marketManagerFacade.GetStore(storeId);
                _logger.Info($"Get store {storeId} succeed.");
                return Response<string>.FromValue(store.Name);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in fetching store {storeId}. Error message: {e.Message}");
                return Response<string>.FromError(e.Message);
            }
        }

        public async Task<Response<List<RuleResultDto>>> GetStoreRules(int storeId, string identifier)
        {
            try
            {
                var store = await _marketManagerFacade.GetStore(storeId);
                _logger.Info($"Get store {storeId} succeed.");
                return Response<List<RuleResultDto>>.FromValue(store._rules.Values.Select(rule => new RuleResultDto(rule)).ToList());
            }
            catch (Exception e)
            {
                _logger.Error($"Error in fetching store {storeId}. Error message: {e.Message}");
                return Response<List<RuleResultDto>>.FromError(e.Message);
            }
        }

        public async Task<Response<List<DiscountPolicyResultDto>>> GetStoreDiscountPolicies(int storeId, string identifier)
        {
            try
            {
                var store = await _marketManagerFacade.GetStore(storeId);
                _logger.Info($"Get store {storeId} succeed.");
                return Response<List<DiscountPolicyResultDto>>.FromValue(store._discountPolicyManager.Policies.Values.Select(policy => new DiscountPolicyResultDto((DiscountPolicy)policy)).ToList());
            }
            catch (Exception e)
            {
                _logger.Error($"Error in fetching store {storeId}. Error message: {e.Message}");
                return Response<List<DiscountPolicyResultDto>>.FromError(e.Message);
            }
        }

        public async Task<Response<List<PolicyResultDto>>> GetStorePurchacePolicies(int storeId, string identifier)
        {
            try
            {
                var store = await _marketManagerFacade.GetStore(storeId);
                _logger.Info($"Get store {storeId} succeed.");
                return Response<List<PolicyResultDto>>.FromValue(store._purchasePolicyManager.Policies.Values.Select(policy => new PolicyResultDto(policy)).ToList());
            }
            catch (Exception e)
            {
                _logger.Error($"Error in fetching store {storeId}. Error message: {e.Message}");
                return Response<List<PolicyResultDto>>.FromError(e.Message);
            }
        }

        public async Task<Response<List<StoreResultDto>>> GetStores()
        {
            try
            {
                var stores = await _marketManagerFacade.GetStores();
                _logger.Info($"Get stores succeed.");
                return Response<List<StoreResultDto>>.FromValue(stores.Select(store => new StoreResultDto(store)).ToList());
            }
            catch (Exception e)
            {
                _logger.Error($"Error in fetching stores. Error message: {e.Message}");
                return Response<List<StoreResultDto>>.FromError(e.Message);
            }
        }
    }
}
