using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Models;
using MarketBackend.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace MarketBackend.Services
{
    public class ClientService : IClientService
    {

        private static ClientService _clientService = null;
        private MarketManagerFacade marketManagerFacade;
        private ClientService(){
            //marketManagerFacade = MarketManagerFacade.GetInstance();
        }

        public static ClientService GetInstance(){
            if (_clientService == null){
                _clientService = new ClientService();
            }
            return _clientService;
        }

        public void Dispose(){
            _clientService = new ClientService();
        }
        public Response AddToCart(int clientId, int storeId, int productId, int quantity)
        {
            try
            {
                marketManagerFacade.AddToCart(clientId, storeId, productId, quantity);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response CreateStore(int id)
        {
            try
            {
                marketManagerFacade.CreateStore(id);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response EnterAsGuest(int id)
        {
             try
            {
                marketManagerFacade.EnterAsGuest();
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response ExitGuest()
        {
             try
            {
                marketManagerFacade.ExitGuest();
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response<List<Purchase>> GetPurchaseHistory(int id)
        {
             try
            {
                List<Purchase> purchases = marketManagerFacade.GetPurchaseHistoryByClient(id);
                //log
                return Response<List<Purchase>>.FromValue(purchases);
            }
            catch (Exception e)
            {
                //log
                return Response<List<Purchase>>.FromError(e.Message);
            }
        }

        public Response LoginClient(int userId, string username, string password)
        {
             try
            {
                marketManagerFacade.LoginClient(username, password);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response LogoutClient(int id)
        {
             try
            {
                marketManagerFacade.LogoutClient(id);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response PurchaseCart(int id)
        {
             try
            {
                marketManagerFacade.PurchaseCart(id);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response Register(int id, string username, string password, string email, int age)
        {
             try
            {
                marketManagerFacade.Register(username, password, email, age);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response RemoveFromCart(int clientId, int productId)
        {
            try
            {
                marketManagerFacade.RemoveFromCart(clientId, productId);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response<bool> ResToStoreManageReq(int id)
        {
            try
            {
                bool ans = marketManagerFacade.ResToStoreManageReq(id);
                //log
                return Response<bool>.FromValue(ans);
            }
            catch (Exception e)
            {
                //log
                return Response<bool>.FromError(e.Message);
            }
        }

        public Response<bool> ResToStoreOwnershipReq(int id)
        {
            try
            {
                bool ans = marketManagerFacade.ResToStoreOwnershipReq(id);
                //log
                return Response<bool>.FromValue(ans);
            }
            catch (Exception e)
            {
                //log
                return Response<bool>.FromError(e.Message);
            }
        }

        public Response<ShoppingCart> ViewCart(int id)
        {
            try
            {
                ShoppingCart cart = marketManagerFacade.ViewCart(id);
                //log
                return Response<ShoppingCart>.FromValue(cart);
            }
            catch (Exception e)
            {
                //log
                return Response<ShoppingCart>.FromError(e.Message);
            }
        }

        public Response EditPurchasePolicy(int storeId){
            try
            {
                marketManagerFacade.EditPurchasePolicy(storeId);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }
    }
}
