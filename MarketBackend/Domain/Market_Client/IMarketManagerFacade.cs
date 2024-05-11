using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Market_Client
{
    public interface IMarketManagerFacade
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
        void GetPurchaseHistoryByClient(int id);
        void GetPurchaseHistoryByStore(int id);
        void AddProduct(int productId, string productName, int storeId, string category, double price, int quantity, double discount);
        void RemoveProduct(int productId);
        void UpdateProduct(int productId);
        void RemoveStaffMember(int activeId, int storeId, int toRemoveId);
        void AddManger(int activeId, int storeId, int toAddId);
        void RemoveManger(int activeId, int storeId, int toRemoveId);
        void AddOwner(int activeId, int storeId, int toAddId);
        void RemoveOwner(int activeId, int storeId, int toRemoveId);
        void GetOwners();
        void GetMangers();
        void GetFounder();
        void UpdateProductQuantity(int productId, int quantity);
        void UpdateProductPrice(int productId, int price);
        void CloseStore(int storeId);
        void OpenStore(int storeId);
        void IsAvailable(int productId);
        void RemovePermission(int activeId, int storeId, int toRemoveId);
        void AddPermission(int activeId, int storeId, int toAddId);
        void EditPurchasePolicy(int storeId);
        List<Product> SearchByKeyWords();
        List<Product> SearchByName();
        List<Product> SearchByCategory();
        bool HasPermission();
        string GetProductInfo();        
    }
}
