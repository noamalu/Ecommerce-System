using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.Domain.Models;
using MarketBackend.Services.Interfaces;

namespace MarketBackend.Domain.Market_Client
{
    public abstract class User
    {
        private int Id {get; set;}
        private ShoppingCart Cart {get; set;}

        public User(int id)
        {            
            Id = id;
            Cart = new ShoppingCart(Id);
        }

        public virtual Purchase PurchaseCart(int id)
        {
            Purchase pendingPurchase = Cart.PurchaseCart();
            return pendingPurchase;
        }
       // public virtual void LoginClient(string username, string password);
        // public virtual void LogoutClient(int id);
        // public virtual void SignUp(string userName, string password, string email);
        public virtual void RemoveFromCart(int basketId, int clientId, int productId)
        {
            Cart.removeFromCart(basketId, clientId, productId);
        }
        // public virtual void ViewCart(int id);
        public virtual void AddToCart(int clientId, int storeId, int productId, int quantity)
        {
            Cart.addToCart(storeId, productId, quantity);
        }
        public virtual void ResToStoreManageReq(){}
        public virtual void ResToStoreOwnershipReq(){} //respond to store ownership request
        public virtual void GetPurchaseHistoryByClient(int id){}
    }
}