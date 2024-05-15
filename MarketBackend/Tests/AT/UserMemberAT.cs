using System.IO.Compression;
using MarketBackend.Domain.Payment;
using MarketBackend.Domain.Shipping;
using MarketBackend.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MarketBackend.Tests.AT
{
    [TestClass()]
    public class UserMemberAT
    {
        string userName = "user1";
        string userName2 = "user2";
        string userPassword = "pass1";
        string pass2 = "pass2";
        string email1 = "printz@post.bgu.ac.il";
        string email2 = "hadaspr100@gmail.com";
        string wrongEmail = "@gmail.com";
        int userId;
        Proxy proxy;
        int productID1 = 111;
        string productName1 = "Banana";
        string category1 = "Fruit";
        double price1 = 5.0;
        int quantity1 = 10;
        double discount1 = 0.5; 
        int userAge = 20;
        int userAge2 = 16;


        [TestInitialize()]
        public void Setup(){
            proxy = new Proxy();
            userId = proxy.GetUserId();
            var mockShippingSystem = new Mock<IShippingSystemFacade>();
            var mockPaymentSystem = new Mock<IPaymentSystemFacade>();
            mockShippingSystem.SetReturnsDefault(true);
            mockPaymentSystem.SetReturnsDefault(true);
        }

        [TestCleanup]
        public void CleanUp(){
            proxy.Dispose();
        }

        [TestMethod]
        public void EnterAsGuestSuccess(){
            Assert.IsFalse(proxy.Login(userId, userName, userPassword));
        }

        [TestMethod]
        public void RegisterSuccess(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
        }

        [TestMethod]
        public void RegisterFail_RegisterTwice(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
            Assert.IsFalse(proxy.Register(userId, userName, userPassword, email1, userAge));
        }

        [TestMethod]
        public void LoginSuccess(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
            Assert.IsTrue(proxy.Login(userId, userName, userPassword));
        }

        [TestMethod]
        public void LoginFail_NotRegister(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsFalse(proxy.Login(userId, userName, userPassword));
        }

        [TestMethod]
        public void LoginFail_WrongUserName(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
            Assert.IsFalse(proxy.Login(userId, userName2, userPassword));
        }

        [TestMethod]
        public void LoginFail_WrongPassword(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
            Assert.IsFalse(proxy.Login(userId, userName, pass2));
        }

        [TestMethod]
        public void LogOutSuccess(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
            Assert.IsFalse(proxy.Login(userId, userName, userPassword));
            Assert.IsTrue(proxy.LogOut(userId));
        }

        [TestMethod]
        public void LogOutFail_NotLoggedIn(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
            Assert.IsFalse(proxy.LogOut(userId));
        }

        [TestMethod]
        public void GetInfo(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
            Assert.IsTrue(proxy.Login(userId, userName, userPassword));
            //get info?
        }

        [TestMethod]
        public void CreateShopSuccess()
        {
           Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.OpenStore(shopID));
        }

        [TestMethod]
        public void CreateShopFail_NotLoggedIn()
        {
           Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           int shopID = 1;
           Assert.IsFalse(proxy.OpenStore(shopID));
        }

        [TestMethod]
        public void GetInfoSuccess()
        {
           Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.OpenStore(shopID));
           Assert.IsTrue(proxy.GetInfo(shopID));
        }

        [TestMethod]
        public void GetInfoFail_NotLoggedIn()
        {
           Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.OpenStore(shopID));
           Assert.IsTrue(proxy.LogOut(userId));
           Assert.IsFalse(proxy.GetInfo(shopID));
        }

        [TestMethod]
        public void SearchByKeyWords(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));

        }

        [TestMethod]
        public void AddToCartSuccess()
        {
           Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.OpenStore(shopID));
           Assert.IsTrue(proxy.AddProduct(productID1, productName1, shopID, category1, price1, quantity1, discount1));
           int userId2 = proxy.GetUserId();
           Assert.IsTrue(proxy.EnterAsGuest(userId2));
           Assert.IsTrue(proxy.Register(userId2, userName2, pass2, email2, userAge));
           Assert.IsTrue(proxy.Login(userId2, userName2, pass2));
           Assert.IsTrue(proxy.AddToCart(userId2, productID1));
        }

        [TestMethod]
        public void AddToCartFail_NoProduct()
        {
           Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.OpenStore(shopID));
           int userId2 = proxy.GetUserId();
           Assert.IsTrue(proxy.EnterAsGuest(userId2));
           Assert.IsTrue(proxy.Register(userId2, userName2, pass2, email2, userAge));
           Assert.IsTrue(proxy.Login(userId2, userName2, pass2));
           Assert.IsFalse(proxy.AddToCart(userId2, productID1));
        }

        [TestMethod]
        public void RemoveFromCartSuccess()
        {
           Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.OpenStore(shopID));
           Assert.IsTrue(proxy.AddProduct(productID1, productName1, shopID, category1, price1, quantity1, discount1));
           int userId2 = proxy.GetUserId();
           Assert.IsTrue(proxy.EnterAsGuest(userId2));
           Assert.IsTrue(proxy.Register(userId2, userName2, pass2, email2, userAge));
           Assert.IsTrue(proxy.Login(userId2, userName2, pass2));
           Assert.IsTrue(proxy.AddToCart(userId2, productID1));
           Assert.IsTrue(proxy.RemoveFromCart(userId2, productID1));
        }

        [TestMethod]
        public void RemoveFromCartFail_NoProduct()
        {
           Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.OpenStore(shopID));
           Assert.IsTrue(proxy.AddProduct(productID1, productName1, shopID, category1, price1, quantity1, discount1));
           int userId2 = proxy.GetUserId();
           Assert.IsTrue(proxy.EnterAsGuest(userId2));
           Assert.IsTrue(proxy.Register(userId2, userName2, pass2, email2, userAge));
           Assert.IsTrue(proxy.Login(userId2, userName2, pass2));
           Assert.IsFalse(proxy.RemoveFromCart(userId2, productID1));
        }

        [TestMethod]
        public void PurchaseCartSuccess()
        {
           Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.OpenStore(shopID));
           Assert.IsTrue(proxy.AddProduct(productID1, productName1, shopID, category1, price1, quantity1, discount1));
           int userId2 = proxy.GetUserId();
           Assert.IsTrue(proxy.EnterAsGuest(userId2));
           Assert.IsTrue(proxy.Register(userId2, userName2, pass2, email2, userAge));
           Assert.IsTrue(proxy.Login(userId2, userName2, pass2));
           Assert.IsTrue(proxy.AddToCart(userId2, productID1));
           Assert.IsTrue(proxy.PurchaseCart(userId2));
        }

        [TestMethod]
        public void PurchaseCartFail_EmptyCart()
        {
           Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.OpenStore(shopID));
           Assert.IsTrue(proxy.AddProduct(productID1, productName1, shopID, category1, price1, quantity1, discount1));
           int userId2 = proxy.GetUserId();
           Assert.IsTrue(proxy.EnterAsGuest(userId2));
           Assert.IsTrue(proxy.Register(userId2, userName2, pass2, email2, userAge));
           Assert.IsTrue(proxy.Login(userId2, userName2, pass2));
           Assert.IsFalse(proxy.PurchaseCart(userId2));
        }

        [TestMethod]
        public void PurchaseCartFail_IlegalAge()
        {
           Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.OpenStore(shopID));
           //add product as ilegal
           Assert.IsTrue(proxy.AddProduct(productID1, productName1, shopID, category1, price1, quantity1, discount1));
           int userId2 = proxy.GetUserId();
           Assert.IsTrue(proxy.EnterAsGuest(userId2));
           Assert.IsTrue(proxy.Register(userId2, userName2, pass2, email2, userAge2));
           Assert.IsTrue(proxy.Login(userId2, userName2, pass2));
           Assert.IsFalse(proxy.PurchaseCart(userId2));
        }

        [TestMethod]
        public void GetPurchaseHistoryFail_Permission()
        {
           Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.OpenStore(shopID));
           Assert.IsTrue(proxy.AddProduct(productID1, productName1, shopID, category1, price1, quantity1, discount1));
           int userId2 = proxy.GetUserId();
           Assert.IsTrue(proxy.EnterAsGuest(userId2));
           Assert.IsTrue(proxy.Register(userId2, userName2, pass2, email2, userAge));
           Assert.IsTrue(proxy.Login(userId2, userName2, pass2));
           Assert.IsTrue(proxy.AddToCart(userId2, productID1));
           Assert.IsTrue(proxy.PurchaseCart(userId2));
           Assert.IsFalse(proxy.GetPurchaseHistory(userId2));
        }

        //view cart




    }
}