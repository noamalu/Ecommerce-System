using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketBackend.Services.Interfaces;

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

        public void AddProduct(int productId, string productName, int storeId, string category, double price, int quantity, double discount)
        {
            throw new NotImplementedException();
        }

        public void CloseStore(int storeId)
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

        public void OpenStore(int storeId)
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

        public void RemoveProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public void RemoveStaffMember(int activeId, int storeId, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        public void UpdateProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public void UpdateProductPrice(int productId, int price)
        {
            throw new NotImplementedException();
        }

        public void UpdateProductQuantity(int productId, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
