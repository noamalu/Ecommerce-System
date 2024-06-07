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
        // private Logger logger;
        private MarketService(IShippingSystemFacade shippingSystemFacade, IPaymentSystemFacade paymentSystem){
            marketManagerFacade = MarketManagerFacade.GetInstance(shippingSystemFacade, paymentSystem);
            // logger = MyLogger.GetLogger();
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
        
        public Response AddManger(int activeId, int storeId, int toAddId)
        {
            try
            {
                marketManagerFacade.AddManger(activeId, storeId, toAddId);
                // logger.Info($"client {activeId} added {toAddId} as manager.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in adding {toAddId} by {activeId} as a manager. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response AddOwner(int activeId, int storeId, int toAddId)
        {
            try
            {
                marketManagerFacade.AddOwner(activeId, storeId, toAddId);
                // logger.Info($"client {activeId} added {toAddId} as owner.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in adding {toAddId} by {activeId} as a owner. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response AddPermission(int activeId, int storeId, int toAddId, Permission permission)
        {
            try
            {
                marketManagerFacade.AddPermission(activeId, storeId, toAddId, permission);
                // logger.Info($"Client {activeId} added permission {permission} to client {toAddId} in store {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in adding permission {permission} by client {activeId} to client {toAddId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response<int> AddProduct(int storeId, int userId, string name, string sellMethod, string description, double price, string category, int quantity, bool ageLimit)
        {
            try
            {
                var product = marketManagerFacade.AddProduct(storeId, userId, name, sellMethod, description, price, category, quantity, ageLimit);
                // logger.Info($"Client {userId} added product {name} store {storeId} with sellmethod {sellMethod}, description {description}, category {category}, price {price}, quantity {quantity}, ageLimit {ageLimit}.");
                return Response<int>.FromValue(product._productid);
            }
            catch (Exception e)
            {
                // logger.Error($"Error in adding product {name} to store {storeId} by client {userId}, with  sellmethod {sellMethod}, description {description}, category {category}, price {price}, quantity {quantity}, ageLimit {ageLimit}. Error message: {e.Message}");
                return Response<int>.FromError(e.Message);
            }
        }

        public Response CloseStore(int clientId, int storeId)
        {
            try
            {
                marketManagerFacade.CloseStore(clientId, storeId);
                // logger.Info($"Client {clientId} closed store {storeId}");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in closing store {storeId} by client {clientId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response EditPurchasePolicy(int storeId)
        {
            try
            {
                marketManagerFacade.EditPurchasePolicy(storeId);
                // logger.Info($"Purchase policy was edited in store {storeId}");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in editing purchase policy in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response<Member> GetFounder(int storeId)
        {
            try
            {
                Member founder = marketManagerFacade.GetFounder(storeId);
                // logger.Info($"Founder for store {storeId} got.");
                return Response<Member>.FromValue(founder);
            }
            catch (Exception e)
            {
                // logger.Error($"Error in getting founfer for store {storeId}. Error message: {e.Message}");
                return Response<Member>.FromError(e.Message);
            }
        }

        public Response<List<Member>> GetMangers(int storeId)
        {
            try
            {
                List<Member> managers = marketManagerFacade.GetMangers(storeId);
                // logger.Info($"Managers for store {storeId} got.");
                return Response<List<Member>>.FromValue(managers);
            }
            catch (Exception e)
            {
                // logger.Error($"Error in getting managers for store {storeId}. Error message: {e.Message}");
                return Response<List<Member>>.FromError(e.Message);
            }
        }

        public Response<List<Member>> GetOwners(int storeId)
        {
            try
            {
                List<Member> owners = marketManagerFacade.GetOwners(storeId);
                // logger.Info($"Owners for store {storeId} got.");
                return Response<List<Member>>.FromValue(owners);
            }
            catch (Exception e)
            {
                // logger.Error($"Error in getting owners for store {storeId}. Error message: {e.Message}");
                return Response<List<Member>>.FromError(e.Message);
            }
        }

        public Response<bool> IsAvailable(int productId)
        {
            try
            {
                bool ans = marketManagerFacade.IsAvailable(productId);
                // logger.Info($"product {productId} available: {ans}.");
                return Response<bool>.FromValue(ans);
            }
            catch (Exception e)
            {
                // logger.Error($"Error in checking available of product {productId}. Error message: {e.Message}");
                return Response<bool>.FromError(e.Message);
            }
        }

        public Response OpenStore(int clientId, int storeId)
        {
            try
            {
                marketManagerFacade.OpenStore(clientId, storeId);
                // logger.Info($"client {clientId} opened store {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in open store {storeId} by client {clientId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response RemoveManger(int activeId, int storeId, int toRemoveId)
        {
            try
            {
                marketManagerFacade.RemoveManger(activeId, storeId, toRemoveId);
                // logger.Info($"client {activeId} removed manager of client {toRemoveId} in store {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in removing manager of client {toRemoveId} by client {activeId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response RemoveOwner(int activeId, int storeId, int toRemoveId)
        {
            try
            {
                marketManagerFacade.RemoveOwner(activeId, storeId, toRemoveId);
                // logger.Info($"client {activeId} removed owner of client {toRemoveId} in store {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in removing owner of client {toRemoveId} by client {activeId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response RemovePermission(int activeId, int storeId, int toRemoveId)
        {
            try
            {
                marketManagerFacade.RemovePermission(activeId, storeId, toRemoveId);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response RemoveProduct(int storeId,int userId, int productId)
        {
            try
            {
                marketManagerFacade.RemoveProduct(storeId, userId, productId);
                // logger.Info($"Client {userId} removed product {productId} from store {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in removing product {productId} from store {storeId} by client {userId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response RemoveStaffMember(int storeId, int activeId, Role role, int toRemoveId)
        {
            try
            {
                marketManagerFacade.RemoveStaffMember(storeId, activeId, role, toRemoveId);
                // logger.Info($"client {activeId} removed role {role} for client {toRemoveId} in store {storeId}");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in removing role {role} for client {toRemoveId} by client {activeId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response AddStaffMember(int storeId, int activeId, Role role, int toAddId)
        {
            try
            {
                marketManagerFacade.AddStaffMember(storeId, activeId, role, toAddId);
                // logger.Info($"client {activeId} added role {role} for client {toAddId} in store {storeId}");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in adding role {role} for client {toAddId} by client {activeId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        // public Response UpdateProductDiscount(int productId, double discount)
        // {
        //     try
        //     {
        //         marketManagerFacade.UpdateProductDiscount(productId, discount);
                // logger.Info($"Product {productId} discount was updated to {discount}.");
        //         return new Response();
        //     }
        //     catch (Exception e)
        //     {
                // logger.Error($"Error in updating product {productId} discount to {discount}. Error message: {e.Message}");
        //         return new Response(e.Message);
        //     }
        // }

        public Response UpdateProductPrice(int storeId, int userId, int productId, double price)
        {
            try
            {
                marketManagerFacade.UpdateProductPrice(storeId, userId, productId, price);
                // logger.Info($"Product {productId} price was updated to {price}.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in updating product {productId} price to {price}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response UpdateProductQuantity(int storeId, int userId, int productId, int quantity)
        {
            try
            {
                marketManagerFacade.UpdateProductQuantity(storeId, userId,productId, quantity);
                // logger.Info($"Product {productId} quantity was updated to {quantity}.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in updating product {productId} quantoty to {quantity}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response<HashSet<Product>> SearchByKeywords(string keywords)
        {
            try
            {
                HashSet<Product> products = marketManagerFacade.SearchByKeyWords(keywords);
                // logger.Info($"Search by keyWords {keywords} succeed.");
                return Response<HashSet<Product>>.FromValue(products);
            }
            catch (Exception e)
            {
                // logger.Error($"Error in search by keyword {keywords}. Error message: {e.Message}");
                return Response<HashSet<Product>>.FromError(e.Message);
            }
        }

        public Response<HashSet<Product>> SearchByName(string name)
        {
            string lowerName = name.ToLower();
            try
            {
                HashSet<Product> products = marketManagerFacade.SearchByName(name);
                // logger.Info($"Search by name {name} succeed.");
                return Response<HashSet<Product>>.FromValue(products);
            }
            catch (Exception e)
            {
                // logger.Error($"Error in search by name {name}. Error message: {e.Message}");
                return Response<HashSet<Product>>.FromError(e.Message);
            }
        }

        public Response<HashSet<Product>> SearchByCategory(string category)
        {
            try
            {
                HashSet<Product> products = marketManagerFacade.SearchByCategory(category);
                // logger.Info($"Search by category {category} succeed.");
                return Response<HashSet<Product>>.FromValue(products);
            }
            catch (Exception e)
            {
                // logger.Error($"Error in search by category {category}. Error message: {e.Message}");
                return Response<HashSet<Product>>.FromError(e.Message);
            }
        }

        public Response<string> GetInfo(int storeId){
            try
            {
                string info = marketManagerFacade.GetInfo(storeId);
                // logger.Info($"Info got for store {storeId}");
                return Response<string>.FromValue(info);
            }
            catch (Exception e)
            {
                // logger.Error($"Error in getting info for store {storeId}. Error message: {e.Message}");
                return Response<string>.FromError(e.Message);
            }
        }

        public Response<string> GetProductInfo(int storeId, int productId){
            try
            {
                string info = marketManagerFacade.GetProductInfo(storeId, productId);
                // logger.Info($"Info got for product {productId} in store {storeId}");
                return Response<string>.FromValue(info);
            }
            catch (Exception e)
            {
                // logger.Error($"Error in getting info for product {productId} in store {storeId}. Error message: {e.Message}");
                return Response<string>.FromError(e.Message);
            }
        }

        public Response PurchaseCart(int id, PaymentDetails paymentDetails, ShippingDetails shippingDetails)
        {
             try
            {
                marketManagerFacade.PurchaseCart(id, paymentDetails, shippingDetails);
                // logger.Info($"Purchase cart for client {id} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in purchase cart for client {id}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response AddKeyWord(int id, string keyWord, int storeId, int productId)
        {
             try
            {
                marketManagerFacade.AddKeyWord(keyWord, storeId, productId);
                // logger.Info($"Add keyWord for product {productId} in store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in Adding keyWord for product {productId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response<List<PurchaseResultDto>> GetPurchaseHistory(int storeId, int clientId)
        {
             try
            {
                List<Purchase> purchases = marketManagerFacade.GetPurchaseHistoryByStore(storeId, clientId);
                //log
                return Response<List<PurchaseResultDto>>.FromValue(purchases.Select(purchase => new PurchaseResultDto(purchase)).ToList());
            }
            catch (Exception e)
            {
                // logger.Error($"Error in getting purchase history for store {storeId}, client {clientId}, Error message: {e.Message}");
                return Response<List<PurchaseResultDto>>.FromError(e.Message);
            }
        }
        public Response RemovePolicy (int clientId, int storeId, int policyID,string type)
        {
            try
            {
                marketManagerFacade.RemovePolicy(clientId, storeId, policyID, type);
                // logger.Info($"Remove policy {policyID} for store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in removing policy {policyID} for store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public Response AddSimpleRule(int clientId, int storeId,string subject)
        {
            try
            {
                marketManagerFacade.AddSimpleRule(clientId, storeId, subject);
                // logger.Info($"Add simple rule for store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in adding simple rule for store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public Response AddQuantityRule(int clientId, int storeId, string subject, int minQuantity, int maxQuantity)
        {
            try
            {
                marketManagerFacade.AddQuantityRule(clientId, storeId, subject, minQuantity, maxQuantity);
                // logger.Info($"Add quantity rule for store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in adding quantity rule for store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public Response AddTotalPriceRule(int clientId, int storeId, string subject, int targetPrice)
        {
            try
            {
                marketManagerFacade.AddTotalPriceRule(clientId, storeId, subject, targetPrice);
                // logger.Info($"Add total price rule for store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in adding total price rule for store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public Response AddCompositeRule(int clientId, int storeId, int Operator, List<int> rules)
        {
            try
            {
                marketManagerFacade.AddCompositeRule(clientId, storeId, Operator, rules);
                // logger.Info($"Add composite rule for store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in adding composite rule for store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public Response UpdateRuleSubject(int clientId, int storeId, int ruleId, string subject)
        {
            try
            {
                marketManagerFacade.UpdateRuleSubject(clientId, storeId, ruleId, subject);
                // logger.Info($"Update rule subject for rule {ruleId} in store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in updating rule subject for rule {ruleId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public Response UpdateRuleQuantity(int clientId, int storeId, int ruleId, int minQuantity, int maxQuantity)
        {
            try
            {
                marketManagerFacade.UpdateRuleQuantity(clientId, storeId, ruleId, minQuantity, maxQuantity);
                // logger.Info($"Update rule quantity for rule {ruleId} in store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in updating rule quantity for rule {ruleId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public Response UpdateRuleTargetPrice(int clientId, int storeId, int ruleId, int targetPrice)
        {
            try
            {
                marketManagerFacade.UpdateRuleTargetPrice(clientId, storeId, ruleId, targetPrice);
                // logger.Info($"Update rule target price for rule {ruleId} in store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in updating rule target price for rule {ruleId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public Response UpdateCompositeOperator(int clientId, int storeId, int ruleId, int Operator)
        {
            try
            {
                marketManagerFacade.UpdateCompositeOperator(clientId, storeId, ruleId, Operator);
                // logger.Info($"Update composite operator for rule {ruleId} in store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in updating composite operator for rule {ruleId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public Response UpdateCompositeRules(int clientId, int storeId, int ruleId, List<int> rules)
        {
            try
            {
                marketManagerFacade.UpdateCompositeRules(clientId, storeId, ruleId, rules);
                // logger.Info($"Update composite rules for rule {ruleId} in store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in updating composite rules for rule {ruleId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public Response AddPurchasePolicy(int clientId, int storeId, DateTime expirationDate, string subject, int ruleId)
        {
            try
            {
                marketManagerFacade.AddPurchasePolicy(clientId, storeId, expirationDate, subject, ruleId);
                // logger.Info($"Add purchase policy for store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in adding purchase policy for store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public Response AddDiscountPolicy(int clientId, int storeId, DateTime expirationDate, string subject, int ruleId, double precentage)
        {
            try
            {
                marketManagerFacade.AddDiscountPolicy(clientId, storeId, expirationDate, subject, ruleId, precentage);
                // logger.Info($"Add discount policy for store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in adding discount policy for store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
        public Response AddCompositePolicy(int clientId, int storeId, DateTime expirationDate, string subject, int Operator, List<int> policies)
        {
            try
            {
                marketManagerFacade.AddCompositePolicy(clientId, storeId, expirationDate, subject, Operator, policies);
                // logger.Info($"Add composite policy for store {storeId} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                // logger.Error($"Error in adding composite policy for store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }


    }
}
