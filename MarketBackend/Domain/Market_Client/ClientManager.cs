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
        private static ConcurrentDictionary<string, Member> MemberByToken {get; set;}
        private static ConcurrentDictionary<string, Guest> GuestBySession {get; set;}        
        private readonly IClientRepository _clientRepository;
        private readonly SecurityManager _security;
        public ClientManager(int userCounter) 
        {
            this.UserCounter = userCounter;
   
        }
                private int UserCounter {get; set;}
        private object _lock = new object();
    
        private ClientManager()
        {
            UserCounter = 1;
            GuestBySession = new ConcurrentDictionary<string, Guest>();
            MemberByToken = new ConcurrentDictionary<string, Member>();
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

        public static bool CheckClientIdentifier(string identifier)
        {
            if (MemberByToken.ContainsKey(identifier) || GuestBySession.ContainsKey(identifier))
                return true;

            throw new KeyNotFoundException($"Client ID {identifier} not found in members or active guests.");
        }

        public Client GetClientByIdentifier(string identifier)
        {
            if (MemberByToken.TryGetValue(identifier, out var member))
            {
                return member;
            }

            if (GuestBySession.TryGetValue(identifier, out var guest))
            {
                return guest;
            }

            return null;
        }
        public async Task<Guest> GetGuestByIdentifier(string identiifer)
        {
            if (GuestBySession.TryGetValue(identiifer, out var guest))
            {
                return guest;
            }

            return null;
        }

        public async Task<Member> GetMemberByIdentifier(string identifier)
        {
            if (MemberByToken.TryGetValue(identifier, out var member))
            {
                return member;
            }            
            throw new KeyNotFoundException($"identifier={identifier} not found in members");
        }

        public async Task<bool> AddToCart(string identifier, int storeId, int productId, int quantity)
        {
            Client client = GetClientByIdentifier(identifier);
            await client?.AddToCart(storeId ,productId, quantity);
            return client is not null;
        }

        public async Task<Client> Register(string username, string password, string email, int age)
        {
            try
            {
                var newClient = await CreateMember(username, password, email, age);
                await _clientRepository.Add(newClient);
                return newClient;
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

        private async Task<Member> CreateMember(string username, string password, string email, int age)
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

        private async Task<bool> ValidateUserName(string username){
            if(await _clientRepository.ContainsUserName(username))
            {
                throw new ArgumentException("Username already exists.", nameof(username));
            }
            return true;            
        }

        public async Task RemoveFromCart(string identifier, int productId, int storeId, int quantity)
        {
            var client = GetClientByIdentifier(identifier);
            await client.RemoveFromCart(storeId, productId, quantity);
        }

        public ShoppingCart ViewCart(string identifier)
        {
            var client = GetClientByIdentifier(identifier);
            return client.Cart;
        }

        public async Task<bool> IsMember(string userName){
            return await _clientRepository.ContainsUserName(userName);
        }

        public async Task<Member> GetSystemAdmin()
        {
            var allMembers = await _clientRepository.getAll();
            var allPossibleAdmin = allMembers.Where(member => member.IsSystemAdmin);
            return allPossibleAdmin.FirstOrDefault();
        }

        public async Task<Client> RegisterAsSystemAdmin(string username, string password, string email, int age)
        {
            var registerAdmin = await GetSystemAdmin();
            if (registerAdmin != null) return registerAdmin;

            try
            {
                registerAdmin = await CreateMember(username, password, email, age);
                registerAdmin.IsSystemAdmin = true;
                await _clientRepository.Add(registerAdmin);
                return registerAdmin;
            }
            catch (ArgumentException)
            {
                throw;
            }
        }

        public async Task<string> LoginClient(string username, string password)
        {
            try{
                var client = await _clientRepository.GetByUserName(username);
                
                if(_security.VerifyPassword(password, client.Password) && !client.IsLoggedIn){
                    var token = _security.GenerateToken(username);
                    MemberByToken.TryAdd(token, client);
                    client.IsLoggedIn = true;
                    return token;
                }
                else
                    throw new Exception(@$"{client.UserName} already logged in.");
                    
            }catch(Exception){
                throw;
            }
        }

        public async Task LogoutClient(string identifier)
        {
            try{
                if (_security.ValidateToken(identifier))
                {
                    var client = await GetMemberByIdentifier(identifier);

                    if (client.IsLoggedIn)
                    {
                        client.IsLoggedIn = false;
                        MemberByToken.TryRemove(new(identifier, client));
                    }
                    else
                    {
                        throw new Exception($"{client.UserName} not logged in");
                    }
                }
                else
                {
                    throw new Exception($"Invalid token");
                }
                
                    
            }catch(Exception){
                throw;
            }
        }

        public async Task BrowseAsGuest(string identifier)
        {
            var guest = new Guest(UserCounter);
            GuestBySession.TryAdd(identifier, guest);
            UserCounter++;
        }

        public async Task DeactivateGuest(string identifier)
        {
            var client = await GetGuestByIdentifier(identifier);
            GuestBySession.TryRemove(identifier, out client);
        }


        public async Task<int> GetMemberIDrByUserName(string userName)
        {
            if(await _clientRepository.ContainsUserName(userName))
            {
                return (await _clientRepository.GetByUserName(userName)).Id;
            }
            return -1;       
        }

        public async Task<Member> GetMember(string userName)
        {
            if(await _clientRepository.ContainsUserName(userName))
            {
                return await _clientRepository.GetByUserName(userName);
            }
            return null;       
        }

        public async Task<Member> GetMemberByUserName(string userName)
        {
            return await _clientRepository.GetByUserName(userName);    
        }

        public bool CheckMemberIsLoggedIn(string identifier)
        {
            if (MemberByToken.TryGetValue(identifier, out var member))
            {
                return member.IsLoggedIn;
            }            
            throw new KeyNotFoundException($"identifier= {identifier} not found in members");
        }

        public async Task<List<ShoppingCartHistory>> GetPurchaseHistoryByClient(string userName)
        {
            return (await GetMemberByUserName(userName)).GetHistory();
        }

        public async Task PurchaseBasket(string identifier, Basket basket)
        {
            if(await GetMemberByIdentifier(identifier) is not null)
                (await GetMemberByIdentifier(identifier))?.PurchaseBasket(basket);
            else
                await GetClientByIdentifier(identifier)?.PurchaseBasket(basket);
        }

        public async Task NotificationOn(string identifier)
        {
            if((await GetMemberByIdentifier(identifier)) is not null)
                (await GetMemberByIdentifier(identifier))?.NotificationOn();
            else
                {
                    throw new Exception($"{identifier} not logged in");
                }
        }

        public async Task NotificationOff(string identifier)
        {
            if((await GetMemberByIdentifier(identifier)) is not null)
                (await GetMemberByIdentifier(identifier))?.NotificationOff();
            else
                {
                    throw new Exception($"{identifier} not logged in");
                }
        }

        public async Task SetMemberNotifications(string identifier, bool on)
        {
            if(on){
                await NotificationOn(identifier);
            }else{
                await NotificationOff(identifier);
            }
        }
        public string GetTokenByUserName(string userName)
        {
            return MemberByToken.Where(pair => pair.Value.UserName == userName).FirstOrDefault().Key;
        }
    }
   
}