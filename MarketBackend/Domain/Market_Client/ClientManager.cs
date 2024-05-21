using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Xml;
using MarketBackend.DAL;
using MarketBackend.Domain.Models;
using MarketBackend.Domain.Security;
using MarketBackend.Services.Interfaces;
using Microsoft.VisualBasic;

namespace MarketBackend.Domain.Market_Client
{
    public class ClientManager
    {
        private static ClientManager Manager = null;
        private static ConcurrentDictionary<int, Member> MemberxClientId {get; set;}
        private static ConcurrentDictionary<int, Guest> ActiveGuests {get; set;}        
        private readonly IClientRepository _clientRepository;
        private readonly SecurityManager _security;
        private int UserCounter {get; set;}
        private object _lock = new object();
    
        private ClientManager()
        {
            UserCounter = 1;
            ActiveGuests = new ConcurrentDictionary<int, Guest>();
            MemberxClientId = new ConcurrentDictionary<int, Member>();
            _clientRepository = ClientRepositoryRAM.GetInstance();
            _security = SecurityManager.GetInstance();
        }

        public static ClientManager GetInstance()
        {
            Manager ??= new ClientManager();
            return Manager;
        }

        public void Dispose()
        {
            Manager = new ClientManager();

        }

        public static bool CheckClientId(int clientId)
        {
            if (MemberxClientId.ContainsKey(clientId) || ActiveGuests.ContainsKey(clientId))
                return true;

            throw new KeyNotFoundException($"Client ID {clientId} not found in members or active guests.");
        }

        public Client GetClientById(int clientId)
        {
            if (MemberxClientId.TryGetValue(clientId, out var member))
            {
                return member;
            }

            if (ActiveGuests.TryGetValue(clientId, out var guest))
            {
                return guest;
            }

            return null;
        }

        public bool AddToCart(int clientId, int storeId, int productId, int quantity)
        {
            Client client = GetClientById(clientId);
            client?.AddToCart(storeId ,productId, quantity);
            return client is not null;
        }

        public void Register(string username, string password, string email, int age)
        {
            try{
                var emailParsed = ValidateEmail(email);
                ValidateUserName(username);
                ValidatePassword(password);

                //hash password first
                _clientRepository.Add(new Member(UserCounter, username, emailParsed, password));
                UserCounter++;
            }
            catch (ArgumentException){
                throw;
            }
        }

        private MailAddress ValidateEmail(string email){
            try{
                return new MailAddress(email);                
            }
            catch (FormatException){
                throw new ArgumentException("Email address is not valid.");
            }
            
        }

        private bool ValidatePassword(string password){
            if (string.IsNullOrWhiteSpace(password) || password.Contains(' '))
            {
                throw new ArgumentException("Password cannot be empty or contain spaces.", nameof(password));
            }
            return true;            
        }

        private bool ValidateUserName(string username){
            if(_clientRepository.ContainsUserName(username))
            {
                throw new ArgumentException("Username already exists.", nameof(username));
            }
            return true;            
        }

        public void RemoveFromCart(int clientId, int productId, int basketId, int quantity)
        {
            var client = GetClientById(clientId);
            client.RemoveFromCart(basketId, productId, quantity);
        }

        public ShoppingCart ViewCart(int clientId)
        {
            var client = GetClientById(clientId);
            return client.Cart;
        }

        public bool IsMember(int clientId){
            return MemberxClientId.ContainsKey(clientId);
        }
    }
   
}