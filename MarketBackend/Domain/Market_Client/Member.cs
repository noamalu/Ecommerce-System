using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using MarketBackend.Domain.Models;

namespace MarketBackend.Domain.Market_Client
{
    public class Member : Client
    {
        public string UserName {get; set;}
        public string Password {get; set;}
        public MailAddress Email {get; set;}
        public ConcurrentDictionary<int, Role> Roles {get; set;}
        public ConcurrentDictionary<int,ShoppingCartHistory> OrderHistory {get; set;}
        public bool IsSystemAdmin {get; set;}
        public bool IsLoggedIn {get; set;}
        public bool IsNotification {get; set;}
        object _lock = new Object();

        public SynchronizedCollection<Message> alerts;
        public NotificationManager _alertManager = NotificationManager.GetInstance();

        public Member(int id, string userName, MailAddress mailAddress, string password) : base(id)
        {
            UserName = userName;
            Password = password;
            Email = mailAddress;
            Roles = new(); 
            OrderHistory = new(); 
            IsSystemAdmin = false;
            IsLoggedIn = false;
            IsNotification = true;
            alerts = new SynchronizedCollection<Message>();
        }

        public override void PurchaseBasket(Basket basket)
        {
            if(!OrderHistory.TryGetValue(basket._cartId, out var cartInHistory)){
                cartInHistory ??= new(){_shoppingCartId = basket._cartId};
                OrderHistory.TryAdd(basket._cartId, cartInHistory);
            }            
            cartInHistory.AddBasket(basket);            
            base.PurchaseBasket(basket);
        }

        public List<ShoppingCartHistory> GetHistory()
        {
            return OrderHistory.Values.ToList();
        }

        public void Notify(string msg)
        {
            var message = new Message(msg);

            if (IsNotification && IsLoggedIn)
            {
                _alertManager.SendNotification(msg, UserName);
                message.Seen = true;
            }
            else{
                alerts.Add(message);            
            }

        }

        public void NotificationOn()
        {
            if (!IsNotification)
            {
                IsNotification = true;
            }
            else throw new Exception("Notification On");
        }

        public void NotificationOff()
        {
            if (IsNotification)
            {
                IsNotification = false;
            }
            else throw new Exception("Notification Off");
        }

        public List<Message> GetMessages()
        {
            lock (_lock)
            {                
                return alerts.ToList();
            }
        }

    }
    
}