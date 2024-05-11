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
        void IMarketService.AddManger(int activeId, int storeId, int toAddId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.AddOwner(int activeId, int storeId, int toAddId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.AddPermission(int activeId, int storeId, int toAddId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.AddProduct(int productId, string productName, int storeId, string category, double price, int quantity, double discount)
        {
            throw new NotImplementedException();
        }

        void IMarketService.CloseStore(int storeId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.EditPurchasePolicy(int storeId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.GetFounder()
        {
            throw new NotImplementedException();
        }

        void IMarketService.GetMangers()
        {
            throw new NotImplementedException();
        }

        void IMarketService.GetOwners()
        {
            throw new NotImplementedException();
        }

        void IMarketService.IsAvailable(int productId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.OpenStore(int storeId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.RemoveManger(int activeId, int storeId, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.RemoveOwner(int activeId, int storeId, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.RemovePermission(int activeId, int storeId, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.RemoveProduct(int productId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.RemoveStaffMember(int activeId, int storeId, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.UpdateProduct(int productId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.UpdateProductPrice(int productId, int price)
        {
            throw new NotImplementedException();
        }

        void IMarketService.UpdateProductQuantity(int productId, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
