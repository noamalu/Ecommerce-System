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
        public bool LoggedIn {get; set;} 
        public ShoppingCart Cart {get; set;}
        public bool IsAbove18 {get; set;}

        public Client(int clientId){
            Id = clientId;
            Cart = new ShoppingCart(Id);
            IsAbove18 = false;
        }

        // public void Register(string userName, string password, string email){
        //     try{
        //         ValidateEmail(email);
        //         UserName = userName;
        //         // hash the password in Security
        //         Password = password;
        //     }
        //     catch (ArgumentException){
        //         throw;
        //     }
        // }

        // public void LogIn(string userName, string password){
        //     if (LoggedIn){
        //         throw new AggregateException("User is already logged in");
        //     }
        //     // hash the password
        //     if (UserName.Equals(userName) && Password.Equals(password)){
        //         LoggedIn = true;
        //     }
        // }

        // public void LogOut(){
        //     if (!LoggedIn){
        //         throw new AggregateException("User is already logged out");
        //     }
        //     LoggedIn = false;
        // }

        // public ? ViewCart(){

        // }

        public virtual void AddToCart(int basket, int productId, int quantity){
            Cart.addToCart(basket, productId, quantity);
            throw new NotImplementedException();
        }

        public virtual void RemoveFromCart(int basket, int productId, int quantity){
            Cart.removeFromCart(basket, productId, quantity);
        }

        public void PurchaseCart(){
            throw new NotImplementedException();
        }

        public virtual List<string> GetHistory(){
            throw new NotImplementedException();
        }

        public virtual bool ResToStoreManagerReq(){
            return true;
        }

        public virtual bool ResToStoreOwnershipReq(){
            return true;
        }

    }
}