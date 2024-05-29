﻿using System;
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
        public Response Register(int id, string username, string password, string email, int age);
        public Response EnterAsGuest(int id);
        public Response CreateStore(int id, string storeName, string email, string phoneNum);
        public Response<bool> ResToStoreManageReq(int id);
        public Response<bool> ResToStoreOwnershipReq(int id); //respond to store ownership request
        public Response LogoutClient(int id);
        public Response RemoveFromCart(int clientId, int productId, int basketId, int quantity);
        public Response<ShoppingCartResultDto> ViewCart(int id);
        public Response AddToCart(int clientId, int storeId, int productId, int quantity);
        public Response LoginClient(int clientId, string username, string password);
        public Response ExitGuest(int id);
        public Response<List<ShoppingCartResultDto>> GetPurchaseHistory(int id);
        public Response EditPurchasePolicy(int storeId);
    }
}
