using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Models;
using MarketBackend.Domain.Payment;
using MarketBackend.Services.Models;

namespace MarketBackend.Services.Interfaces
{
    public interface IClientService
    {
        public Task<Response> Register(string username, string password, string email, int age);
        public Task<Response<string>> EnterAsGuest(string identifier);
        public Task<Response<int>> CreateStore(string identifier, string storeName, string email, string phoneNum);
        public Task<Response<bool>> ResToStoreManageReq(string identifier);
        public Task<Response<bool>> ResToStoreOwnershipReq(string identifier); //respond to store ownership request
        public Task<Response> LogoutClient(string identifier);
        public Task<Response> RemoveFromCart(string identifier, int productId, int storeId, int quantity);
        public Task<Response<ShoppingCartResultDto>> ViewCart(string identifier);
        public Task<Response> AddToCart(string identifier, int storeId, int productId, int quantity);
        public Task<Response<string>> LoginClient(string username, string password);
        public Task<Response> ExitGuest(string identifier);
        public Task<Response<List<ShoppingCartResultDto>>> GetPurchaseHistoryByClient(string userName);
        public Task<Response> EditPurchasePolicy(int storeId);
        public Task<Response<List<StoreResultDto>>> GetMemberStores(string identifier);
        public Task<Response<StoreResultDto>> GetMemberStore(string identifier, int storeId);
        public Task<Response<List<MessageResultDto>>> GetMemberNotifications(string identifier);
        public Response SetMemberNotifications(string identifier, bool on);
        public Response<string> GetTokenByUserName(string userName);
    }
}
