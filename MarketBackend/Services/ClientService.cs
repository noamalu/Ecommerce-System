using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Models;
using MarketBackend.Domain.Payment;
using MarketBackend.Services.Interfaces;
using Microsoft.Extensions.Logging;
using NLog;

namespace MarketBackend.Services
{
    public class ClientService : IClientService
    {

        private static ClientService _clientService = null;
        private MarketManagerFacade marketManagerFacade;
        private Logger logger;
        private ClientService(){
            marketManagerFacade = MarketManagerFacade.GetInstance();
            logger = MyLogger.GetLogger();
        }

        public static ClientService GetInstance(){
            if (_clientService == null){
                _clientService = new ClientService();
            }
            return _clientService;
        }

        public void Dispose(){
            MarketManagerFacade.Dispose();
            _clientService = new ClientService();
        }
        public Response AddToCart(int clientId, int storeId, int productId, int quantity)
        {
            try
            {
                marketManagerFacade.AddToCart(clientId, storeId, productId, quantity);
                logger.Info($"client {clientId} added product {productId} with quantity {quantity} to cart in store {storeId}");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in adding product of client {clientId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response CreateStore(int id, string storeName, string email, string phoneNum)
        {
            try
            {
                marketManagerFacade.CreateStore(id, storeName, email, phoneNum);
                logger.Info($"client {id} created store '{storeName}'");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in creating store of client {id}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response EnterAsGuest(int id)
        {
             try
            {
                marketManagerFacade.EnterAsGuest(id);
                logger.Info($"Client {id} entered as guest.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in entering as guest for client {id}, Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response ExitGuest(int id)
        {
             try
            {
                marketManagerFacade.ExitGuest(id);
                logger.Info($"Client exited.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in exiting, Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response<List<ShoppingCart>> GetPurchaseHistory(int id)
        {
             try
            {
                List<ShoppingCart> shoppingCarts = marketManagerFacade.GetPurchaseHistoryByClient(id);
                //log
                return Response<List<ShoppingCart>>.FromValue(shoppingCarts);
            }
            catch (Exception e)
            {
                logger.Error($"Error in getting purchase history for client {id}, Error message: {e.Message}");
                return Response<List<ShoppingCart>>.FromError(e.Message);
            }
        }

        public Response LoginClient(int userId, string username, string password)
        {
             try
            {
                marketManagerFacade.LoginClient(userId, username, password);
                logger.Info($"Client {userId} logged in with username {username}.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in log in for client {userId} with username {username}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response LogoutClient(int id)
        {
             try
            {
                marketManagerFacade.LogoutClient(id);
                logger.Info($"Client {id} logged out.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in logout for client {id}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response Register(int id, string username, string password, string email, int age)
        {
             try
            {
                marketManagerFacade.Register(id, username, password, email, age);
                logger.Info($"Client {id} registerd with username {username}.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in register for client {id} with username {username}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response RemoveFromCart(int clientId, int productId, int basketId, int quantity)
        {
            try
            {
                marketManagerFacade.RemoveFromCart(clientId, productId, basketId, quantity);
                logger.Info($"Client {clientId} removed {quantity} products {productId} from basket {basketId}.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in removing {quantity} products {productId} from basket {basketId} for client {clientId}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response<bool> ResToStoreManageReq(int id)
        {
            try
            {
                bool ans = marketManagerFacade.ResToStoreManageReq(id);
                logger.Info($"Client {id} responed to store manager request: {ans}");
                return Response<bool>.FromValue(ans);
            }
            catch (Exception e)
            {
                logger.Error($"Error in responding to store manager request for client {id}. Error message: {e.Message}");
                return Response<bool>.FromError(e.Message);
            }
        }

        public Response<bool> ResToStoreOwnershipReq(int id)
        {
            try
            {
                bool ans = marketManagerFacade.ResToStoreOwnershipReq(id);
                logger.Info($"Client {id} responed to store owner request: {ans}");
                return Response<bool>.FromValue(ans);
            }
            catch (Exception e)
            {
                logger.Error($"Error in responding to store owner request for client {id}. Error message: {e.Message}");
                return Response<bool>.FromError(e.Message);
            }
        }

        public Response<ShoppingCart> ViewCart(int id)
        {
            try
            {
                ShoppingCart cart = marketManagerFacade.ViewCart(id);
                logger.Info($"Client {id} view cart successfully.");
                return Response<ShoppingCart>.FromValue(cart);
            }
            catch (Exception e)
            {
                logger.Error($"Error in view cart for client {id}. Error message: {e.Message}");
                return Response<ShoppingCart>.FromError(e.Message);
            }
        }

        public Response EditPurchasePolicy(int storeId){
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

        public void InitiateSystemAdmin(){
            marketManagerFacade.InitiateSystemAdmin();
            logger.Info("initial");
        }
    }
}
