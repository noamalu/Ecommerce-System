using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBackend.Services.Interfaces
{
    public interface IMarketService
    {
        public void AddProduct(int productId, string productName, int storeId, string category, double price, int quantity, double discount);
        public void RemoveProduct(int productId);
        public void UpdateProduct(int productId);
        public void RemoveStaffMember(int activeId, int storeId, int toRemoveId);
        public void AddManger(int activeId, int storeId, int toAddId);
        public void RemoveManger(int activeId, int storeId, int toRemoveId);
        public void AddOwner(int activeId, int storeId, int toAddId);
        public void RemoveOwner(int activeId, int storeId, int toRemoveId);
        public void GetOwners();
        public void GetMangers();
        public void GetFounder();
        public void UpdateProductQuantity(int productId, int quantity);
        public void UpdateProductPrice(int productId, int price);
        public void CloseStore(int storeId);
        public void OpenStore(int storeId);
        public void IsAvailable(int productId);
        public void RemovePermission(int activeId, int storeId, int toRemoveId);
        public void AddPermission(int activeId, int storeId, int toAddId);
        public void EditPurchasePolicy(int storeId);
    }
}
