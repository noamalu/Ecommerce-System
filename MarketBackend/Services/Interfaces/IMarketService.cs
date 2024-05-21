using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Payment;
using MarketBackend.Domain.Shipping;

namespace MarketBackend.Services.Interfaces
{
    public interface IMarketService
    {
        public Response AddProduct(int storeId, int userId, string name, string sellMethod, string description, double price, string category, int quantity, bool ageLimit);
        public Response RemoveProduct(int storeId,int userId, int productId);
        public Response RemoveStaffMember(int storeId, int activeId, Role role, int toRemoveId);
        public Response AddStaffMember(int storeId, int activeId, Role role, int toAddId);
        public Response AddManger(int activeId, int storeId, int toAddId);
        public Response RemoveManger(int activeId, int storeId, int toRemoveId);
        public Response AddOwner(int activeId, int storeId, int toAddId);
        public Response RemoveOwner(int activeId, int storeId, int toRemoveId);
        public Response<List<Member>> GetOwners(int storeId);
        public Response<List<Member>> GetMangers(int storeId);
        public Response<Member> GetFounder(int storeId);
        public Response UpdateProductQuantity(int storeId, int userId, int productId, int quantity);
        public Response UpdateProductPrice(int storeId, int userId,  int productId, double price);
        public Response CloseStore(int clientId, int storeId);
        public Response OpenStore(int clientId, int storeId);
        public Response<bool> IsAvailable(int productId);
        public Response RemovePermission(int activeId, int storeId, int toRemoveId);
        public Response AddPermission(int activeId, int storeId, int toAddId, Permission permission);
        public Response EditPurchasePolicy(int storeId);
        public Response<HashSet<Product>> SearchByKeywords(string keywords);
        public Response<HashSet<Product>> SearchByName(string name);
        public Response<HashSet<Product>> SearchByCategory(string category);
        public Response<string> GetInfo(int storeId);
        public Response<string> GetProductInfo(int storeId, int productId);
        public Response PurchaseCart(int id, PaymentDetails paymentDetails, ShippingDetails shippingDetails);
        public Response<List<Purchase>> GetPurchaseHistory(int storeId, int clientId);

    }
}
