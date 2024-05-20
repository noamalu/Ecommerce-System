using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketBackend.Domain.Models;
using MarketBackend.DAL;
using MarketBackend.Domain.Payment;
using MarketBackend.Services.Interfaces;
using Microsoft.Extensions.Logging;


namespace MarketBackend.Domain.Market_Client
{
    public class MarketManagerFacade : IMarketManagerFacade
    {
        private static MarketManagerFacade marketManagerFacade = null;
        private readonly IStoreRepository _storeRepository;
        private readonly ClientManager _clientManager;
        private readonly IPaymentSystemFacade _paymentSystem;
        private int _storeCounter = 0;

        private readonly ILogger<MarketManagerFacade> _logger;
        private MarketManagerFacade(){
            _storeRepository = StoreRepositoryRAM.GetInstance();
            _clientManager = ClientManager.GetInstance();
            _paymentSystem = new PaymentSystemProxy();
            // TODO: initializie system admin
            // _logger = logger;
        }

        public static MarketManagerFacade GetInstance(){
            if (marketManagerFacade == null){
                marketManagerFacade = new MarketManagerFacade();
            }
            return marketManagerFacade;
        }
        
        public void AddManger(int activeId, int storeId, int toAddId)
        {
            throw new NotImplementedException();
        }

        public void AddOwner(int activeId, int storeId, int toAddId)
        {
            throw new NotImplementedException();
        }

        public void AddPermission(int activeId, int storeId, int toAddId)
        {
            throw new NotImplementedException();
        }

        public void AddProduct(int productId, string productName, int storeId, string category, double price, int quantity, double discount)
        {
            throw new NotImplementedException();
        }

        public void AddToCart(int clientId, int storeId, int productId, int quantity)
        {
            ClientManager.CheckClientId(clientId);
            _clientManager.AddToCart(clientId, storeId, productId, quantity);
            // _logger.LogInformation($"Product id={productId} were added to client id={clientId} cart, to storeId={storeId} basket.!");
        }

        public void BrowseGuest()
        {
            throw new NotImplementedException();
        }

        public void CloseStore(int userId, int storeId)
        {
            Store store = _storeRepository.GetById(storeId);
            if (store != null){
                store.CloseStore(userId);
            }
            else{
                throw new Exception("Store doesn't exists");
            }
        }

        public void CreateStore(int id, string storeName, string email, string phoneNum)
        {
            Client store_founder = _clientManager.GetClientById(id);
            if(store_founder != null)
            {
                int storeId = _storeCounter++;
                if (_storeRepository.GetById(storeId) != null){
                    throw new Exception("Store exists");
                }
                Store store = new Store(storeId, storeName, email, phoneNum)
                {
                    _active = true
                };
                _storeRepository.Add(store);
                // make as a founder/ owner
            }
            else
            {
                throw new Exception("Store founder must be a Member.");
            }
        }

        public void EditPurchasePolicy(int storeId)
        {
            throw new NotImplementedException();
        }

        public void EnterAsGuest()
        {
            throw new NotImplementedException();
        }

        public void ExitGuest()
        {
            throw new NotImplementedException();
        }

        public Member GetFounder(int storeId)
        {
            throw new NotImplementedException();
        }

        public List<Member> GetMangers(int storeId)
        {
            throw new NotImplementedException();
        }

        public List<Member> GetOwners(int storeId)
        {
            throw new NotImplementedException();
        }

        public string GetProductInfo(int productId)
        {
            throw new NotImplementedException();
        }

        public string GetInfo(int storeId){
            throw new NotImplementedException();
        }

        public List<Purchase> GetPurchaseHistoryByClient(int id)
        {
            throw new NotImplementedException();
        }

        public List<Purchase> GetPurchaseHistoryByStore(int id)
        {
            throw new NotImplementedException();
        }

        public bool HasPermission()
        {
            throw new NotImplementedException();
        }

        public bool IsAvailable(int storeId)
        {
            Store store = _storeRepository.GetById(storeId);
            if (store != null){
                return store._active;
            }
            else{
                throw new Exception("Store doesn't exists");
            }
        }

        public void LoginClient(string username, string password)
        {
            throw new NotImplementedException();
        }

        public void LogoutClient(int id)
        {
            throw new NotImplementedException();
        }

        public void OpenStore(int clientId, int storeId)
        {
            Store store = _storeRepository.GetById(storeId);
            if (store != null){
                store.OpenStore(clientId);
            }
            else{
                throw new Exception("Store doesn't exists");
            }
        }

        public void PurchaseCart(int id, PaymentDetails paymentDetails) //clientId
        {
            ClientManager.CheckClientId(id);
            var client = _clientManager.GetClientById(id);
            var baskets = client.Cart.GetBaskets();
            var stores = new List<Store>();
            foreach(var basket in baskets){
                var store = _storeRepository.GetById(basket.Key);
                stores.Add(store);
                if(!store.checkBasketInSupply(basket.Value)) throw new Exception("unavailable.");                
            }
            foreach(var store in stores){
                var totalPrice = store.CalculateBasketPrice(baskets[store.StoreId]);
                if(_paymentSystem.Pay(paymentDetails, totalPrice) > 0) 
                    store.PurchaseBasket(id, baskets[store.StoreId]);
                else 
                    throw new Exception("payment failed.");
            }

        }

        public void Register(string username, string password, string email, int age)
        {
            _clientManager.Register(username, password, email, age);
        }

        public void RemoveFromCart(int clientId, int productId, int basketId, int quantity)
        {
            _clientManager.RemoveFromCart(clientId, productId, basketId, quantity);
        }

        public void RemoveManger(int activeId, int storeId, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        public void RemoveOwner(int activeId, int storeId, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        public void RemovePermission(int activeId, int storeId, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        public void RemoveProduct(int productId)
        {
            throw new NotImplementedException();
        }

        public void RemoveStaffMember(int storeId, int activeId, Role role, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        public bool ResToStoreManageReq(int id)
        {
            throw new NotImplementedException();
        }

        public bool ResToStoreOwnershipReq(int id)
        {
            throw new NotImplementedException();
        }

        public List<Product> SearchByCategory(string category)
        {
            throw new NotImplementedException();
        }

        public List<Product> SearchByKeyWords(string keywords)
        {
            throw new NotImplementedException();
        }

        public List<Product> SearchByName(string name)
        {
            throw new NotImplementedException();
        }

        public void UpdateProductDiscount(int productId, double discount)
        {
            throw new NotImplementedException();
        }

        public void UpdateProductPrice(int productId, double price)
        {
            throw new NotImplementedException();
        }

        public void UpdateProductQuantity(int productId, int quantity)
        {
            throw new NotImplementedException();
        }

        public ShoppingCart ViewCart(int id)
        {
            ClientManager.CheckClientId(id);
            return _clientManager.ViewCart(id);
        }

        public void AddStaffMember(int storeId, int activeId, Role role, int toAddId){
            throw new NotImplementedException();
        }
    }
}
