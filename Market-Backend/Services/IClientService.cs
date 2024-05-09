using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market-Backend.Services
{
    public interface IClientService
    {
        void register(string username, string password);
        void enterAsGuest();
        void purchaseCart(int id);
        void createStore(int id);
        void resToStoreManageReq();
        void resToStoreOwnershipReq();
        void logoutClient(int id);
        void removeFromCart(int clientId, int productId);
        void viewCart(int id);
        void addToCart(int clientId, int productId);

        void loginClient(string username, string password);
        void exitGuest();
        void browseGuest();
        void getPurchaseHistory(int id);
    }
}
