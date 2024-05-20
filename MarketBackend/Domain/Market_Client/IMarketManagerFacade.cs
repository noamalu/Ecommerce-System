using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MarketBackend.Domain.Models;
using MarketBackend.Domain.Payment;

namespace MarketBackend.Domain.Market_Client
{
    public interface IMarketManagerFacade
    {
        void Register(string username, string password, string email, int age);
        void EnterAsGuest();
        void PurchaseCart(int id, PaymentDetails paymentDetails);
        void CreateStore(int id, string storeName, string email, string phoneNum);
        bool ResToStoreManageReq(int id);
        bool ResToStoreOwnershipReq(int id); //respond to store ownership request
        void LogoutClient(int id);
        void RemoveFromCart(int clientId, int productId, int basketId, int quantity);
        ShoppingCart ViewCart(int id);
        void AddToCart(int clientId, int storeId, int productId, int quantity);

        void LoginClient(string username, string password);
        void ExitGuest();
        void BrowseGuest();
        List<Purchase> GetPurchaseHistoryByClient(int id);
        List<Purchase> GetPurchaseHistoryByStore(int id);
        void AddProduct(int productId, string productName, int storeId, string category, double price, int quantity, double discount);
        void RemoveProduct(int productId);
        void UpdateProductDiscount(int productId, double discount);
        void RemoveStaffMember(int storeId, int activeId, Role role, int toRemoveId);
        void AddManger(int activeId, int storeId, int toAddId);
        void RemoveManger(int activeId, int storeId, int toRemoveId);
        void AddOwner(int activeId, int storeId, int toAddId);
        void RemoveOwner(int activeId, int storeId, int toRemoveId);
        List<Member> GetOwners(int storeId);
        List<Member> GetMangers(int storeId);
        Member GetFounder(int storeId);
        void UpdateProductQuantity(int productId, int quantity);
        void UpdateProductPrice(int productId, double price);
        void CloseStore(int userId, int storeId);
        void OpenStore(int clientId, int storeId);
        bool IsAvailable(int storeId);
        void RemovePermission(int activeId, int storeId, int toRemoveId);
        void AddPermission(int activeId, int storeId, int toAddId);
        void EditPurchasePolicy(int storeId);
        List<Product> SearchByKeyWords(string keywords);
        List<Product> SearchByName(string name);
        List<Product> SearchByCategory(string category);
        bool HasPermission();
        string GetProductInfo(int productId);
        public void AddStaffMember(int storeId, int activeId, Role role, int toAddId);   
        public string GetInfo(int storeId);     
    }
}
