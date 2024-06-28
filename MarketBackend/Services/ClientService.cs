using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Models;
using MarketBackend.Domain.Payment;
using MarketBackend.Domain.Shipping;
using MarketBackend.Services.Interfaces;
using MarketBackend.Services.Models;
using Microsoft.Extensions.Logging;
using NLog;

namespace MarketBackend.Services
{
    public class ClientService : IClientService
    {

        private static ClientService _clientService = null;
        private MarketManagerFacade marketManagerFacade;
        private Logger logger;
        private ClientService(IShippingSystemFacade shippingSystemFacade, IPaymentSystemFacade paymentSystem){
            marketManagerFacade = MarketManagerFacade.GetInstance(shippingSystemFacade, paymentSystem);
            logger = MyLogger.GetLogger();
        }

        public static ClientService GetInstance(IShippingSystemFacade shippingSystemFacade, IPaymentSystemFacade paymentSystem){
            if (_clientService == null){
                _clientService = new ClientService(shippingSystemFacade, paymentSystem);
            }
            return _clientService;
        }

        public void Dispose(){
            MarketManagerFacade.Dispose();
            _clientService = null;
        }
        public Response AddToCart(string identifier, int storeId, int productId, int quantity)
        {
            try
            {
                marketManagerFacade.AddToCart(identifier, storeId, productId, quantity);
                logger.Info($"client {identifier} added product {productId} with quantity {quantity} to cart in store {storeId}");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in adding product of client {identifier}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response<int> CreateStore(string identifier, string storeName, string email, string phoneNum)
        {
            try
            {
                var storeId = marketManagerFacade.CreateStore(identifier, storeName, email, phoneNum);
                logger.Info($"client {identifier} created store '{storeName}'");
                return Response<int>.FromValue(storeId);
            }
            catch (Exception e)
            {
                logger.Error($"Error in creating store of client {identifier}. Error message: {e.Message}");
                return Response<int>.FromError(e.Message);
            }
        }

        public Response<string> EnterAsGuest(string identifier)
        {
             try
            {
                marketManagerFacade.EnterAsGuest(identifier);
                logger.Info($"Client {identifier} entered as guest.");
                return Response<string>.FromValue(identifier);
            }
            catch (Exception e)
            {
                logger.Error($"Error in entering as guest for client {identifier}, Error message: {e.Message}");
                return Response<string>.FromError(e.Message);
            }
        }

        public Response ExitGuest(string identifier)
        {
             try
            {
                marketManagerFacade.ExitGuest(identifier);
                logger.Info($"Client exited.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in exiting, Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response<List<ShoppingCartResultDto>> GetPurchaseHistoryByClient(string userName)
        {
             try
            {
                List<ShoppingCartHistory> shoppingCarts = marketManagerFacade.GetPurchaseHistoryByClient(userName);
                //log
                return Response<List<ShoppingCartResultDto>>.FromValue(shoppingCarts.Select(cart => new ShoppingCartResultDto(cart)).ToList());
            }
            catch (Exception e)
            {
                logger.Error($"Error in getting purchase history for client {userName}, Error message: {e.Message}");
                return Response<List<ShoppingCartResultDto>>.FromError(e.Message);
            }
        }

        public Response<string> LoginClient(string username, string password)
        {
             try
            {
                string token = marketManagerFacade.LoginClient(username, password);
                logger.Info($"Client {username} logged in with username {username}.");
                return Response<string>.FromValue(token);
            }
            catch (Exception e)
            {
                logger.Error($"Error in log in for client {username} with username {username}. Error message: {e.Message}");
                return Response<string>.FromError(e.Message);
            }
        }

        public Response LogoutClient(string identifier)
        {
             try
            {
                marketManagerFacade.LogoutClient(identifier);
                logger.Info($"Client {identifier} logged out.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in logout for client {identifier}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response Register(string username, string password, string email, int age)
        {
             try
            {
                marketManagerFacade.Register(username, password, email, age);
                logger.Info($"Client {username} registerd with username {username}.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in register for client with username {username}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response RemoveFromCart(string identifier, int productId, int storeId, int quantity)
        {
            try
            {
                marketManagerFacade.RemoveFromCart(identifier, productId, storeId, quantity);
                logger.Info($"Client {identifier} removed {quantity} products {productId} from basket {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in removing {quantity} products {productId} from basket {storeId} for client {identifier}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public Response<bool> ResToStoreManageReq(string identifier)
        {
            try
            {
                bool ans = marketManagerFacade.ResToStoreManageReq(identifier);
                logger.Info($"Client {identifier} responed to store manager request: {ans}");
                return Response<bool>.FromValue(ans);
            }
            catch (Exception e)
            {
                logger.Error($"Error in responding to store manager request for client {identifier}. Error message: {e.Message}");
                return Response<bool>.FromError(e.Message);
            }
        }

        public Response<bool> ResToStoreOwnershipReq(string identifier)
        {
            try
            {
                bool ans = marketManagerFacade.ResToStoreOwnershipReq(identifier);
                logger.Info($"Client {identifier} responed to store owner request: {ans}");
                return Response<bool>.FromValue(ans);
            }
            catch (Exception e)
            {
                logger.Error($"Error in responding to store owner request for client {identifier}. Error message: {e.Message}");
                return Response<bool>.FromError(e.Message);
            }
        }

        public Response<ShoppingCartResultDto> ViewCart(string identifier)
        {
            try
            {
                ShoppingCart cart = marketManagerFacade.ViewCart(identifier);
                logger.Info($"Client {identifier} view cart successfully.");
                return Response<ShoppingCartResultDto>.FromValue(new(cart));
            }
            catch (Exception e)
            {
                logger.Error($"Error in view cart for client {identifier}. Error message: {e.Message}");
                return Response<ShoppingCartResultDto>.FromError(e.Message);
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

        public int GetMemberIDByUserName(string username)
        {
            return marketManagerFacade.GetMemberIDrByUserName(username);
        }

        public Member GetMember(string username)
        {
            return marketManagerFacade.GetMember(username);
        }

        public Response<List<StoreResultDto>> GetMemberStores(string identifier)
        {
            try
            {
                var stores = marketManagerFacade.GetMemberStores(identifier);
                logger.Info($"fetched client {identifier} stores successfuly.");
                return Response<List<StoreResultDto>>.FromValue(stores.Select(store => new StoreResultDto(store)).ToList());
            }
            catch (Exception e)
            {
                logger.Error($"Error in fetching client {identifier} stores. Error message: {e.Message}");
                return Response<List<StoreResultDto>>.FromError(e.Message);
            }
        }

        public Response<StoreResultDto> GetMemberStore(string identifier, int storeId)
        {
            try
            {
                var store = marketManagerFacade.GetMemberStore(identifier, storeId);
                logger.Info($"fetched client {identifier} stores successfuly.");
                return Response<StoreResultDto>.FromValue(new(store));
            }
            catch (Exception e)
            {
                logger.Error($"Error in fetching client {identifier} stores. Error message: {e.Message}");
                return Response<StoreResultDto>.FromError(e.Message);
            }        
        }

        public Response<List<MessageResultDto>> GetMemberNotifications(string identifier)
        {
           try
            {
                var notifications = marketManagerFacade.GetMemberNotifications(identifier);
                logger.Info($"fetched client {identifier} notifications successfuly.");
                return Response<List<MessageResultDto>>.FromValue(notifications.Select(notification => new MessageResultDto(notification)).ToList());
            }
            catch (Exception e)
            {
                logger.Error($"Error in fetching client {identifier} notifications. Error message: {e.Message}");
                return Response<List<MessageResultDto>>.FromError(e.Message);
            }    
        }

        public Response SetMemberNotifications(string identifier, bool on)
        {
            try
            {
                marketManagerFacade.SetMemberNotifications(identifier, on);
                logger.Info($"set client {identifier} notifications successfuly.");
                return new Response();
            }
            catch (Exception e)
            {
                logger.Error($"Error in setting client {identifier} notifications. Error message: {e.Message}");
                return new Response(e.Message);
            } 
        }
    }


}
