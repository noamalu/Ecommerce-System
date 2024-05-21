using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Diagnostics.Contracts;
using MarketBackend.Domain.Models;

namespace MarketBackend.Domain.Market_Client
{
    public abstract class Client
    {
        public int Id  {get; set;}
        public ShoppingCart Cart {get; set;}
        public bool IsAbove18 {get; set;}

        public Client(int clientId){
            Id = clientId;
            Cart = new ShoppingCart(Id);
            IsAbove18 = false;
        }

        public virtual void AddToCart(int basket, int productId, int quantity){
            Cart.addToCart(basket, productId, quantity);
        }

        public virtual void RemoveFromCart(int basket, int productId, int quantity){
            Cart.removeFromCart(basket, productId, quantity);
        }

        public virtual bool ResToStoreManagerReq(){
            return true;
        }

        public virtual bool ResToStoreOwnershipReq(){
            return true;
        }

        public virtual void PurchaseBasket(int id, Basket basket)
        {            
            Cart.PurchaseBasket(id);
        }
    }
}