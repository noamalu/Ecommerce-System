using MarketBackend.Services;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Payment;

namespace MarketBackend.Tests.AT
{
    public class Proxy
    {
        MarketService marketService;
        ClientService clientService;

        int userId = 1;

        public Proxy(){
            marketService = MarketService.GetInstance();
            clientService = ClientService.GetInstance();
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
        public bool OpenStore(int storeId){
            Response res = marketService.OpenStore(storeId);
            return !res.ErrorOccured;
        }

        public bool CloseStore(int storeId){
            Response res = marketService.CloseStore(storeId);
            return !res.ErrorOccured;
        }

        public bool GetInfo(int storeId){
            Response res = marketService.GetInfo(storeId);
            return !res.ErrorOccured;
        }

        public bool AddProduct(int productId, string productName, int storeId, string category, double price, int quantity, double discount)
        {
            Response res = marketService.AddProduct(productId, productName, storeId, category, price, quantity, discount);
            return !res.ErrorOccured;
        }

        public bool RemoveProduct(int productId)
        {
            Response res = marketService.RemoveProduct(productId);
            return !res.ErrorOccured;
        }

        public bool AddToCart(int clientId, int storeId, int productId, int quantity){
            Response res = clientService.AddToCart(clientId, storeId, productId, quantity);
            return !res.ErrorOccured;
        }

        public bool RemoveFromCart(int clientId, int productId){
            Response res = clientService.RemoveFromCart(clientId, productId);
            return !res.ErrorOccured;
        }

        public bool PurchaseCart(int clientId, PaymentDetails paymentDetails){
            Response res = clientService.PurchaseCart(clientId, paymentDetails);
            return !res.ErrorOccured;
        }

        public bool UpdateProductDiscount(int productId, double discount)
        {
            Response res = marketService.UpdateProductDiscount(productId, discount);
            return !res.ErrorOccured;
        }

        public bool UpdateProductPrice(int productId, double price)
        {
            Response res = marketService.UpdateProductPrice(productId, price);
            return !res.ErrorOccured;        
        }

        public bool UpdateProductQuantity(int productId, int quantity)
        {
            Response res = marketService.UpdateProductQuantity(productId, quantity);
            return !res.ErrorOccured;
        }

        public bool GetPurchaseHistory(int id)
        {
            Response res = clientService.GetPurchaseHistory(id);
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


    }
}