using System.IO.Compression;
using MarketBackend.Domain.Payment;
using MarketBackend.Domain.Shipping;
using MarketBackend.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MarketBackend.Tests.AT
{
    [TestClass()]
    public class UserMemberAT
    {
        string userName = "user1";
        string session1 = "1";
        string token1;
        string userName2 = "user2";
        string session2 = "2";
        string token2;
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
        Mock<IShippingSystemFacade> mockShippingSystem;
        Mock<IPaymentSystemFacade> mockPaymentSystem;

        [TestInitialize()]
        public void Setup(){
            proxy = new Proxy();
            userId = proxy.GetUserId();
            mockShippingSystem = new Mock<IShippingSystemFacade>();
            mockPaymentSystem = new Mock<IPaymentSystemFacade>();
            mockPaymentSystem.Setup(pay =>pay.Connect()).Returns(true);
            mockShippingSystem.Setup(ship => ship.Connect()).Returns(true);
            mockPaymentSystem.Setup(pay =>pay.Pay(It.IsAny<PaymentDetails>(), It.IsAny<double>())).Returns(1);
            mockShippingSystem.Setup(ship =>ship.OrderShippment(It.IsAny<ShippingDetails>())).Returns(1);
            mockPaymentSystem.Setup(pay =>pay.Pay(It.IsAny<PaymentDetails>(), It.IsAny<double>())).Returns(1);
            mockShippingSystem.Setup(ship =>ship.OrderShippment(It.IsAny<ShippingDetails>())).Returns(1);
            mockShippingSystem.SetReturnsDefault(true);
            mockPaymentSystem.SetReturnsDefault(true);
            proxy.InitiateSystemAdmin();
            proxy.EnterAsGuest(session1);
            proxy.Register(userName, userPassword, email1, userAge);
            token1 = proxy.LoginWithToken(userName, userPassword);
            userId = proxy.GetMembeIDrByUserName(userName);
            int userId2 = proxy.GetUserId();
            proxy.EnterAsGuest(session2);
            proxy.Register(userName2, pass2, email2, userAge);
            token2 = proxy.LoginWithToken(userName2, pass2);
            userId2 = proxy.GetMembeIDrByUserName(userName2);
        }

        [TestCleanup]
        public void CleanUp(){
            proxy.Dispose();
        }


        [TestMethod]
        public void LogOutSuccess(){
            Assert.IsTrue(proxy.LogOut(token1));
        }

        [TestMethod]
        public void LogOutFail_NotLoggedIn(){
            proxy.LogOut(token1);
            Assert.IsFalse(proxy.LogOut(token1));
        }

        [TestMethod]
        public void GetInfo(){
            //get info?
        }

        [TestMethod]
        public void CreateShopSuccess()
        {
           Assert.IsTrue(proxy.CreateStore(token1, storeName, storeEmail, phoneNum));
        }

        [TestMethod]
        public void CreateShopFail_NotLoggedIn()
        {
           proxy.LogOut(token1);
           Assert.IsFalse(proxy.CreateStore(token1, storeName, storeEmail, phoneNum));
        }

        [TestMethod]
        public void GetInfoSuccess()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(token1, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.GetInfo(shopID));
        }

        [TestMethod]
        public void SearchByKeyWords(){
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(token1, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false));
           Assert.IsTrue(proxy.AddKeyWord(token1, "nice", shopID, 11));
           Assert.IsTrue(proxy.SearchByKeywords("nice"));
        }

        [TestMethod]
        public void AddToCartSuccess()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(token1, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false));
           userId2 = proxy.GetMembeIDrByUserName(userName2);
           Assert.IsTrue(proxy.AddToCart(token2, shopID, productID1, quantity1));
        }

        [TestMethod]
        public void AddToCartFail_NoProduct()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(token1, storeName, storeEmail, phoneNum));
           userId2 = proxy.GetMembeIDrByUserName(userName2);
           Assert.IsFalse(proxy.AddToCart(token2, shopID, productID1, quantity1));
        }

        [TestMethod]
        public void RemoveFromCartSuccess()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(token1, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false));
           userId2 = proxy.GetMembeIDrByUserName(userName2);
           Assert.IsTrue(proxy.AddToCart(token2, shopID, productID1, 1));
           Assert.IsTrue(proxy.RemoveFromCart(token2, productID1, basketId, 1));
        }

        [TestMethod]
        public void RemoveFromCartFail_NoProduct()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(token1, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false));
           userId2 = proxy.GetMembeIDrByUserName(userName2);
           Assert.IsFalse(proxy.RemoveFromCart(token2, productID1, basketId, 1));
        }

        [TestMethod]
        public void PurchaseCartSuccess()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(token1, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false));
           userId2 = proxy.GetMembeIDrByUserName(userName2);
           Assert.IsTrue(proxy.AddToCart(token2, shopID, productID1, quantity1));
           PaymentDetails paymentDetails = new PaymentDetails("5326888878675678", "2027", "10", "101", "3190876789", "Hadas");
           ShippingDetails shippingDetails = new ShippingDetails("name",  "city",  "address",  "country",  "zipcode");
           Assert.IsTrue(proxy.PurchaseCart(token2, paymentDetails, shippingDetails));
        }

        [TestMethod]
        public void PurchaseCartFail_NoProduct()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(token1, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false));
           userId2 = proxy.GetMembeIDrByUserName(userName2);
           Assert.IsTrue(proxy.AddToCart(token2, shopID, productID1, quantity1));
           PaymentDetails paymentDetails = new PaymentDetails("5326888878675678", "2027", "10", "101", "3190876789", "Hadas");
           ShippingDetails shippingDetails = new ShippingDetails("name",  "city",  "address",  "country",  "zipcode");
           Assert.IsTrue(proxy.RemoveProduct(shopID, token1, 11));
           Assert.IsFalse(proxy.PurchaseCart(token2, paymentDetails, shippingDetails));
        }

        [TestMethod]
        public void PurchaseCartFail_EmptyCart()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(token1, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false));
           userId2 = proxy.GetMembeIDrByUserName(userName2);
           Assert.IsFalse(proxy.PurchaseCart(token2, paymentDetails, shippingDetails));
        }

        [TestMethod]
        public void PurchaseCartFail_IlegalAge()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(token1, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, true));
           userId2 = proxy.GetMembeIDrByUserName(userName2);
           Assert.IsFalse(proxy.PurchaseCart(token2, paymentDetails, shippingDetails));
        }

        [TestMethod]
        public void PurchaseCartFail_Payment(){
            int shopID = 1;
            Assert.IsTrue(proxy.CreateStore(token1, storeName, storeEmail, phoneNum));
            Assert.IsTrue(proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false));
            userId2 = proxy.GetMembeIDrByUserName(userName2);
            mockPaymentSystem.SetReturnsDefault(false);
            Assert.IsFalse(proxy.PurchaseCart(token2, paymentDetails, shippingDetails));
        }

        [TestMethod]
        public void PurchaseCartFail_Shipping(){
            int shopID = 1;
            Assert.IsTrue(proxy.CreateStore(token1, storeName, storeEmail, phoneNum));
            Assert.IsTrue(proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false));
            userId2 = proxy.GetMembeIDrByUserName(userName2);
            mockShippingSystem.SetReturnsDefault(false);
            Assert.IsFalse(proxy.PurchaseCart(token2, paymentDetails, shippingDetails));
        }

        [TestMethod]
        public void GetPurchaseHistorySuccess_Permission()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(token1, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false));
           userId2 = proxy.GetMembeIDrByUserName(userName2);
           Assert.IsTrue(proxy.AddToCart(token2, shopID, productID1, quantity1));
           Assert.IsTrue(proxy.PurchaseCart(token2, paymentDetails, shippingDetails));
           Assert.IsTrue(proxy.AddOwner(token1, 1, userName2));
           Assert.IsTrue(proxy.GetPurchaseHistory(shopID, token2));
        }

        [TestMethod]
        public void GetPurchaseHistoryFail_Permission()
        {
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(token1, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false));
           userId2 = proxy.GetMembeIDrByUserName(userName2);
           Assert.IsTrue(proxy.AddToCart(token2, shopID, productID1, quantity1));
           Assert.IsTrue(proxy.PurchaseCart(token2, paymentDetails, shippingDetails));
           Assert.IsFalse(proxy.GetPurchaseHistory(shopID, token2));
        }




    }
}