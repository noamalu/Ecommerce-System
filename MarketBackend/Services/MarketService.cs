using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketBackend.Services.Interfaces;
using MarketBackend.Domain.Market_Client;

namespace MarketBackend.Services
{
    public class MarketService : IMarketService
    {
        private static MarketService _marketService = null;
        private MarketService(){
            
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
        
        public void AddManger(int activeId, int storeId, int toAddId)
        {
            throw new NotImplementedException();
        }

        public void AddOwner(int activeId, int storeId, int toAddId)
        {
            throw new NotImplementedException();
        }

        public void AddPermission(int activeId, int storeId, int toAddId)
        {
            throw new NotImplementedException();
        }

        public Response AddProduct(int productId, string productName, int storeId, string category, double price, int quantity, double discount)
        {
            throw new NotImplementedException();
        }

        public Response CloseStore(int storeId)
        {
            throw new NotImplementedException();
        }

        public void EditPurchasePolicy(int storeId)
        {
            throw new NotImplementedException();
        }

        public void GetFounder()
        {
            throw new NotImplementedException();
        }

        public void GetMangers()
        {
            throw new NotImplementedException();
        }

        public void GetOwners()
        {
            throw new NotImplementedException();
        }

        public void IsAvailable(int productId)
        {
            throw new NotImplementedException();
        }

        public Response OpenStore(int storeId)
        {
            throw new NotImplementedException();
        }

        public void RemoveManger(int activeId, int storeId, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        public void RemoveOwner(int activeId, int storeId, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        public void RemovePermission(int activeId, int storeId, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        public Response RemoveProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public Response RemoveStaffMember(int storeId, int activeId, Role role, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        public Response AddStaffMember(int storeId, int activeId, Role role, int toAddId)
        {
            throw new NotImplementedException();
        }

        public Response UpdateProductDiscount(int productId, double discount)
        {
            throw new NotImplementedException();
        }

        public Response UpdateProductPrice(int productId, double price)
        {
            throw new NotImplementedException();
        }

        public Response UpdateProductQuantity(int productId, int quantity)
        {
            throw new NotImplementedException();
        }

        public Response<Product> SearchByKeywords(string keywords)
        {
            throw new NotImplementedException();
        }

        public Response<Product> SearchByName(string name)
        {
            string lowerName = name.ToLower();
            throw new NotImplementedException();
        }

        public Response<Product> SearchByCategory(string category)
        {
            throw new NotImplementedException();
        }

        public Response<string> GetInfo(int storeId){
            throw new NotImplementedException();
        }
    }
}
