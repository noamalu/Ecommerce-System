using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market-Backend.Services
{
    public interface IMarketService
    {
        void addProduct(int productId, string productName, int storeId, string category, double price, int quantity, double discount);
        void removeProduct(int productId);
        void updateProduct(int productId);
        void removeStaffMember(int activeId, int storeId, int toRemoveId);
        void addManger(int activeId, int storeId, int toAddId);
        void removeManger(int activeId, int storeId, int toRemoveId);
        void addOwner(int activeId, int storeId, int toAddId);
        void removeOwner(int activeId, int storeId, int toRemoveId);
        void getOwners();
        void getMangers();
        void getFounder();
        void updateProductQuantity(int productId, int quantity);
        void updateProductPrice(int productId, int price);
        void closeStrore(int storeId);
        void openStore(int storeId);
        void isAvailable(int productId);
        void removePermission(int activeId, int storeId, int toRemoveId);
        void addPermission(int activeId, int storeId, int toAddId);
        void editPurchasePolicy(int storeId);
    }
}
