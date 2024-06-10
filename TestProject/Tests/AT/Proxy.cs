using MarketBackend.Services;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Payment;
using NLog;
using MarketBackend.Domain.Shipping;
using Moq;

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

        public bool Login(int userId, string userName, string password){
            Response res = clientService.LoginClient(userId, userName, password);
            return !res.ErrorOccured;
        }

        public bool EnterAsGuest(int userId){
            Response res = clientService.EnterAsGuest(userId);
            return !res.ErrorOccured;
        }

        public bool Register(int id, string userName, string password, string email, int age){
            Response res = clientService.Register(id, userName, password, email, age);
            return !res.ErrorOccured;
        }

        public bool LogOut(int userId){
            Response res = clientService.LogoutClient(userId);
            return !res.ErrorOccured;
        }

        public bool SearchByKeywords(string keywords){
            Response res = marketService.SearchByKeywords(keywords);
            return !res.ErrorOccured;
        }
        public bool SearchByName(string name){
            Response res = marketService.SearchByName(name);
            return !res.ErrorOccured;
        }
        public bool SearchByCategory(string category){
            Response res = marketService.SearchByCategory(category);
            return !res.ErrorOccured;
        }
        public bool OpenStore(int clientId, int storeId){
            Response res = marketService.OpenStore(clientId, storeId);
            return !res.ErrorOccured;
        }

        public bool CloseStore(int clientId, int storeId){
            Response res = marketService.CloseStore(clientId, storeId);
            return !res.ErrorOccured;
        }

        public bool GetInfo(int storeId){
            Response res = marketService.GetInfo(storeId);
            return !res.ErrorOccured;
        }

        public bool AddProduct(int storeId, int userId, string name, string sellMethod, string description, double price, string category, int quantity, bool ageLimit)
        {
            Response res = marketService.AddProduct(storeId, userId, name, sellMethod, description, price, category, quantity, ageLimit);
            return !res.ErrorOccured;
        }

        public bool RemoveProduct(int storeId,int userId, int productId)
        {
            Response res = marketService.RemoveProduct(storeId, userId, productId);
            return !res.ErrorOccured;
        }

        public bool AddToCart(int clientId, int storeId, int productId, int quantity){
            Response res = clientService.AddToCart(clientId, storeId, productId, quantity);
            return !res.ErrorOccured;
        }

        public bool RemoveFromCart(int clientId, int productId, int basketId, int quantity){
            Response res = clientService.RemoveFromCart(clientId, productId, basketId, quantity);
            return !res.ErrorOccured;
        }

        public bool PurchaseCart(int clientId, PaymentDetails paymentDetails, ShippingDetails shippingDetails){
            Response res = marketService.PurchaseCart(clientId, paymentDetails, shippingDetails);
            return !res.ErrorOccured;
        }

        // public bool UpdateProductDiscount(int productId, double discount) 
        // {
        //     Response res = marketService.UpdateProductDiscount(productId, discount);
        //     return !res.ErrorOccured;
        // }

        public bool UpdateProductPrice(int storeId, int userId, int productId, double price)
        {
            Response res = marketService.UpdateProductPrice(storeId, userId, productId, price);
            return !res.ErrorOccured;        
        }

        public bool UpdateProductQuantity(int storeId, int userId, int productId, int quantity)
        {
            Response res = marketService.UpdateProductQuantity(storeId, userId, productId, quantity);
            return !res.ErrorOccured;
        }

        public bool GetPurchaseHistory(int storeId, int clientId)
        {
            Response res = marketService.GetPurchaseHistory(storeId, clientId);
            return !res.ErrorOccured;
        }

        public bool RemoveStaffMember(int storeId, int activeId, Role role, int toRemoveId)
        {
            Response res = marketService.RemoveStaffMember(storeId, activeId, role, toRemoveId);
            return !res.ErrorOccured;
        }

        public bool AddStaffMember(int storeId, int activeId, Role role, int toAddId)
        {
            Response res = marketService.AddStaffMember(storeId, activeId, role, toAddId);
            return !res.ErrorOccured;
        }

        public bool CreateStore(int id, string storeName, string email, string phoneNum){
            Response res = clientService.CreateStore(id, storeName, email, phoneNum);
            return !res.ErrorOccured;
        }
        
        public void InitiateSystemAdmin(){
            clientService.InitiateSystemAdmin();
        }

        public bool ExitGuest(int id){
            Response res = clientService.ExitGuest(id);
            return !res.ErrorOccured;
        }

        public bool AddOwner(int activeId, int storeId, int toAddId){
            Response res = marketService.AddOwner(activeId, storeId, toAddId);
            return !res.ErrorOccured;
        }

        public int GetMembeIDrByUserName(string userName){
            // Response res = 
            return clientService.GetMemberIDByUserName(userName);
            // return res.ErrorOccured ? -1 : int.Parse(res.ErrorMessage);
        }

        public bool AddKeyWord(int id, string keyWord, int storeId, int productId){
            Response res = marketService.AddKeyWord(id, keyWord, storeId, productId);
            return !res.ErrorOccured;
        }

        public bool AddSimpleRule(int clientId, int storeId,string subject){
            Response res = marketService.AddSimpleRule(clientId, storeId, subject);
            return !res.ErrorOccured;
        }

        public bool AddQuantityRule(int clientId, int storeId, string subject, int minQuantity, int maxQuantity){
            Response res = marketService.AddQuantityRule(clientId, storeId, subject, minQuantity, maxQuantity);
            return !res.ErrorOccured;
        }

        public bool AddTotalPriceRule(int clientId, int storeId, string subject, int targetPrice){
            Response res = marketService.AddTotalPriceRule(clientId, storeId, subject, targetPrice);
            return !res.ErrorOccured;
        }




    }
}