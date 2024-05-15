using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketBackend.Domain.Market_Client;

namespace MarketBackend.Services.Interfaces
{
    public interface IMarketService
    {
        public Response AddProduct(int productId, string productName, int storeId, string category, double price, int quantity, double discount);
        public Response RemoveProduct(int productId);
        public Response UpdateProductDiscount(int productId, double discount);
        public Response RemoveStaffMember(int storeId, int activeId, Role role, int toRemoveId);
        public Response AddStaffMember(int storeId, int activeId, Role role, int toAddId);
        public void AddManger(int activeId, int storeId, int toAddId);
        public void RemoveManger(int activeId, int storeId, int toRemoveId);
        public void AddOwner(int activeId, int storeId, int toAddId);
        public void RemoveOwner(int activeId, int storeId, int toRemoveId);
        public void GetOwners();
        public void GetMangers();
        public void GetFounder();
        public Response UpdateProductQuantity(int productId, int quantity);
        public Response UpdateProductPrice(int productId, double price);
        public Response CloseStore(int storeId);
        public Response OpenStore(int storeId);
        public void IsAvailable(int productId);
        public void RemovePermission(int activeId, int storeId, int toRemoveId);
        public void AddPermission(int activeId, int storeId, int toAddId);
        public void EditPurchasePolicy(int storeId);
        public Response<Product> SearchByKeywords(string keywords);
        public Response<Product> SearchByName(string name);
        public Response<Product> SearchByCategory(string category);
        public Response<string> GetInfo(int storeId);

    }
}
