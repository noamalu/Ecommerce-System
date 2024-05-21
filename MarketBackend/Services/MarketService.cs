using MarketBackend.Services.Interfaces;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Payment;
using NLog;

namespace MarketBackend.Services
{
    public class MarketService : IMarketService
    {
        private static MarketService _marketService = null;
        private MarketManagerFacade marketManagerFacade;
        private Logger logger;
        private MarketService(){
            marketManagerFacade = MarketManagerFacade.GetInstance();
            logger = MyLogger.GetLogger();
        }

        public static MarketService GetInstance(){
            if (_marketService == null){
                _marketService = new MarketService();
            }
            return _marketService;
        }

        public void Dispose(){
            MarketManagerFacade.Dispose();
            _marketService = MarketService.GetInstance();
        }
        
        public Response AddManger(int activeId, int storeId, int toAddId)
        {
            try
            {
                marketManagerFacade.AddManger(activeId, storeId, toAddId);
                logger.Info($"client {activeId} added {toAddId} as manager.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in adding {toAddId} by {activeId} as a manager. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response AddOwner(int activeId, int storeId, int toAddId)
        {
            try
            {
                marketManagerFacade.AddOwner(activeId, storeId, toAddId);
                logger.Info($"client {activeId} added {toAddId} as owner.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in adding {toAddId} by {activeId} as a owner. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response AddPermission(int activeId, int storeId, int toAddId, Permission permission)
        {
            try
            {
                marketManagerFacade.AddPermission(activeId, storeId, toAddId, permission);
                logger.Info($"Client {activeId} added permission {permission} to client {toAddId} in store {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in adding permission {permission} by client {activeId} to client {toAddId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response AddProduct(int storeId, int userId, string name, string sellMethod, string description, double price, string category, int quantity, bool ageLimit)
        {
            try
            {
                marketManagerFacade.AddProduct(storeId, userId, name, sellMethod, description, price, category, quantity, ageLimit);
                logger.Info($"Client {userId} added product {name} store {storeId} with sellmethod {sellMethod}, description {description}, category {category}, price {price}, quantity {quantity}, ageLimit {ageLimit}.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in adding product {name} to store {storeId} by client {userId}, with  sellmethod {sellMethod}, description {description}, category {category}, price {price}, quantity {quantity}, ageLimit {ageLimit}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response CloseStore(int clientId, int storeId)
        {
            try
            {
                marketManagerFacade.CloseStore(clientId, storeId);
                logger.Info($"Client {clientId} closed store {storeId}");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in closing store {storeId} by client {clientId}. Error message: {e.Message}");
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

        public Response OpenStore(int clientId, int storeId)
        {
            try
            {
                marketManagerFacade.OpenStore(clientId, storeId);
                logger.Info($"client {clientId} opened store {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in open store {storeId} by client {clientId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response RemoveManger(int activeId, int storeId, int toRemoveId)
        {
            try
            {
                marketManagerFacade.RemoveManger(activeId, storeId, toRemoveId);
                logger.Info($"client {activeId} removed manager of client {toRemoveId} in store {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in removing manager of client {toRemoveId} by client {activeId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response RemoveOwner(int activeId, int storeId, int toRemoveId)
        {
            try
            {
                marketManagerFacade.RemoveOwner(activeId, storeId, toRemoveId);
                logger.Info($"client {activeId} removed owner of client {toRemoveId} in store {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in removing owner of client {toRemoveId} by client {activeId} in store {storeId}. Error message: {e.Message}");
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
                logger.Info($"Client {userId} removed product {productId} from store {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in removing product {productId} from store {storeId} by client {userId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response RemoveStaffMember(int storeId, int activeId, Role role, int toRemoveId)
        {
            try
            {
                marketManagerFacade.RemoveStaffMember(storeId, activeId, role, toRemoveId);
                logger.Info($"client {activeId} removed role {role} for client {toRemoveId} in store {storeId}");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in removing role {role} for client {toRemoveId} by client {activeId} in store {storeId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response AddStaffMember(int storeId, int activeId, Role role, int toAddId)
        {
            try
            {
                marketManagerFacade.AddStaffMember(storeId, activeId, role, toAddId);
                logger.Info($"client {activeId} added role {role} for client {toAddId} in store {storeId}");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in adding role {role} for client {toAddId} by client {activeId} in store {storeId}. Error message: {e.Message}");
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

        public Response UpdateProductPrice(int storeId, int userId, int productId, double price)
        {
            try
            {
                marketManagerFacade.UpdateProductPrice(storeId, userId, productId, price);
                logger.Info($"Product {productId} price was updated to {price}.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in updating product {productId} price to {price}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response UpdateProductQuantity(int storeId, int userId, int productId, int quantity)
        {
            try
            {
                marketManagerFacade.UpdateProductQuantity(storeId, userId,productId, quantity);
                logger.Info($"Product {productId} quantity was updated to {quantity}.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in updating product {productId} quantoty to {quantity}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response<HashSet<Product>> SearchByKeywords(string keywords)
        {
            try
            {
                HashSet<Product> products = marketManagerFacade.SearchByKeyWords(keywords);
                logger.Info($"Search by keyWords {keywords} succeed.");
                return Response<HashSet<Product>>.FromValue(products);
            }
            catch (Exception e)
            {
                logger.Error($"Error in search by keyword {keywords}. Error message: {e.Message}");
                return Response<HashSet<Product>>.FromError(e.Message);
            }
        }

        public Response<HashSet<Product>> SearchByName(string name)
        {
            string lowerName = name.ToLower();
            try
            {
                HashSet<Product> products = marketManagerFacade.SearchByName(name);
                logger.Info($"Search by name {name} succeed.");
                return Response<HashSet<Product>>.FromValue(products);
            }
            catch (Exception e)
            {
                logger.Error($"Error in search by name {name}. Error message: {e.Message}");
                return Response<HashSet<Product>>.FromError(e.Message);
            }
        }

        public Response<HashSet<Product>> SearchByCategory(string category)
        {
            try
            {
                HashSet<Product> products = marketManagerFacade.SearchByCategory(category);
                logger.Info($"Search by category {category} succeed.");
                return Response<HashSet<Product>>.FromValue(products);
            }
            catch (Exception e)
            {
                logger.Error($"Error in search by category {category}. Error message: {e.Message}");
                return Response<HashSet<Product>>.FromError(e.Message);
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

        public Response PurchaseCart(int id, PaymentDetails paymentDetails)
        {
             try
            {
                marketManagerFacade.PurchaseCart(id, paymentDetails);
                logger.Info($"Purchase cart for client {id} succeed.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in purchase cart for client {id}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }
    }
}
