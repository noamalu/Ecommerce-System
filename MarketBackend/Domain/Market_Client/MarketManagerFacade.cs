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
            ProductRepositoryRAM.Dispose();
            RoleRepositoryRAM.Dispose();
            StoreRepositoryRAM.Dispose();
            ClientManager.Dispose();
            PurchaseRepositoryRAM.Dispose();
            marketManagerFacade = null;            
        }
        
        public async Task InitiateSystemAdmin()
        {
            await _clientManager.RegisterAsSystemAdmin("system_admin", "system_admin", "system.admin@mail.com", 30);            
        }
        
        public async Task AddManger(string identifier, int storeId, string toAddUserName)
        {
            Store store = await _storeRepository.GetById(storeId);
            if (store != null && _clientManager.CheckMemberIsLoggedIn(identifier))
            {
                Member activeMember = (Member)_clientManager.GetClientByIdentifier(identifier);
                Role role = new Role(new StoreManagerRole(RoleName.Manager), activeMember, storeId, toAddUserName);
                await store.AddStaffMember(toAddUserName, role, activeMember.UserName);
            }
            else
                throw new Exception("Store doesn't exist!");

        }

        public async Task AddOwner(string identifier, int storeId, string userName)
        {
            Store store = await _storeRepository.GetById(storeId);
            if (store != null && _clientManager.CheckMemberIsLoggedIn(identifier))
            {
                Member activeMember = (Member)_clientManager.GetClientByIdentifier(identifier);
                Role role = new Role(new Owner(RoleName.Owner), activeMember, storeId, userName);
                await store.AddStaffMember(userName, role, activeMember.UserName);
            }
            else
                throw new Exception("Store doesn't exist!");

        }

        public async Task AddPermission(string identifier, int storeId, string toAddUserName, Permission permission)
        {
            Store store = await _storeRepository.GetById(storeId);
            if (store != null && _clientManager.CheckMemberIsLoggedIn(identifier))
            {
                Member activeMember = (Member)_clientManager.GetClientByIdentifier(identifier);
                store.AddPermission(activeMember.UserName, toAddUserName, permission);
            }
            else
                throw new Exception("Store doesn't exist!");
        }

        public async Task RemovePermission(string identifier, int storeId, string toRemoveUserName, Permission permission)
        {
            Store store = await _storeRepository.GetById(storeId);
            if (store != null && _clientManager.CheckMemberIsLoggedIn(identifier))
            {
                Member activeMember = (Member)_clientManager.GetClientByIdentifier(identifier);
                store.RemovePermission(activeMember.UserName, toRemoveUserName, permission);
            }
            else
                throw new Exception("Store doesn't exist!");

        }


        public async Task<Product> AddProduct(int storeId, string identifier, string name, string sellMethod, string description, double price, string category, int quantity, bool ageLimit)
        {
            Store store = await _storeRepository.GetById(storeId);
            if (store != null && _clientManager.CheckMemberIsLoggedIn(identifier))
            {
                Member activeMember = (Member)_clientManager.GetClientByIdentifier(identifier);
                return await store.AddProduct(activeMember.UserName, name, sellMethod, description, price, category, quantity, ageLimit);
            }
            else
                throw new Exception("Store doesn't exist!");

        }

        public async Task AddToCart(string identifier, int storeId, int productId, int quantity)
        {
            ClientManager.CheckClientIdentifier(identifier);
            Store store = await _storeRepository.GetById(storeId);
            bool found = false;
            if (store != null){
                foreach (var product in store._products){
                    if (product._productId == productId){
                        found = true;
                        break;
                    }
                }
                if (found) 
                    await _clientManager.AddToCart(identifier, storeId, productId, quantity);
                else
                    throw new Exception($"No productid {productId}");
            }
            else
                throw new Exception($"Store {store} doesn't exists.");
            
        }

        public async Task CloseStore(string identifier, int storeId)
        {
            Store store = await _storeRepository.GetById(storeId);
            if (store != null && _clientManager.CheckMemberIsLoggedIn(identifier)){
                Member activeMember = (Member)_clientManager.GetClientByIdentifier(identifier);
                store.CloseStore(activeMember.UserName);
            }
            else{
                throw new Exception("Store doesn't exists");
            }
        }

    
        public async Task<int> CreateStore(string identifier, string storeName, string email, string phoneNum)
        {
            int storeId=-1;
            Client store_founder = _clientManager.GetClientByIdentifier(identifier);
            if(store_founder != null && _clientManager.CheckMemberIsLoggedIn(identifier))
            {
                storeId = _storeCounter++;
                Store store1 = await _storeRepository.GetById(storeId);
                if (store1 != null){
                    throw new Exception("Store exists");
                }
                Store store = new Store(storeId, storeName, email, phoneNum)
                {
                    _active = true
                };
                await _storeRepository.Add(store);
                // _storeRepository.Add(store);
                Member activeMember = (Member)_clientManager.GetClientByIdentifier(identifier);
                Role role = new Role(new Founder(RoleName.Founder), activeMember, storeId, activeMember.UserName);

                store.SubscribeStoreOwner(activeMember);
                await store.AddStaffMember(activeMember.UserName, role, activeMember.UserName); //adds himself
                
            }
            else
            {
                throw new Exception("Store founder must be a Member.");
            }
            return storeId;
        }

        public Task EditPurchasePolicy(int storeId)
        {
            throw new NotImplementedException();
        }

        public async Task EnterAsGuest(string identifier)
        {
            await _clientManager.BrowseAsGuest(identifier);
        }

        public async Task ExitGuest(string identifier)
        {
            await _clientManager.DeactivateGuest(identifier);
        }

        public async Task<Member> GetFounder(int storeId)
        {
            Store store = await _storeRepository.GetById(storeId);
            if (store != null)
            {
                string founderUsername = store.roles.FirstOrDefault(pair => pair.Value.getRoleName() == RoleName.Founder).Key;
                if (await _clientManager.IsMember(founderUsername))
                    return await _clientManager.GetMemberByUserName(founderUsername);
                else
                    throw new Exception("should not happen! founder is not a member");
            }
            else
                throw new Exception("Store doesn't exist!");
        }

        public async Task<List<Member>> GetMangers(int storeId)
        {
            Store store = await _storeRepository.GetById(storeId);
            if (store != null)
            {

                List<string> usernames = store.roles.Where(pair => pair.Value.getRoleName() == RoleName.Manager).Select(pair => pair.Key).ToList();
                List<Member> managers = new();
                usernames.ForEach(async username => managers.Add(await _clientManager.GetMemberByUserName(username)));
                return managers;
            }
            else
                throw new Exception("Store doesn't exist!");

        }

        public async Task<List<Member>> GetOwners(int storeId)
        {
            Store store = await _storeRepository.GetById(storeId);
            if (store != null)
            {

                List<string> usernames = store.roles.Where(pair => pair.Value.getRoleName() == RoleName.Owner).Select(pair => pair.Key).ToList();
                List<Member> managers = new List<Member>();
                usernames.ForEach(async useerName => managers.Add(await _clientManager.GetMemberByUserName(useerName)));
                return managers;
            }
            else
                throw new Exception("Store doesn't exist!");
        }

        public async Task<string> GetProductInfo(int storeId, int productId)
        {
            Store store = await _storeRepository.GetById(storeId);
            if (store == null){
                throw new Exception("Store doesn't exists");
            }
            return store.getProductInfo(productId);
        }

        public async Task<Product> GetProduct(int storeId, int productId)
        {
            Store store = await _storeRepository.GetById(storeId);
            if (store == null){
                throw new Exception("Store doesn't exists");
            }
            return store.GetProduct(productId);
        }

        public async Task<string> GetInfo(int storeId){
            Store store = await _storeRepository.GetById(storeId);
            if (store == null){
                throw new Exception("Store doesn't exists");
            }
            return store.getInfo();
        }

        public async Task<List<ShoppingCartHistory>> GetPurchaseHistoryByClient(string userName)
        {
            return await _clientManager.GetPurchaseHistoryByClient(userName);
        }

        public async Task<List<Purchase>> GetPurchaseHistoryByStore(int storeId, string identifier)
        {
            Store store = await _storeRepository.GetById(storeId);
            if (store != null){           
                var member  = await _clientManager.GetMemberByIdentifier(identifier);
                return store.getHistory(member.UserName);
            }
            else{
                throw new Exception("Store doesn't exists");
            }
        }

        public async Task<bool> IsAvailable(int storeId)
        {
            Store store = await _storeRepository.GetById(storeId);
            if (store != null){
                return store._active;
            }
            else{
                throw new Exception("Store doesn't exists");
            }
        }

        public async Task<string> LoginClient(string username, string password)
        {
            return await _clientManager.LoginClient(username, password);
        }

        public async Task LogoutClient(string identifier)
        {
            await _clientManager.LogoutClient(identifier);
        }

        public async Task OpenStore(string identifier, int storeId)
        {
            Store store = await _storeRepository.GetById(storeId);
            if (store != null && _clientManager.CheckMemberIsLoggedIn(identifier)){
                Member activeMember = (Member) _clientManager.GetClientByIdentifier(identifier);
                store.OpenStore(activeMember.UserName);
            }
            else{
                throw new Exception("Store doesn't exists");
            }
        }

        public async Task PurchaseCart(string identifier, PaymentDetails paymentDetails, ShippingDetails shippingDetails) //clientId
        {
            ClientManager.CheckClientIdentifier(identifier);
            var client = _clientManager.GetClientByIdentifier(identifier);
            var baskets = await client.Cart.GetBaskets();
            if (baskets.IsNullOrEmpty()){
                throw new Exception("Empty cart.");
            }
            var stores = new List<Store>();
            foreach(var basket in baskets){
                var store = await _storeRepository.GetById(basket.Key);
                stores.Add(store);
                if(!store.checkBasketInSupply(basket.Value)) throw new Exception("unavailable."); 
                if(!store.checklegalBasket(basket.Value, client.IsAbove18)) throw new Exception("unavailable.");               
            }
            foreach(var store in stores){
                var totalPrice = store.CalculateBasketPrice(baskets[store.StoreId]);
                if(_paymentSystem.Pay(paymentDetails, totalPrice) > 0) {
                    if(_shippingSystemFacade.OrderShippment(shippingDetails) > 0){
                        await store.PurchaseBasket(identifier, baskets[store.StoreId]);
                        await _clientManager.PurchaseBasket(identifier, baskets[store.StoreId]);
                    }
                    else{
                        throw new Exception("shippment failed.");
                    }                  
                }
                else 
                    throw new Exception("payment failed.");
            }           

        }

        public async Task Register(string username, string password, string email, int age)
        {
            await _clientManager.Register(username, password, email, age);
        }

        public async Task RemoveFromCart(string identifier, int productId, int storeId, int quantity)
        {
            await _clientManager.RemoveFromCart(identifier, productId, storeId, quantity);
        }

        public async Task RemoveManger(string identifier, int storeId, string toRemoveUserName)
        {
            await RemoveStaffMember(storeId, identifier, toRemoveUserName);
        }

        public async Task RemoveOwner(string identifier, int storeId, string toRemoveUserName)
        {
            await RemoveStaffMember(storeId, identifier, toRemoveUserName);
        }

        public async Task RemoveProduct(int storeId, string identifier, int productId)
        {
            Store store = await _storeRepository.GetById(storeId);
            if (store == null){
                throw new Exception("Store doesn't exists");
            }
            Member activeMember = (Member)_clientManager.GetClientByIdentifier(identifier);
            await store.RemoveProduct(activeMember.UserName, productId);
        }

        public async Task RemoveStaffMember(int storeId, string identifier, string toRemoveUserName)
        {
            Store store = await _storeRepository.GetById(storeId);
            if (store != null)
            {
                Member activeMember = (Member)_clientManager.GetClientByIdentifier(identifier);
                store.RemoveStaffMember(toRemoveUserName, activeMember.UserName);
            }
            else
                throw new Exception("Store doesn't exist!");
        }

        public Task<bool> ResToStoreManageReq(string identifier)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ResToStoreOwnershipReq(string identifier)
        {
            throw new NotImplementedException();
        }

        public async Task<HashSet<Product>> SearchByCategory(string category)
        {
            return await SearchingManager.searchByCategory(category);
        }

        public async Task<HashSet<Product>> SearchByKeyWords(string keywords)
        {
            return await SearchingManager.searchByKeyword(keywords);
        }

        public async Task<HashSet<Product>> SearchByName(string name)
        {
            return await SearchingManager.serachByName(name);
        }
        public async Task<HashSet<Product>> SearchByCategoryWithStore(int storeId, string category)
        {
            return await SearchingManager.searchByCategoryWithStore(storeId, category);
        }

        public async Task<HashSet<Product>> SearchByKeyWordsWithStore(int storeId, string keywords)
        {
            return await SearchingManager.searchByKeywordWithStore(storeId, keywords);
        }

        public async Task<HashSet<Product>> SearchByNameWithStore(int storeId, string name)
        {
            return await SearchingManager.serachByNameWithStore(storeId, name);
        }

        public async Task Filter(HashSet<Product> products, string category, double lowPrice, double highPrice, double lowProductRate, double highProductRate, double lowStoreRate, double highStoreRate)
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

        public async Task UpdateProductPrice(int storeId, string identifier,  int productId, double price)
        {
            if (_storeRepository.GetById(storeId) != null && _clientManager.CheckMemberIsLoggedIn(identifier))
            {
                Member activeMember = (Member)_clientManager.GetClientByIdentifier(identifier);
                (await _storeRepository.GetById(storeId)).UpdateProductPrice(activeMember.UserName, productId, price);
            }
            else
            {
                throw new Exception("Store not found");
            }

        }

        public async Task UpdateProductQuantity(int storeId, string identifier, int productId, int quantity)
        {
            if (_storeRepository.GetById(storeId) != null && _clientManager.CheckMemberIsLoggedIn(identifier)) 
            {
                Member activeMember = (Member)_clientManager.GetClientByIdentifier(identifier);
                (await _storeRepository.GetById(storeId)).UpdateProductQuantity(activeMember.UserName, productId, quantity);
            }
            else
            {
                throw new Exception("Store not found");
            }
        }

        public async Task<ShoppingCart> ViewCart(string identifier)
        {
            ClientManager.CheckClientIdentifier(identifier);
            return _clientManager.ViewCart(identifier);
        }

        public async Task AddStaffMember(int storeId, string identifier, string roleName, string toAddUserName){            
            Store store = await _storeRepository.GetById(storeId);
            if (store != null && _clientManager.CheckMemberIsLoggedIn(identifier))
            {
                Member appoint = await _clientManager.GetMemberByIdentifier(identifier);
                Member appointe = await _clientManager.GetMemberByUserName(toAddUserName);
                RoleType roleType = RoleType.GetRoleTypeFromDescription(roleName);
                Role role = new(roleType, appoint, storeId, toAddUserName);
                await store.AddStaffMember(toAddUserName, role, appoint.UserName);
                store.SubscribeStaffMember(appoint, appointe);
            }
            else
                throw new Exception("Store doesn't exist!");
        }

        public async Task<Store> GetStore(int storeId){
            return await _storeRepository.GetById(storeId);
        }

        public async Task<int> GetMemberIDrByUserName(string userName)
        {
            return await _clientManager.GetMemberIDrByUserName(userName); 
        }

        public async Task<Member> GetMember(string userName)
        {
            return await _clientManager.GetMember(userName); 
        }

        public async Task AddKeyWord(string keyWord, int storeId, int productId)
        {
            Store store = await _storeRepository.GetById(storeId);
            if (store != null){
                store.AddKeyword(productId, keyWord);
            }
            else
                throw new Exception("Store doesn't exist!");
        }

        // policies ------------------------------------------------
        public async Task RemovePolicy(string identifier, int storeId, int policyID,string type)
        {
            _clientManager.CheckMemberIsLoggedIn(identifier);
            Store store = await _storeRepository.GetById(storeId);
            if (store != null){
                Member activeMember = await _clientManager.GetMemberByIdentifier(identifier);                
                store.RemovePolicy(activeMember.UserName, policyID, type);
            }
            else
                throw new Exception("Store doesn't exist!");
        }
        public async Task<int> AddSimpleRule(string identifier, int storeId,string subject)
        {
            _clientManager.CheckMemberIsLoggedIn(identifier);
            Store store = await _storeRepository.GetById(storeId);
            if (store != null){
                Member activeMember = await _clientManager.GetMemberByIdentifier(identifier);                
                return await store.AddSimpleRule(activeMember.UserName, subject);
            }
            else
                throw new Exception("Store doesn't exist!");
        }
        public async Task<int> AddQuantityRule(string identifier, int storeId, string subject, int minQuantity, int maxQuantity)
        {
            _clientManager.CheckMemberIsLoggedIn(identifier);
            Store store = await _storeRepository.GetById(storeId);
            if (store != null){
                Member activeMember = await _clientManager.GetMemberByIdentifier(identifier);                
                return await store.AddQuantityRule(activeMember.UserName, subject, minQuantity, maxQuantity);
            }
            else
                throw new Exception("Store doesn't exist!");
        }
        public async Task<int> AddTotalPriceRule(string identifier, int storeId, string subject, int targetPrice)
        {
            _clientManager.CheckMemberIsLoggedIn(identifier);
            Store store = await _storeRepository.GetById(storeId);
            if (store != null){
                Member activeMember = await _clientManager.GetMemberByIdentifier(identifier);                
                return await store.AddTotalPriceRule(activeMember.UserName, subject, targetPrice);
            }
            else
                throw new Exception("Store doesn't exist!");
        }
        public async Task<int> AddCompositeRule(string identifier, int storeId, int Operator, List<int> rules)
        {
            _clientManager.CheckMemberIsLoggedIn(identifier);
            Store store = await _storeRepository.GetById(storeId);
            if (store != null){
                Member activeMember = await _clientManager.GetMemberByIdentifier(identifier);                
                LogicalOperator op = (LogicalOperator)Enum.ToObject(typeof(LogicalOperator), Operator);
                return await store.AddCompositeRule(activeMember.UserName, op, rules);
            }
            else
                throw new Exception("Store doesn't exist!");
        }
        public async Task UpdateRuleSubject(string identifier, int storeId, int ruleId, string subject)
        {
            _clientManager.CheckMemberIsLoggedIn(identifier);
            Store store = await _storeRepository.GetById(storeId);
            if (store != null){
                Member activeMember = await _clientManager.GetMemberByIdentifier(identifier);                
                store.UpdateRuleSubject(activeMember.UserName, ruleId, subject);
            }
            else
                throw new Exception("Store doesn't exist!");
        }
        public async Task UpdateRuleQuantity(string identifier, int storeId, int ruleId, int minQuantity, int maxQuantity)
        {
            _clientManager.CheckMemberIsLoggedIn(identifier);
            Store store = await _storeRepository.GetById(storeId);
            if (store != null){
                Member activeMember = await _clientManager.GetMemberByIdentifier(identifier);                
                store.UpdateRuleQuantity(activeMember.UserName, ruleId, minQuantity, maxQuantity);
            }
            else
                throw new Exception("Store doesn't exist!");
        }
        public async Task UpdateRuleTargetPrice(string identifier, int storeId, int ruleId, int targetPrice)
        {
            _clientManager.CheckMemberIsLoggedIn(identifier);
            Store store = await _storeRepository.GetById(storeId);
            if (store != null){
                Member activeMember = await _clientManager.GetMemberByIdentifier(identifier);                
                store.UpdateRuleTargetPrice(activeMember.UserName, ruleId, targetPrice);
            }
            else
                throw new Exception("Store doesn't exist!");
        }
        public async Task UpdateCompositeOperator(string identifier, int storeId, int ruleId, int Operator)
        {
            _clientManager.CheckMemberIsLoggedIn(identifier);
            Store store = await _storeRepository.GetById(storeId);
            if (store != null){
                Member activeMember = await _clientManager.GetMemberByIdentifier(identifier);                
                LogicalOperator op = (LogicalOperator)Enum.ToObject(typeof(LogicalOperator), Operator);
                store.UpdateCompositeOperator(activeMember.UserName, ruleId, op);
            }
            else
                throw new Exception("Store doesn't exist!");
        }
        public async Task UpdateCompositeRules(string identifier, int storeId, int ruleId, List<int> rules)
        {
            _clientManager.CheckMemberIsLoggedIn(identifier);
            Store store = await _storeRepository.GetById(storeId);
            if (store != null){
                Member activeMember = await _clientManager.GetMemberByIdentifier(identifier);                
                store.UpdateCompositeRules(activeMember.UserName, ruleId, rules);
            }
            else
                throw new Exception("Store doesn't exist!");
        }

        public async Task<int> AddPurchasePolicy(string identifier, int storeId, DateTime expirationDate, string subject, int ruleId)
        {
            _clientManager.CheckMemberIsLoggedIn(identifier);
            Store store = await _storeRepository.GetById(storeId);
            if (store != null){
                Member activeMember = await _clientManager.GetMemberByIdentifier(identifier);                
                return await store.AddPurchasePolicy(activeMember.UserName, expirationDate, subject, ruleId);
            }
            else
                throw new Exception("Store doesn't exist!");
        }
        public async Task<int> AddDiscountPolicy(string identifier, int storeId, DateTime expirationDate, string subject, int ruleId, double precentage)
        {
            _clientManager.CheckMemberIsLoggedIn(identifier);
            Store store = await _storeRepository.GetById(storeId);
            if (store != null){
                Member activeMember = await _clientManager.GetMemberByIdentifier(identifier);                
                return store.AddDiscountPolicy(activeMember.UserName, expirationDate, subject, ruleId, precentage);
            }
            else
                throw new Exception("Store doesn't exist!");
        }
        public async Task<int> AddCompositePolicy(string identifier, int storeId, DateTime expirationDate, string subject, int Operator, List<int> policies)
        {
            _clientManager.CheckMemberIsLoggedIn(identifier);
            Store store = await _storeRepository.GetById(storeId);
            if (store != null){
                Member activeMember = await _clientManager.GetMemberByIdentifier(identifier);                
                NumericOperator op = (NumericOperator)Enum.ToObject(typeof(NumericOperator), Operator);
                return store.AddCompositePolicy(activeMember.UserName, expirationDate, subject, op, policies);
            }
            else
                throw new Exception("Store doesn't exist!");
        }

        public async Task NotificationOn(string identifier){
            _clientManager.CheckMemberIsLoggedIn(identifier);
            await _clientManager.NotificationOn(identifier);
        }

        public async Task NotificationOff(string identifier){
            _clientManager.CheckMemberIsLoggedIn(identifier);
            await _clientManager.NotificationOff(identifier);
        }

        public async Task<List<Store>> GetMemberStores(string identifier)
        {
            var member = await _clientManager.GetMemberByIdentifier(identifier);

            return (await _storeRepository.getAll())
                .Where(store => store.roles.Values.Any(role => role.userName == member.UserName))
                .ToList(); 
        }

        public async Task<Store> GetMemberStore(string identifier, int storeId)
        {
            return (await GetMemberStores(identifier)).Where(store => store.StoreId == storeId).FirstOrDefault();
        }

        public async Task<List<Store>> GetStores()
        {
            return (await _storeRepository.getAll()).ToList();
        }

        public async Task<List<Message>> GetMemberNotifications(string identifier)
        {
            var member = await _clientManager.GetMemberByIdentifier(identifier);
            return member.alerts.ToList();
        }

        public void SetMemberNotifications(string identifier, bool on)
        {
            _clientManager.SetMemberNotifications( identifier,  on);
            
        }

        public string GetTokenByUserName(string userName)
        {
            return _clientManager.GetTokenByUserName(userName);
        }
        // ---------------------------------------------------------

    }
}
