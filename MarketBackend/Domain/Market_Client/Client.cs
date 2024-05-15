using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Diagnostics.Contracts;
using MarketBackend.Domain.Models;

namespace MarketBackend.Domain.Market_Client
{
    public class Client
    {
        private int _clientID;
        private string _userName;
        private string _password;
        private MailAddress _email;
        public bool loggedIn;
        private ShoppingCart shoppingCart;

        public Client(int clientId){
            _clientID = clientId;
        }

        public void Register(string userName, string password, string email){
            try{
                ValidateEmail(email);
                _userName = userName;
                // hash the password in Security
                _password = password;
            }
            catch (ArgumentException){
                throw;
            }
        }

        public void LogIn(string userName, string password){
            if (loggedIn){
                throw new AggregateException("User is already logged in");
            }
            // hash the password
            if (_userName.Equals(userName) && _password.Equals(password)){
                loggedIn = true;
            }
        }

        public void LogOut(){
            if (!loggedIn){
                throw new AggregateException("User is already logged out");
            }
            loggedIn = false;
        }

        // public ? ViewCart(){

        // }

        public void addToCart(int basket, int productId, int quantity){
            shoppingCart.addToCart(basket, productId, quantity);
            throw new NotImplementedException();
        }

        public void RemoveFromCart(int basket, int productId, int quantity){
            shoppingCart.removeFromCart(basket, productId, quantity);
        }

        public void PurchaseCart(){
            throw new NotImplementedException();
        }

        public List<string> GetHistory(){
            throw new NotImplementedException();
        }

        public bool ResToStoreManagerReq(){
            return true;
        }

        public bool ResToStoreOwnershipReq(){
            return true;
        }

        public void ValidateEmail(string email){
            try{
                MailAddress mailAddress = new MailAddress(email);
                _email = mailAddress;
            }
            catch (FormatException){
                throw new ArgumentException("Email address is not valid.");
            }
            
        }

    }
}