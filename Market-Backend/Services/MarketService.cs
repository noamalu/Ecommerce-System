using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market-Backend.Services
{
    public class MarketService : IMarketService
    {
        void IMarketService.addManger(int activeId, int storeId, int toAddId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.addOwner(int activeId, int storeId, int toAddId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.addPermission(int activeId, int storeId, int toAddId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.addProduct(int productId, string productName, int storeId, string category, double price, int quantity, double discount)
        {
            throw new NotImplementedException();
        }

        void IMarketService.closeStrore(int storeId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.editPurchasePolicy(int storeId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.getFounder()
        {
            throw new NotImplementedException();
        }

        void IMarketService.getMangers()
        {
            throw new NotImplementedException();
        }

        void IMarketService.getOwners()
        {
            throw new NotImplementedException();
        }

        void IMarketService.isAvailable(int productId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.openStore(int storeId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.removeManger(int activeId, int storeId, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.removeOwner(int activeId, int storeId, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.removePermission(int activeId, int storeId, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.removeProduct(int productId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.removeStaffMember(int activeId, int storeId, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.updateProduct(int productId)
        {
            throw new NotImplementedException();
        }

        void IMarketService.updateProductPrice(int productId, int price)
        {
            throw new NotImplementedException();
        }

        void IMarketService.updateProductQuantity(int productId, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
