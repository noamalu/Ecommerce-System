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
using System.Security.Policy;
using System.Data;
using MarketBackend.Domain.Shipping;
using Microsoft.IdentityModel.Tokens;


namespace MarketBackend.Domain.Market_Client
{
    public class MarketManagerFacade : IMarketManagerFacade
    {
        private static MarketManagerFacade marketManagerFacade = null;
        private readonly IStoreRepository _storeRepository;
        private readonly ClientManager _clientManager;
        private readonly IPaymentSystemFacade _paymentSystem;
        private readonly IShippingSystemFacade _shippingSystemFacade;
        private int _storeCounter = 1;

        
        private MarketManagerFacade(IShippingSystemFacade shippingSystemFacade, IPaymentSystemFacade paymentSystem){
            _storeRepository = StoreRepositoryRAM.GetInstance();
            _clientManager = ClientManager.GetInstance();
            _paymentSystem = paymentSystem;
            _shippingSystemFacade = shippingSystemFacade;
            _shippingSystemFacade.Connect();
            _paymentSystem.Connect();
            InitiateSystemAdmin();
            
        }

        public static MarketManagerFacade GetInstance(IShippingSystemFacade shippingSystemFacade, IPaymentSystemFacade paymentSystem){
            if (marketManagerFacade == null){
                marketManagerFacade = new MarketManagerFacade(shippingSystemFacade, paymentSystem);
            }
            return marketManagerFacade;
        }

        public static void Dispose(){
            StoreRepositoryRAM.Dispose();
            BasketRepositoryRAM.Dispose();
            ClientRepositoryRAM.Dispose();
            PaymentDetailsRepositoryRAM.Dispose();
            ProductRepositoryRAM.Dispose();
            RoleRepositoryRAM.Dispose();
            ShippingDetailsRepositoryRAM.Dispose();
            StoreRepositoryRAM.Dispose();
            ClientManager.Dispose();
            PurchaseRepositoryRAM.Dispose();
            marketManagerFacade = null;            
        }
        
        public void InitiateSystemAdmin()
        {
            _clientManager.RegisterAsSystemAdmin("system_admin", "system_admin", "system.admin@mail.com", 30);            
        }
        
        public void AddManger(int activeId, int storeId, int toAddId)
        {
            Store store = _storeRepository.GetById(storeId);
            if (store != null && _clientManager.CheckMemberIsLoggedIn(activeId))
            {
                Member activeMember = (Member)_clientManager.GetClientById(activeId);
                Role role = new Role(new StoreManagerRole(RoleName.Manager), activeMember, storeId, toAddId);
                store.AddStaffMember(toAddId, role, activeId);
            }
            else
                throw new Exception("Store doesn't exist!");

        }

        public void AddOwner(int activeId, int storeId, int toAddId)
        {
            Store store = _storeRepository.GetById(storeId);
            if (store != null && _clientManager.CheckMemberIsLoggedIn(activeId))
            {
                Member activeMember = (Member)_clientManager.GetClientById(activeId);
                Role role = new Role(new Owner(RoleName.Owner), activeMember, storeId, toAddId);
                store.AddStaffMember(toAddId, role, activeId);
            }
            else
                throw new Exception("Store doesn't exist!");

        }

        public void AddPermission(int activeId, int storeId, int toAddId, Permission permission)
        {
            Store store = _storeRepository.GetById(storeId);
            if (store != null && _clientManager.CheckMemberIsLoggedIn(activeId))
            {
                store.AddPermission(activeId, toAddId, permission);
            }
            else
                throw new Exception("Store doesn't exist!");
        }

        public void RemovePermission(int activeId, int storeId, int toRemoveId, Permission permission)
        {
            Store store = _storeRepository.GetById(storeId);
            if (store != null && _clientManager.CheckMemberIsLoggedIn(activeId))
            {
                store.RemovePermission(activeId, toRemoveId, permission);
            }
            else
                throw new Exception("Store doesn't exist!");

        }


        public Product AddProduct(int storeId, int userId, string name, string sellMethod, string description, double price, string category, int quantity, bool ageLimit)
        {
            Store store = _storeRepository.GetById(storeId);
            if (store == null && _clientManager.CheckMemberIsLoggedIn(userId)){
                throw new Exception("Store doesn't exists");
            }
            return store.AddProduct(userId, name, sellMethod, description, price, category, quantity, ageLimit);

        }

        public void AddToCart(int clientId, int storeId, int productId, int quantity)
        {
            ClientManager.CheckClientId(clientId);
            Store store = _storeRepository.GetById(storeId);
            bool found = false;
            if (store != null){
                foreach (var product in store._products){
                    if (product._productid == productId){
                        found = true;
                        break;
                    }
                }
                if (found) 
                    _clientManager.AddToCart(clientId, storeId, productId, quantity);
                else
                    throw new Exception($"No productid {productId}");
            }
            else
                throw new Exception($"Store {store} doesn't exists.");
            
        }

        public void CloseStore(int userId, int storeId)
        {
            Store store = _storeRepository.GetById(storeId);
            if (store != null && _clientManager.CheckMemberIsLoggedIn(userId)){
                store.CloseStore(userId);
            }
            else{
                throw new Exception("Store doesn't exists");
            }
        }

    
        public int CreateStore(int id, string storeName, string email, string phoneNum)
        {
            int storeId=-1;
            Client store_founder = _clientManager.GetClientById(id);
            if(store_founder != null && _clientManager.CheckMemberIsLoggedIn(id))
            {
                storeId = _storeCounter++;
                if (_storeRepository.GetById(storeId) != null){
                    throw new Exception("Store exists");
                }
                Store store = new Store(storeId, storeName, email, phoneNum)
                {
                    _active = true
                };
                _storeRepository.Add(store);
                Member activeMember = (Member)_clientManager.GetClientById(id);
                Role role = new Role(new Founder(RoleName.Founder), activeMember, storeId, id);

                store.AddStaffMember(id, role, id); 
            }
            else
            {
                throw new Exception("Store founder must be a Member.");
            }
            return storeId;
        }

        public void EditPurchasePolicy(int storeId)
        {
            throw new NotImplementedException();
        }

        public void EnterAsGuest(int id)
        {
            _clientManager.BrowseAsGuest(id);
        }

        public void ExitGuest(int id)
        {
            _clientManager.DeactivateGuest(id);
        }

        public Member GetFounder(int storeId)
        {
            Store store = _storeRepository.GetById(storeId);
            if (store != null)
            {
                int founderId = store.roles.FirstOrDefault(pair => pair.Value.getRoleName() == RoleName.Founder).Key;
                if (_clientManager.IsMember(founderId))
                    return (Member)_clientManager.GetClientById(founderId);
                else
                    throw new Exception("should not happen! founder is not a member");
            }
            else
                throw new Exception("Store doesn't exist!");
        }

        public List<Member> GetMangers(int storeId)
        {
            Store store = _storeRepository.GetById(storeId);
            if (store != null)
            {

                List<int> managerIds = store.roles.Where(pair => pair.Value.getRoleName() == RoleName.Manager).Select(pair => pair.Key).ToList();
                List<Member> managers = new List<Member>();
                managerIds.ForEach(id => managers.Add((Member)_clientManager.GetClientById(id)));
                return managers;
            }
            else
                throw new Exception("Store doesn't exist!");

        }

        public List<Member> GetOwners(int storeId)
        {
            Store store = _storeRepository.GetById(storeId);
            if (store != null)
            {

                List<int> managerIds = store.roles.Where(pair => pair.Value.getRoleName() == RoleName.Owner).Select(pair => pair.Key).ToList();
                List<Member> managers = new List<Member>();
                managerIds.ForEach(id => managers.Add((Member)_clientManager.GetClientById(id)));
                return managers;
            }
            else
                throw new Exception("Store doesn't exist!");
        }

        public string GetProductInfo(int storeId, int productId)
        {
            Store store = _storeRepository.GetById(storeId);
            if (store == null){
                throw new Exception("Store doesn't exists");
            }
            return store.getProductInfo(productId);
        }

        public string GetInfo(int storeId){
            Store store = _storeRepository.GetById(storeId);
            if (store == null){
                throw new Exception("Store doesn't exists");
            }
            return store.getInfo();
        }

        public List<ShoppingCartHistory> GetPurchaseHistoryByClient(int id)
        {
            return _clientManager.GetPurchaseHistoryByClient(id);
        }

        public List<Purchase> GetPurchaseHistoryByStore(int storeId, int userId)
        {
            Store store = _storeRepository.GetById(storeId);
            if (store != null && _clientManager.CheckMemberIsLoggedIn(userId)){
                return store.getHistory(userId);
            }
            else{
                throw new Exception("Store doesn't exists");
            }
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

        public void LoginClient(int id, string username, string password)
        {
            _clientManager.LoginClient(id, username, password);
        }

        public void LogoutClient(int id)
        {
            _clientManager.LogoutClient(id);
        }

        public void OpenStore(int clientId, int storeId)
        {
            Store store = _storeRepository.GetById(storeId);
            if (store != null && _clientManager.CheckMemberIsLoggedIn(clientId)){
                store.OpenStore(clientId);
            }
            else{
                throw new Exception("Store doesn't exists");
            }
        }

        public void PurchaseCart(int id, PaymentDetails paymentDetails, ShippingDetails shippingDetails) //clientId
        {
            ClientManager.CheckClientId(id);
            var client = _clientManager.GetClientById(id);
            var baskets = client.Cart.GetBaskets();
            if (baskets.IsNullOrEmpty()){
                throw new Exception("Empty cart.");
            }
            var stores = new List<Store>();
            foreach(var basket in baskets){
                var store = _storeRepository.GetById(basket.Key);
                stores.Add(store);
                if(!store.checkBasketInSupply(basket.Value)) throw new Exception("unavailable."); 
                if(!store.checklegalBasket(basket.Value, client.IsAbove18)) throw new Exception("unavailable.");               
            }
            foreach(var store in stores){
                var totalPrice = store.CalculateBasketPrice(baskets[store.StoreId]);
                if(_paymentSystem.Pay(paymentDetails, totalPrice) > 0) {
                    store.PurchaseBasket(id, baskets[store.StoreId]);
                    client.PurchaseBasket(id, baskets[store.StoreId]);
                }
                else 
                    throw new Exception("payment failed.");
            }

            _shippingSystemFacade.OrderShippment(shippingDetails);

            

        }

        public void Register(int id, string username, string password, string email, int age)
        {
            _clientManager.Register(id, username, password, email, age);
        }

        public void RemoveFromCart(int clientId, int productId, int basketId, int quantity)
        {
            _clientManager.RemoveFromCart(clientId, productId, basketId, quantity);
        }

        public void RemoveManger(int activeId, int storeId, int toRemoveId)
        {
            RemoveStaffMember(storeId, activeId, null, toRemoveId);
        }

        public void RemoveOwner(int activeId, int storeId, int toRemoveId)
        {
            RemoveStaffMember(storeId, activeId, null ,toRemoveId);
        }

        public void RemovePermission(int activeId, int storeId, int toRemoveId)
        {
            throw new NotImplementedException();
        }

        public void RemoveProduct(int storeId,int userId, int productId)
        {
            Store store = _storeRepository.GetById(storeId);
            if (store == null){
                throw new Exception("Store doesn't exists");
            }
            store.RemoveProduct(userId, productId);
        }

        public void RemoveStaffMember(int storeId, int activeId, Role role, int toRemoveId)
        {
            Store store = _storeRepository.GetById(storeId);
            if (store != null)
            {
                store.RemoveStaffMember(toRemoveId, activeId);
            }
            else
                throw new Exception("Store doesn't exist!");
        }

        public bool ResToStoreManageReq(int id)
        {
            throw new NotImplementedException();
        }

        public bool ResToStoreOwnershipReq(int id)
        {
            throw new NotImplementedException();
        }

        public HashSet<Product> SearchByCategory(string category)
        {
            return SearchingManager.searchByCategory(category);
        }

        public HashSet<Product> SearchByKeyWords(string keywords)
        {
            return SearchingManager.searchByKeyword(keywords);
        }

        public HashSet<Product> SearchByName(string name)
        {
            return SearchingManager.serachByName(name);
        }
        public HashSet<Product> SearchByCategoryWithStore(int storeId, string category)
        {
            return SearchingManager.searchByCategoryWithStore(storeId, category);
        }

        public HashSet<Product> SearchByKeyWordsWithStore(int storeId, string keywords)
        {
            return SearchingManager.searchByKeywordWithStore(storeId, keywords);
        }

        public HashSet<Product> SearchByNameWithStore(int storeId, string name)
        {
            return SearchingManager.serachByNameWithStore(storeId, name);
        }

        public void Filter(HashSet<Product> products, string category, double lowPrice, double highPrice, double lowProductRate, double highProductRate, double lowStoreRate, double highStoreRate)
        {
            FilterParameterManager filter = new FilterParameterManager(category, lowPrice, highPrice, lowProductRate, highProductRate, lowStoreRate, highStoreRate);
            filter.Filter(products);
        }

        // public void UpdateProductDiscount(int storeId, int userId, int productId, double discount)
        // {
        

        //     if (_storeRepository.GetById(storeId) != null)
        //     {
        //         _storeRepository.GetById(storeId).UpdateProductDiscount(userId, productId, discount);
        //     }
        //     else
        //     {
        //         throw new Exception("Store not found");
        //     }

        // }

        public void UpdateProductPrice(int storeId, int userId,  int productId, double price)
        {
            if (_storeRepository.GetById(storeId) != null && _clientManager.CheckMemberIsLoggedIn(userId))
            {
                _storeRepository.GetById(storeId).UpdateProductPrice(userId, productId, price);
            }
            else
            {
                throw new Exception("Store not found");
            }

        }

        public void UpdateProductQuantity(int storeId, int userId, int productId, int quantity)
        {
            if (_storeRepository.GetById(storeId) != null && _clientManager.CheckMemberIsLoggedIn(userId)) 
            {
                _storeRepository.GetById(storeId).UpdateProductPrice(userId, productId, quantity);
            }
            else
            {
                throw new Exception("Store not found");
            }
        }

        public ShoppingCart ViewCart(int id)
        {
            ClientManager.CheckClientId(id);
            return _clientManager.ViewCart(id);
        }

        public void AddStaffMember(int storeId, int activeId, Role role, int toAddId){
            Store store = _storeRepository.GetById(storeId);
            if (store != null && _clientManager.CheckMemberIsLoggedIn(activeId))
            {
                store.AddStaffMember(toAddId, role, activeId);
            }
            else
                throw new Exception("Store doesn't exist!");
        }

        public Store GetStore(int storeId){
            return _storeRepository.GetById(storeId);
        }

        public int GetMemberIDrByUserName(string userName)
        {
            return _clientManager.GetMemberIDrByUserName(userName); 
        }

        public void AddKeyWord(string keyWord, int storeId, int productId)
        {
            Store store = _storeRepository.GetById(storeId);
            if (store != null){

            }
            else
                throw new Exception("Store doesn't exist!");
        }
    }
}
