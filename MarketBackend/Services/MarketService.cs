using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketBackend.Services.Interfaces;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Payment;

namespace MarketBackend.Services
{
    public class MarketService : IMarketService
    {
        private static MarketService _marketService = null;
        private MarketManagerFacade marketManagerFacade;
        private MarketService(){
            marketManagerFacade = MarketManagerFacade.GetInstance();
        }

        public static MarketService GetInstance(){
            if (_marketService == null){
                _marketService = new MarketService();
            }
            return _marketService;
        }

        public void Dispose(){
            _marketService = new MarketService();
        }
        
        public Response AddManger(int activeId, int storeId, int toAddId)
        {
            try
            {
                marketManagerFacade.AddManger(activeId, storeId, toAddId);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response AddOwner(int activeId, int storeId, int toAddId)
        {
            try
            {
                marketManagerFacade.AddOwner(activeId, storeId, toAddId);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response AddPermission(int activeId, int storeId, int toAddId)
        {
            try
            {
                marketManagerFacade.AddPermission(activeId, storeId, toAddId);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response AddProduct(int productId, string productName, int storeId, string category, double price, int quantity, double discount)
        {
            try
            {
                marketManagerFacade.AddProduct(productId, productName, storeId, category, price, quantity, discount);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response CloseStore(int storeId)
        {
            try
            {
                marketManagerFacade.CloseStore(storeId);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response EditPurchasePolicy(int storeId)
        {
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

        public Response<Member> GetFounder(int storeId)
        {
            try
            {
                Member founder = marketManagerFacade.GetFounder(storeId);
                //log
                return Response<Member>.FromValue(founder);
            }
            catch (Exception e)
            {
                //log
                return Response<Member>.FromError(e.Message);
            }
        }

        public Response<List<Member>> GetMangers(int storeId)
        {
            try
            {
                List<Member> managers = marketManagerFacade.GetMangers(storeId);
                //log
                return Response<List<Member>>.FromValue(managers);
            }
            catch (Exception e)
            {
                //log
                return Response<List<Member>>.FromError(e.Message);
            }
        }

        public Response<List<Member>> GetOwners(int storeId)
        {
            try
            {
                List<Member> owners = marketManagerFacade.GetOwners(storeId);
                //log
                return Response<List<Member>>.FromValue(owners);
            }
            catch (Exception e)
            {
                //log
                return Response<List<Member>>.FromError(e.Message);
            }
        }

        public Response<bool> IsAvailable(int productId)
        {
            try
            {
                bool ans = marketManagerFacade.IsAvailable(productId);
                //log
                return Response<bool>.FromValue(ans);
            }
            catch (Exception e)
            {
                //log
                return Response<bool>.FromError(e.Message);
            }
        }

        public Response OpenStore(int storeId)
        {
            try
            {
                marketManagerFacade.OpenStore(storeId);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response RemoveManger(int activeId, int storeId, int toRemoveId)
        {
            try
            {
                marketManagerFacade.RemoveManger(activeId, storeId, toRemoveId);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response RemoveOwner(int activeId, int storeId, int toRemoveId)
        {
            try
            {
                marketManagerFacade.RemoveOwner(activeId, storeId, toRemoveId);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
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

        public Response RemoveProduct(int productId)
        {
            try
            {
                marketManagerFacade.RemoveProduct(productId);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response RemoveStaffMember(int storeId, int activeId, Role role, int toRemoveId)
        {
            try
            {
                marketManagerFacade.RemoveStaffMember(storeId, activeId, role, toRemoveId);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response AddStaffMember(int storeId, int activeId, Role role, int toAddId)
        {
            try
            {
                marketManagerFacade.AddStaffMember(storeId, activeId, role, toAddId);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response UpdateProductDiscount(int productId, double discount)
        {
            throw new NotImplementedException();
            // try
            // {
            //     marketManagerFacade.UpdateProductDiscount(storeId, userId, productId, discount);
            //     //log
            //     return new Response();
            // }
            // catch (Exception e)
            // {
            //     //log
            //     return new Response(e.Message);
            // }
        }

        public Response UpdateProductPrice(int storeId, int userId, int productId, double price)
        {
            try
            {
                marketManagerFacade.UpdateProductPrice(storeId, userId, productId, price);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response UpdateProductQuantity(int storeId, int userId, int productId, int quantity)
        {
            try
            {
                marketManagerFacade.UpdateProductQuantity(storeId, userId,productId, quantity);
                //log
                return new Response();
            }
            catch (Exception e)
            {
                //log
                return new Response(e.Message);
            }
        }

        public Response<List<Product>> SearchByKeywords(string keywords)
        {
            try
            {
                List<Product> products = marketManagerFacade.SearchByKeyWords(keywords);
                //log
                return Response<List<Product>>.FromValue(products);
            }
            catch (Exception e)
            {
                //log
                return Response<List<Product>>.FromError(e.Message);
            }
        }

        public Response<List<Product>> SearchByName(string name)
        {
            string lowerName = name.ToLower();
            try
            {
                List<Product> products = marketManagerFacade.SearchByName(name);
                //log
                return Response<List<Product>>.FromValue(products);
            }
            catch (Exception e)
            {
                //log
                return Response<List<Product>>.FromError(e.Message);
            }
        }

        public Response<List<Product>> SearchByCategory(string category)
        {
            try
            {
                List<Product> products = marketManagerFacade.SearchByCategory(category);
                //log
                return Response<List<Product>>.FromValue(products);
            }
            catch (Exception e)
            {
                //log
                return Response<List<Product>>.FromError(e.Message);
            }
        }

        public Response<string> GetInfo(int storeId){
            try
            {
                string info = marketManagerFacade.GetInfo(storeId);
                //log
                return Response<string>.FromValue(info);
            }
            catch (Exception e)
            {
                //log
                return Response<string>.FromError(e.Message);
            }
        }

        public Response<string> GetProductInfo(int productId){
            try
            {
                string info = marketManagerFacade.GetProductInfo(productId);
                //log
                return Response<string>.FromValue(info);
            }
            catch (Exception e)
            {
                //log
                return Response<string>.FromError(e.Message);
            }
        }

        public Response PurchaseCart(int id, PaymentDetails paymentDetails)
        {
             try
            {
                marketManagerFacade.PurchaseCart(id, paymentDetails);
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
