using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBackend.Services.Interfaces
{
    public interface IClientService
    {
        public void Register(string username, string password);
        public void EnterAsGuest();
        public void PurchaseCart(int id);
        public void CreateStore(int id);
        public void ResToStoreManageReq();
        public void ResToStoreOwnershipReq(); //respond to store ownership request
        public void LogoutClient(int id);
        public void RemoveFromCart(int clientId, int productId);
        public void ViewCart(int id);
        public void AddToCart(int clientId, int productId);
        public Response LoginClient(string username, string password);
        public void ExitGuest();
        public void BrowseGuest();
        public void GetPurchaseHistory(int id);
    }
}
