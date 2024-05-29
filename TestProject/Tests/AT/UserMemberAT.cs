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
        int productID1 = 11;
        string productName1 = "Banana";
        string category1 = "Fruit";
        double price1 = 5.0;
        int quantity1 = 10;
        double discount1 = 0.5; 
        int userAge = 20;
        int userAge2 = 16;
        int basketId = 1;
        PaymentDetails paymentDetails = new PaymentDetails("5326888878675678", "2027", "10", "101", "3190876789", "Hadas");
        ShippingDetails shippingDetails = new ShippingDetails("name",  "city",  "address",  "country",  "zipcode");

        string storeName = "Rami Levi";
        string storeEmail = "RamiLevi@gmail.com";
        string phoneNum  = "0522458976";
        string sellmethod = "RegularSell";
        string desc = "nice";
        int userId2;

        [TestInitialize()]
        public void Setup(){
            proxy = new Proxy();
            userId = proxy.GetUserId();
            var mockShippingSystem = new Mock<IShippingSystemFacade>();
            var mockPaymentSystem = new Mock<IPaymentSystemFacade>();
            mockPaymentSystem.Setup(pay =>pay.Connect()).Returns(true);
            mockShippingSystem.Setup(ship => ship.Connect()).Returns(true);
            mockShippingSystem.SetReturnsDefault(true);
            mockPaymentSystem.SetReturnsDefault(true);
            proxy.InitiateSystemAdmin();
            proxy.EnterAsGuest(userId);
            proxy.Register(userId, userName, userPassword, email1, userAge);
            proxy.Login(userId, userName, userPassword);
            userId = proxy.GetMembeIDrByUserName(userName);
            int userId2 = proxy.GetUserId();
            proxy.EnterAsGuest(userId2);
            proxy.Register(userId2, userName2, pass2, email2, userAge);
            proxy.Login(userId2, userName2, pass2);
            userId2 = proxy.GetMembeIDrByUserName(userName2);
        }

        [TestCleanup]
        public void CleanUp(){
            proxy.Dispose();
        }


        [TestMethod]
        public void LogOutSuccess(){
            Assert.IsTrue(proxy.LogOut(userId));
        }

        [TestMethod]
        public void LogOutFail_NotLoggedIn(){
            proxy.LogOut(userId);
            Assert.IsFalse(proxy.LogOut(userId));
        }

        [TestMethod]
        public void GetInfo(){
            //get info?
        }

        [TestMethod]
        public void CreateShopSuccess()
        {
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
        }

        [TestMethod]
        public void CreateShopFail_NotLoggedIn()
        {
           proxy.LogOut(userId);
           Assert.IsFalse(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
        }

        [TestMethod]
        public void GetInfoSuccess()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.GetInfo(shopID));
        }

        [TestMethod]
        public void SearchByKeyWords(){
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(shopID, userId, productName1, sellmethod, desc, price1, category1, quantity1, false));
           Assert.IsTrue(proxy.AddKeyWord(userId, "nice", shopID, 11));
           Assert.IsTrue(proxy.SearchByKeywords("nice"));
        }

        [TestMethod]
        public void AddToCartSuccess()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(shopID, userId, productName1, sellmethod, desc, price1, category1, quantity1, false));
           userId2 = proxy.GetMembeIDrByUserName(userName2);
           Assert.IsTrue(proxy.AddToCart(userId2, shopID, productID1, quantity1));
        }

        [TestMethod]
        public void AddToCartFail_NoProduct()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           userId2 = proxy.GetMembeIDrByUserName(userName2);
           Assert.IsFalse(proxy.AddToCart(userId2, shopID, productID1, quantity1));
        }

        [TestMethod]
        public void RemoveFromCartSuccess()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(shopID, userId, productName1, sellmethod, desc, price1, category1, quantity1, false));
           userId2 = proxy.GetMembeIDrByUserName(userName2);
           Assert.IsTrue(proxy.AddToCart(userId2, shopID, productID1, 1));
           Assert.IsTrue(proxy.RemoveFromCart(userId2, productID1, basketId, 1));
        }

        [TestMethod]
        public void RemoveFromCartFail_NoProduct()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(shopID, userId, productName1, sellmethod, desc, price1, category1, quantity1, false));
           userId2 = proxy.GetMembeIDrByUserName(userName2);
           Assert.IsFalse(proxy.RemoveFromCart(userId2, productID1, basketId, 1));
        }

        [TestMethod]
        public void PurchaseCartSuccess()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(shopID, userId, productName1, sellmethod, desc, price1, category1, quantity1, false));
           userId2 = proxy.GetMembeIDrByUserName(userName2);
           Assert.IsTrue(proxy.AddToCart(userId2, shopID, productID1, quantity1));
           PaymentDetails paymentDetails = new PaymentDetails("5326888878675678", "2027", "10", "101", "3190876789", "Hadas");
           ShippingDetails shippingDetails = new ShippingDetails("name",  "city",  "address",  "country",  "zipcode");
           Assert.IsTrue(proxy.PurchaseCart(userId2, paymentDetails, shippingDetails));
        }

        [TestMethod]
        public void PurchaseCartFail_NoProduct()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(shopID, userId, productName1, sellmethod, desc, price1, category1, quantity1, false));
           userId2 = proxy.GetMembeIDrByUserName(userName2);
           Assert.IsTrue(proxy.AddToCart(userId2, shopID, productID1, quantity1));
           PaymentDetails paymentDetails = new PaymentDetails("5326888878675678", "2027", "10", "101", "3190876789", "Hadas");
           ShippingDetails shippingDetails = new ShippingDetails("name",  "city",  "address",  "country",  "zipcode");
           Assert.IsTrue(proxy.RemoveProduct(shopID, userId, 11));
           Assert.IsFalse(proxy.PurchaseCart(userId2, paymentDetails, shippingDetails));
        }

        [TestMethod]
        public void PurchaseCartFail_EmptyCart()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(shopID, userId, productName1, sellmethod, desc, price1, category1, quantity1, false));
           userId2 = proxy.GetMembeIDrByUserName(userName2);
           Assert.IsFalse(proxy.PurchaseCart(userId2, paymentDetails, shippingDetails));
        }

        [TestMethod]
        public void PurchaseCartFail_IlegalAge()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(shopID, userId, productName1, sellmethod, desc, price1, category1, quantity1, true));
           userId2 = proxy.GetMembeIDrByUserName(userName2);
           Assert.IsFalse(proxy.PurchaseCart(userId2, paymentDetails, shippingDetails));
        }

        [TestMethod]
        public void GetPurchaseHistorySuccess_Permission()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(shopID, userId, productName1, sellmethod, desc, price1, category1, quantity1, false));
           userId2 = proxy.GetMembeIDrByUserName(userName2);
           Assert.IsTrue(proxy.AddToCart(userId2, shopID, productID1, quantity1));
           Assert.IsTrue(proxy.PurchaseCart(userId2, paymentDetails, shippingDetails));
           Assert.IsTrue(proxy.AddOwner(userId, 1, userId2));
           Assert.IsTrue(proxy.GetPurchaseHistory(shopID, userId2));
        }

        [TestMethod]
        public void GetPurchaseHistoryFail_Permission()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(shopID, userId, productName1, sellmethod, desc, price1, category1, quantity1, false));
           userId2 = proxy.GetMembeIDrByUserName(userName2);
           Assert.IsTrue(proxy.AddToCart(userId2, shopID, productID1, quantity1));
           Assert.IsTrue(proxy.PurchaseCart(userId2, paymentDetails, shippingDetails));
           Assert.IsFalse(proxy.GetPurchaseHistory(shopID, userId2));
        }




    }
}