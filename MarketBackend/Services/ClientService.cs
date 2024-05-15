using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketBackend.Services.Interfaces;

namespace MarketBackend.Services
{
    public class ClientService : IClientService
    {

        private static ClientService _clientService = null;
        private ClientService(){
            
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
        public void AddToCart(int clientId, int productId)
        {
            throw new NotImplementedException();
        }

        public void BrowseGuest()
        {
            throw new NotImplementedException();
        }

        public void CreateStore(int id)
        {
            throw new NotImplementedException();
        }

        public Response EnterAsGuest(int id)
        {
            throw new NotImplementedException();
        }

        public void ExitGuest()
        {
            throw new NotImplementedException();
        }

        public void GetPurchaseHistory(int id)
        {
            throw new NotImplementedException();
        }

        public Response LoginClient(int userId, string username, string password)
        {
            throw new NotImplementedException();
        }

        public Response LogoutClient(int id)
        {
            throw new NotImplementedException();
        }

        public void PurchaseCart(int id)
        {
            throw new NotImplementedException();
        }

        public Response Register(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void RemoveFromCart(int clientId, int productId)
        {
            throw new NotImplementedException();
        }

        public void ResToStoreManageReq()
        {
            throw new NotImplementedException();
        }

        public void ResToStoreOwnershipReq()
        {
            throw new NotImplementedException();
        }

        public void ViewCart(int id)
        {
            throw new NotImplementedException();
        }
    }
}
