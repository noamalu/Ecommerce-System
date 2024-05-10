using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBackend.Services
{
    public interface IClientService
    {
        void Register(string username, string password);
        void EnterAsGuest();
        void PurchaseCart(int id);
        void CreateStore(int id);
        void ResToStoreManageReq();
        void ResToStoreOwnershipReq(); //respond to store ownership request
        void LogoutClient(int id);
        void RemoveFromCart(int clientId, int productId);
        void ViewCart(int id);
        void AddToCart(int clientId, int productId);

        void LoginClient(string username, string password);
        void ExitGuest();
        void BrowseGuest();
        void GetPurchaseHistory(int id);
    }
}
