using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBackend.Services
{
    public class ClientService : IClientService
    {
        void IClientService.AddToCart(int clientId, int productId)
        {
            throw new NotImplementedException();
        }

        void IClientService.BrowseGuest()
        {
            throw new NotImplementedException();
        }

        void IClientService.CreateStore(int id)
        {
            throw new NotImplementedException();
        }

        void IClientService.EnterAsGuest()
        {
            throw new NotImplementedException();
        }

        void IClientService.ExitGuest()
        {
            throw new NotImplementedException();
        }

        void IClientService.GetPurchaseHistory(int id)
        {
            throw new NotImplementedException();
        }

        void IClientService.LoginClient(string username, string password)
        {
            throw new NotImplementedException();
        }

        void IClientService.LogoutClient(int id)
        {
            throw new NotImplementedException();
        }

        void IClientService.PurchaseCart(int id)
        {
            throw new NotImplementedException();
        }

        void IClientService.Register(string username, string password)
        {
            throw new NotImplementedException();
        }

        void IClientService.RemoveFromCart(int clientId, int productId)
        {
            throw new NotImplementedException();
        }

        void IClientService.ResToStoreManageReq()
        {
            throw new NotImplementedException();
        }

        void IClientService.ResToStoreOwnershipReq()
        {
            throw new NotImplementedException();
        }

        void IClientService.ViewCart(int id)
        {
            throw new NotImplementedException();
        }
    }
}
