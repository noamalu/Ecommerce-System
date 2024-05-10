using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBackend.Services
{
    public class ClientService : IClientService
    {
        void IClientService.addToCart(int clientId, int productId)
        {
            throw new NotImplementedException();
        }

        void IClientService.browseGuest()
        {
            throw new NotImplementedException();
        }

        void IClientService.createStore(int id)
        {
            throw new NotImplementedException();
        }

        void IClientService.enterAsGuest()
        {
            throw new NotImplementedException();
        }

        void IClientService.exitGuest()
        {
            throw new NotImplementedException();
        }

        void IClientService.getPurchaseHistory(int id)
        {
            throw new NotImplementedException();
        }

        void IClientService.loginClient(string username, string password)
        {
            throw new NotImplementedException();
        }

        void IClientService.logoutClient(int id)
        {
            throw new NotImplementedException();
        }

        void IClientService.purchaseCart(int id)
        {
            throw new NotImplementedException();
        }

        void IClientService.register(string username, string password)
        {
            throw new NotImplementedException();
        }

        void IClientService.removeFromCart(int clientId, int productId)
        {
            throw new NotImplementedException();
        }

        void IClientService.resToStoreManageReq()
        {
            throw new NotImplementedException();
        }

        void IClientService.resToStoreOwnershipReq()
        {
            throw new NotImplementedException();
        }

        void IClientService.viewCart(int id)
        {
            throw new NotImplementedException();
        }
    }
}
