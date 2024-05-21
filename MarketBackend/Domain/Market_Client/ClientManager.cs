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

        public static void Dispose()
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
        public Guest GetGuestById(int clientId)
        {
            if (ActiveGuests.TryGetValue(clientId, out var guest))
            {
                return guest;
            }

            return null;
        }

        public Member GetMemberById(int clientId)
        {
            if (MemberxClientId.TryGetValue(clientId, out var member))
            {
                return member;
            }            
            throw new KeyNotFoundException($"Client ID {clientId} not found in members");
        }

        public bool AddToCart(int clientId, int storeId, int productId, int quantity)
        {
            Client client = GetClientById(clientId);
            client?.AddToCart(storeId ,productId, quantity);
            return client is not null;
        }

        public Client Register(int id, string username, string password, string email, int age)
        {
            try
            {
                var newClient = CreateMember(username, password, email, age);
                _clientRepository.Add(newClient);
                MemberxClientId.TryAdd(id, newClient);
                return newClient;
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

        private Member CreateMember(string username, string password, string email, int age)
        {
            var emailParsed = ValidateEmail(email);
            ValidateUserName(username);
            ValidatePassword(password);
            
            var newMember = new Member(UserCounter, username, emailParsed, _security.EncryptPassword(password))
            {
                IsAbove18 = age >= 18
            };
            UserCounter++;
            return newMember;
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

        public Member GetSystemAdmin()
        {
            var allMembers = _clientRepository.getAll();
            var allPossibleAdmin = allMembers.Where(member => member.IsSystemAdmin);
            return allPossibleAdmin.FirstOrDefault();
        }

        public Client RegisterAsSystemAdmin(string username, string password, string email, int age)
        {
            var registerAdmin = GetSystemAdmin();
            if (registerAdmin != null) return registerAdmin;

            try
            {
                registerAdmin = CreateMember(username, password, email, age);
                _clientRepository.Add(registerAdmin);
                registerAdmin.IsSystemAdmin = true;
                return registerAdmin;
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

        public void LoginClient(int id, string username, string password)
        {
            try{
                var client = _clientRepository.GetByUserName(username);
                if(_security.VerifyPassword(password, client.Password) && !client.IsLoggedIn){
                    MemberxClientId.TryAdd(id, client);
                    client.IsLoggedIn = true;
                }
                else
                    throw new Exception(@$"{client.UserName} already logged in.");
                    
            }catch(Exception){
                throw;
            }
        }

        public void LogoutClient(int id)
        {
            try{
                var client = GetMemberById(id);
                
                if(client.IsLoggedIn){
                    client.IsLoggedIn = false;
                    MemberxClientId.TryRemove(new(id, client));
                }
                else{
                    throw new Exception($"{client.UserName} not logged in");
                }
                    
            }catch(Exception){
                throw;
            }
        }

        public void BrowseAsGuest(int id)
        {
            var guest = new Guest(UserCounter);
            ActiveGuests.TryAdd(id, guest);
            UserCounter++;
        }

        public void DeactivateGuest(int id)
        {
            var client = GetGuestById(id);
            ActiveGuests.TryRemove(id, out client);
        }

        public bool CheckMemberIsLoggedIn(int clientId)
        {
            if (MemberxClientId.TryGetValue(clientId, out var member))
            {
                return member.IsLoggedIn;
            }            
            throw new KeyNotFoundException($"Client ID {clientId} not found in members");
        }
    }
   
}