using MarketBackend.Services;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Payment;
using NLog;
using MarketBackend.Domain.Shipping;
using Moq;
using MarketBackend.Services.Models;

namespace MarketBackend.Tests.AT
{
    public class Proxy
    {
        MarketService marketService;
        ClientService clientService;

        int userId = 1;
        

        public Proxy(){
            var mockShippingSystem = new Mock<IShippingSystemFacade>();
            var mockPaymentSystem = new Mock<IPaymentSystemFacade>();
            mockPaymentSystem.Setup(pay =>pay.Connect()).Returns(true);
            mockShippingSystem.Setup(ship => ship.Connect()).Returns(true);
            mockPaymentSystem.Setup(pay =>pay.Pay(It.IsAny<PaymentDetails>(), It.IsAny<double>())).Returns(1);
            mockShippingSystem.Setup(ship =>ship.OrderShippment(It.IsAny<ShippingDetails>())).Returns(1);
            mockShippingSystem.SetReturnsDefault(true);
            mockPaymentSystem.SetReturnsDefault(true);            
            marketService = MarketService.GetInstance(mockShippingSystem.Object, mockPaymentSystem.Object);
            clientService = ClientService.GetInstance(mockShippingSystem.Object, mockPaymentSystem.Object);
            
        }

        public void Dispose(){
            marketService.Dispose();
            clientService.Dispose();
        }

        public int GetUserId(){
            string userIds = userId.ToString();
            userId = int.Parse(userIds) + 1;
            return userId;
        }

        public async Task<bool> Login(string userName, string password){
            Response res = await clientService.LoginClient(userName, password);
            return !res.ErrorOccured;
        }
        public async Task<string> LoginWithToken(string userName, string password){
            Response<string> res = await clientService.LoginClient(userName, password);
             if(res.ErrorOccured){
                throw new Exception(res.ErrorMessage);
             }
            return res.Value;
        }

        public async Task<bool> EnterAsGuest(string identifier){
            Response res = await clientService.EnterAsGuest(identifier);
            return !res.ErrorOccured;
        }

        public async Task<bool> Register(string userName, string password, string email, int age){
            Response res = await clientService.Register(userName, password, email, age);
            return !res.ErrorOccured;
        }

        public async Task<bool> LogOut(string identifier){
            Response res = await clientService.LogoutClient(identifier);
            return !res.ErrorOccured;
        }

        public async Task<bool> SearchByKeywords(string keywords){
            Response res = await marketService.SearchByKeywords(keywords);
            return !res.ErrorOccured;
        }

        public async Task<List<ProductResultDto>> SearchByKey(string key){
            Response<List<ProductResultDto>> res = await marketService.SearchByKeywords(key);
            return res.Value;
        }

        public async Task<bool> SearchByName(string name){
            Response res = await marketService.SearchByName(name);
            return !res.ErrorOccured;
        }
        public async Task<bool> SearchByCategory(string category){
            Response res = await marketService.SearchByCategory(category);
            return !res.ErrorOccured;
        }
        public async Task<bool> OpenStore(string identifier, int storeId){
            Response res = await marketService.OpenStore(identifier, storeId);
            return !res.ErrorOccured;
        }

        public async Task<bool> CloseStore(string identifier, int storeId){
            Response res = await marketService.CloseStore(identifier, storeId);
            return !res.ErrorOccured;
        }

        public async Task<bool> GetInfo(int storeId){
            Response res = await marketService.GetInfo(storeId);
            return !res.ErrorOccured;
        }

        public async Task<bool> AddProduct(int storeId, string identifier, string name, string sellMethod, string description, double price, string category, int quantity, bool ageLimit)
        {

            Response<int> task = await marketService.AddProduct(storeId, identifier, name, sellMethod, description, price, category, quantity, ageLimit);
            return !task.ErrorOccured;
        }

        public async Task<bool> RemoveProduct(int storeId,string identifier, int productId)
        {
            Response res = await marketService.RemoveProduct(storeId, identifier, productId);
            return !res.ErrorOccured;
        }

        public async Task<bool> AddToCart(string identifier, int storeId, int productId, int quantity){
            Response res = await clientService.AddToCart(identifier, storeId, productId, quantity);
            return !res.ErrorOccured;
        }

        public async Task<bool> RemoveFromCart(string identifier, int productId, int storeId, int quantity){
            Response res = await clientService.RemoveFromCart(identifier, productId, storeId, quantity);
            return !res.ErrorOccured;
        }

        public async Task<bool> PurchaseCart(string identifier, PaymentDetails paymentDetails, ShippingDetails shippingDetails){
            Response res = await marketService.PurchaseCart(identifier, paymentDetails, shippingDetails);
            return !res.ErrorOccured;
        }

        // public bool UpdateProductDiscount(int productId, double discount) 
        // {
        //     Response res = marketService.UpdateProductDiscount(productId, discount);
        //     return !res.ErrorOccured;
        // }

        public async Task<bool> UpdateProductPrice(int storeId, string identifier, int productId, double price)
        {
            Response res = await marketService.UpdateProductPrice(storeId, identifier, productId, price);
            return !res.ErrorOccured;        
        }

        public async Task<bool> UpdateProductQuantity(int storeId, string identifier, int productId, int quantity)
        {
            Response res = await marketService.UpdateProductQuantity(storeId, identifier, productId, quantity);
            return !res.ErrorOccured;
        }

        public async Task<bool> GetPurchaseHistory(int storeId, string identifier)
        {
            Response res = await marketService.GetPurchaseHistoryByStore(storeId, identifier);
            return !res.ErrorOccured;
        }

        public async Task<bool> RemoveStaffMember(int storeId, string identifier, string toRemoveUserName)
        {
            Response res = await marketService.RemoveStaffMember(storeId, identifier, toRemoveUserName);
            return !res.ErrorOccured;
        }

        public async Task<bool> AddStaffMember(int storeId, string identifier, Role role, string toAddUserName)
        {
            Response res = await marketService.AddStaffMember(storeId, identifier, role.role.roleName.GetDescription(), toAddUserName);
            return !res.ErrorOccured;
        }

        public async Task<bool> CreateStore(string identifier, string storeName, string email, string phoneNum){
            Response<int> task = await clientService.CreateStore(identifier, storeName, email, phoneNum);
            return !task.ErrorOccured;
        }
        
        public async void InitiateSystemAdmin(){
            await clientService.InitiateSystemAdmin();
        }

        public async Task<bool> ExitGuest(string identifier){
            Response res = await clientService.ExitGuest(identifier);
            return !res.ErrorOccured;
        }

        public async Task<bool> AddOwner(string identifier, int storeId, string toAddUserName){
            Response res = await marketService.AddOwner(identifier, storeId, toAddUserName);
            return !res.ErrorOccured;
        }

        public async Task<int> GetMembeIDrByUserName(string userName){
            // Response res = 
            return await clientService.GetMemberIDByUserName(userName);
            // return res.ErrorOccured ? -1 : int.Parse(res.ErrorMessage);
        }

        public async Task<bool> AddKeyWord(string identifier, string keyWord, int storeId, int productId){
            Response res = await marketService.AddKeyWord(identifier, keyWord, storeId, productId);
            return !res.ErrorOccured;
        }

        public async Task<bool> AddSimpleRule(string identifier, int storeId,string subject){
            Response res = await marketService.AddSimpleRule(identifier, storeId, subject);
            return !res.ErrorOccured;
        }

        public async Task<bool> AddQuantityRule(string identifier, int storeId, string subject, int minQuantity, int maxQuantity){
            Response res = await marketService.AddQuantityRule(identifier, storeId, subject, minQuantity, maxQuantity);
            return !res.ErrorOccured;
        }

        public async Task<bool> AddTotalPriceRule(string identifier, int storeId, string subject, int targetPrice){
            Response res = await marketService.AddTotalPriceRule(identifier, storeId, subject, targetPrice);
            return !res.ErrorOccured;
        }

        public async Task<bool> GetPurchaseHistoryByClient(string userName){
            Response res = await clientService.GetPurchaseHistoryByClient(userName);
            return !res.ErrorOccured;
        }

        public async Task<List<ShoppingCartResultDto>> GetPurchaseHistory(string userName){
            Response<List<ShoppingCartResultDto>> res = await clientService.GetPurchaseHistoryByClient(userName);
            return res.Value;
        }

        public async Task<bool> GetProductInfo(int storeId, int productId){
            Response res = await marketService.GetProductInfo(storeId, productId);
            return !res.ErrorOccured;
        }

        public async Task<Product> GetProduct(int storeId, int productId){
            Response<Product> res = await marketService.GetProduct(storeId, productId);
            return res.Value;
        }

        public async Task<List<Member>> GetOwners(int storeId){
            Response<List<Member>> res = await marketService.GetOwners(storeId);
            return res.Value;
        }

        public async Task<string> GetStoreById(int storeId){
            Response<string> res = await marketService.GetStoreById(storeId);
            return res.Value;
        }

        public async Task<Member> GetMember(string userName){
            return await clientService.GetMember(userName);
        }

        public async Task<List<RuleResultDto>> GetStoreRules(int storeId, string identifier){
            Response<List<RuleResultDto>> res = await marketService.GetStoreRules(storeId, identifier);
            return res.Value;
        }
    }
}