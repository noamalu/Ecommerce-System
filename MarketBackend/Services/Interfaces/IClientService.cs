using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketBackend.Domain.Models;

namespace MarketBackend.Services.Interfaces
{
    public interface IClientService
    {
        public Response Register(int id, string username, string password, string email, int age);
        public Response EnterAsGuest(int id);
        public Response PurchaseCart(int id);
        public void CreateStore(int id);
        public void ResToStoreManageReq();
        public void ResToStoreOwnershipReq(); //respond to store ownership request
        public Response LogoutClient(int id);
        public Response RemoveFromCart(int clientId, int productId);
        public void ViewCart(int id);
        public Response AddToCart(int clientId, int productId);
        public Response LoginClient(int clientId, string username, string password);
        public void ExitGuest();
        public void BrowseGuest();
        public Response<Purchase> GetPurchaseHistory(int id);
    }
}
