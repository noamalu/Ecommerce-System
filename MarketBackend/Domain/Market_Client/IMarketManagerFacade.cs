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
        void Register(int id, string username, string password, string email, int age);
        void EnterAsGuest(int id);
        void PurchaseCart(int id, PaymentDetails paymentDetails);
        void CreateStore(int id, string storeName, string email, string phoneNum);
        bool ResToStoreManageReq(int id);
        bool ResToStoreOwnershipReq(int id); //respond to store ownership request
        void LogoutClient(int id);
        void RemoveFromCart(int clientId, int productId, int basketId, int quantity);
        ShoppingCart ViewCart(int id);
        void AddToCart(int clientId, int storeId, int productId, int quantity);

        void LoginClient(int id, string username, string password);
        void ExitGuest(int id);
        // void UpdateProductDiscount(int productId, double discount);
        List<ShoppingCart> GetPurchaseHistoryByClient(int id);
        List<Purchase> GetPurchaseHistoryByStore(int storeId, int userId);
        void AddProduct(int storeId, int userId, string name, string sellMethod, string description, double price, string category, int quantity, bool ageLimit);
        void RemoveProduct(int storeId,int userId, int productId);
        void RemoveStaffMember(int storeId, int activeId, Role role, int toRemoveId);
        void AddManger(int activeId, int storeId, int toAddId);
        void RemoveManger(int activeId, int storeId, int toRemoveId);
        void AddOwner(int activeId, int storeId, int toAddId);
        void RemoveOwner(int activeId, int storeId, int toRemoveId);
        List<Member> GetOwners(int storeId);
        List<Member> GetMangers(int storeId);
        Member GetFounder(int storeId);

        void UpdateProductQuantity(int storeId, int userId, int productId, int quantity); 
        void UpdateProductPrice(int storeId, int userId,  int productId, double price);
        void CloseStore(int userId, int storeId);
        void OpenStore(int clientId, int storeId);
        bool IsAvailable(int storeId);
        void RemovePermission(int activeId, int storeId, int toRemoveId, Permission permission);
        void AddPermission(int activeId, int storeId, int toAddId, Permission permission);
        void EditPurchasePolicy(int storeId);
        HashSet<Product> SearchByKeyWords(string keywords);
        HashSet<Product> SearchByName(string name);
        HashSet<Product> SearchByCategory(string category);
        HashSet<Product> SearchByCategoryWithStore(int storeId, string category);
        HashSet<Product> SearchByKeyWordsWithStore(int storeId, string keywords);
        HashSet<Product> SearchByNameWithStore(int storeId, string name);
        void Filter (HashSet<Product> products, string category, double lowPrice, double highPrice, double lowProductRate, double highProductRate, double lowStoreRate, double highStoreRate);
        string GetProductInfo(int storId, int productId);
        public void AddStaffMember(int storeId, int activeId, Role role, int toAddId);   
        public string GetInfo(int storeId);     
    }
}
