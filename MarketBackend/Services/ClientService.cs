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
        private MarketManagerFacade _marketManagerFacade;
        private Logger _logger;
        public ClientService(IShippingSystemFacade shippingSystemFacade, IPaymentSystemFacade paymentSystem)
        {
            _marketManagerFacade = MarketManagerFacade.GetInstance(shippingSystemFacade, paymentSystem);
            _logger = MyLogger.GetLogger();
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
        public async Task<Response> AddToCart(string identifier, int storeId, int productId, int quantity)
        {
            try
            {
                await _marketManagerFacade.AddToCart(identifier, storeId, productId, quantity);
                _logger.Info($"client {identifier} added product {productId} with quantity {quantity} to cart in store {storeId}");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in adding product of client {identifier}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public async Task<Response<int>> CreateStore(string identifier, string storeName, string email, string phoneNum)
        {
            try
            {
                var storeId = await _marketManagerFacade.CreateStore(identifier, storeName, email, phoneNum);
                _logger.Info($"client {identifier} created store '{storeName}'");
                return Response<int>.FromValue(storeId);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in creating store of client {identifier}. Error message: {e.Message}");
                return Response<int>.FromError(e.Message);
            }
        }

        public async Task<Response<string>> EnterAsGuest(string identifier)
        {
             try
            {
                await _marketManagerFacade.EnterAsGuest(identifier);
                _logger.Info($"Client {identifier} entered as guest.");
                return Response<string>.FromValue(identifier);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in entering as guest for client {identifier}, Error message: {e.Message}");
                return Response<string>.FromError(e.Message);
            }
        }

        public async Task<Response> ExitGuest(string identifier)
        {
             try
            {
                await _marketManagerFacade.ExitGuest(identifier);
                _logger.Info($"Client exited.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in exiting, Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public async Task<Response<List<ShoppingCartResultDto>>> GetPurchaseHistoryByClient(string userName)
        {
             try
            {
                List<ShoppingCartHistory> shoppingCarts = await _marketManagerFacade.GetPurchaseHistoryByClient(userName);
                //log
                return Response<List<ShoppingCartResultDto>>.FromValue(shoppingCarts.Select(cart => new ShoppingCartResultDto(cart)).ToList());
            }
            catch (Exception e)
            {
                _logger.Error($"Error in getting purchase history for client {userName}, Error message: {e.Message}");
                return Response<List<ShoppingCartResultDto>>.FromError(e.Message);
            }
        }

        public async Task<Response<string>> LoginClient(string username, string password)
        {
             try
            {
                string token = await _marketManagerFacade.LoginClient(username, password);
                _logger.Info($"Client {username} logged in with username {username}.");
                return Response<string>.FromValue(token);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in log in for client {username} with username {username}. Error message: {e.Message}");
                return Response<string>.FromError(e.Message);
            }
        }

        public async Task<Response> LogoutClient(string identifier)
        {
             try
            {
                await _marketManagerFacade.LogoutClient(identifier);
                _logger.Info($"Client {identifier} logged out.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in logout for client {identifier}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public async Task<Response> Register(string username, string password, string email, int age)
        {
             try
            {
                await _marketManagerFacade.Register(username, password, email, age);
                _logger.Info($"Client {username} registerd with username {username}.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in register for client with username {username}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public async Task<Response> RemoveFromCart(string identifier, int productId, int storeId, int quantity)
        {
            try
            {
                await _marketManagerFacade.RemoveFromCart(identifier, productId, storeId, quantity);
                _logger.Info($"Client {identifier} removed {quantity} products {productId} from basket {storeId}.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in removing {quantity} products {productId} from basket {storeId} for client {identifier}. Error message: {e.Message}");
                return new Response(e.Message);
            }
        }

        public async Task<Response<bool>> ResToStoreManageReq(string identifier)
        {
            try
            {
                bool ans = await _marketManagerFacade.ResToStoreManageReq(identifier);
                _logger.Info($"Client {identifier} responed to store manager request: {ans}");
                return Response<bool>.FromValue(ans);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in responding to store manager request for client {identifier}. Error message: {e.Message}");
                return Response<bool>.FromError(e.Message);
            }
        }

        public async Task<Response<bool>> ResToStoreOwnershipReq(string identifier)
        {
            try
            {
                bool ans = await _marketManagerFacade.ResToStoreOwnershipReq(identifier);
                _logger.Info($"Client {identifier} responed to store owner request: {ans}");
                return Response<bool>.FromValue(ans);
            }
            catch (Exception e)
            {
                _logger.Error($"Error in responding to store owner request for client {identifier}. Error message: {e.Message}");
                return Response<bool>.FromError(e.Message);
            }
        }

        public async Task<Response<ShoppingCartResultDto>> ViewCart(string identifier)
        {
            try
            {
                ShoppingCart cart = await _marketManagerFacade.ViewCart(identifier);
                _logger.Info($"Client {identifier} view cart successfully.");
                return Response<ShoppingCartResultDto>.FromValue(await ShoppingCartResultDto.CreateAsync(cart));
            }
            catch (Exception e)
            {
                _logger.Error($"Error in view cart for client {identifier}. Error message: {e.Message}");
                return Response<ShoppingCartResultDto>.FromError(e.Message);
            }
        }

        public async Task<Response> EditPurchasePolicy(int storeId){
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

        public async Task InitiateSystemAdmin(){
            await _marketManagerFacade.InitiateSystemAdmin();
            _logger.Info("initial");
        }

        public async Task<int> GetMemberIDByUserName(string username)
        {
            return await _marketManagerFacade.GetMemberIDrByUserName(username);
        }

        public async Task<Member> GetMember(string username)
        {
            return await _marketManagerFacade.GetMember(username);
        }

        public async Task<Response<List<StoreResultDto>>> GetMemberStores(string identifier)
        {
            try
            {
                var stores = await _marketManagerFacade.GetMemberStores(identifier);
                _logger.Info($"fetched client {identifier} stores successfuly.");
                return Response<List<StoreResultDto>>.FromValue(stores.Select(store => new StoreResultDto(store)).ToList());
            }
            catch (Exception e)
            {
                _logger.Error($"Error in fetching client {identifier} stores. Error message: {e.Message}");
                return Response<List<StoreResultDto>>.FromError(e.Message);
            }
        }

        public async Task<Response<StoreResultDto>> GetMemberStore(string identifier, int storeId)
        {
            try
            {
                var store = await _marketManagerFacade.GetMemberStore(identifier, storeId);
                _logger.Info($"fetched client {identifier} stores successfuly.");
                return Response<StoreResultDto>.FromValue(new(store));
            }
            catch (Exception e)
            {
                _logger.Error($"Error in fetching client {identifier} stores. Error message: {e.Message}");
                return Response<StoreResultDto>.FromError(e.Message);
            }        
        }

        public async Task<Response<List<MessageResultDto>>> GetMemberNotifications(string identifier)
        {
           try
            {
                var notifications = await _marketManagerFacade.GetMemberNotifications(identifier);
                _logger.Info($"fetched client {identifier} notifications successfuly.");
                return Response<List<MessageResultDto>>.FromValue(notifications.Select(notification => new MessageResultDto(notification)).ToList());
            }
            catch (Exception e)
            {
                _logger.Error($"Error in fetching client {identifier} notifications. Error message: {e.Message}");
                return Response<List<MessageResultDto>>.FromError(e.Message);
            }    
        }

        public Response SetMemberNotifications(string identifier, bool on)
        {
            try
            {
                _marketManagerFacade.SetMemberNotifications(identifier, on);
                _logger.Info($"set client {identifier} notifications successfuly.");
                return new Response();
            }
            catch (Exception e)
            {
                _logger.Error($"Error in setting client {identifier} notifications. Error message: {e.Message}");
                return new Response(e.Message);
            } 
        }

        public Response<string> GetTokenByUserName(string userName)
        {
            try
            {
                var username = _marketManagerFacade.GetTokenByUserName(userName);
                return Response<string>.FromValue(username);
            }
            catch (Exception e)
            {
                return Response<string>.FromError(e.Message);
            }   
        }
    }


}
