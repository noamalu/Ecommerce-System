using System.IO.Compression;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Models;
using MarketBackend.Domain.Payment;
using MarketBackend.Domain.Shipping;
using MarketBackend.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog;
using NuGet.Frameworks;

namespace MarketBackend.Tests.IT
{
    [TestClass()]
    public class MarketIT
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
        int productID1 = 11;
        string productName1 = "Banana";
        string category1 = "Fruit";
        string storeName = "Remi levi";
        string phoneNum = "0522768972";
        double price1 = 5.0;
        int quantity1 = 10;
        double discount1 = 0.5; 
        int userAge = 20;
        int userAge2 = 16;
        int basketId = 1;
        PaymentDetails paymentDetails = new PaymentDetails("5326888878675678", "2027", "10", "101", "3190876789", "Hadas");
        ShippingDetails shippingDetails = new ShippingDetails("name",  "city",  "address",  "country",  "zipcode");
        private const int NumThreads = 10;
        private const int NumIterations = 100;
        string productname1 = "product1";
        private MarketManagerFacade marketManagerFacade;
        private ClientManager clientManager;
        string sellmethod = "RegularSell";
        string desc = "nice";
        int productCounter = 0;
        Mock<IShippingSystemFacade> mockShippingSystem;
        Mock<IPaymentSystemFacade> mockPaymentSystem;

        [TestInitialize]
        public void Setup()
        {
            // Initialize the managers
            MarketManagerFacade.Dispose();
            mockShippingSystem = new Mock<IShippingSystemFacade>();
            mockPaymentSystem = new Mock<IPaymentSystemFacade>();
            mockPaymentSystem.Setup(pay =>pay.Connect()).Returns(true);
            mockShippingSystem.Setup(ship => ship.Connect()).Returns(true);
            mockPaymentSystem.Setup(pay =>pay.Pay(It.IsAny<PaymentDetails>(), It.IsAny<double>())).Returns(1);
            mockShippingSystem.Setup(ship =>ship.OrderShippment(It.IsAny<ShippingDetails>())).Returns(1);
            mockShippingSystem.SetReturnsDefault(true);
            mockPaymentSystem.SetReturnsDefault(true);            
            marketManagerFacade = MarketManagerFacade.GetInstance(mockShippingSystem.Object, mockPaymentSystem.Object);
            clientManager = ClientManager.GetInstance();
            marketManagerFacade.InitiateSystemAdmin();
            marketManagerFacade.EnterAsGuest(session1);
            marketManagerFacade.Register(userName, userPassword, email1, userAge);
            token1 = marketManagerFacade.LoginClient(userName, userPassword);
            userId = marketManagerFacade.GetMemberIDrByUserName(userName);
            marketManagerFacade.CreateStore(token1, storeName, email1, phoneNum);
            marketManagerFacade.AddProduct(1, token1, productName1, sellmethod, desc, price1, category1, quantity1, false);

        }
        [TestCleanup]
        public void Cleanup()
        {
            MarketManagerFacade.Dispose();
        }

        [TestMethod]
        public void AddProductToShop()
        {
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(!(store.Products.Count() == 0));
        }

        [TestMethod]
        public void RemoveProductFromShop()
        {
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(!(store.Products.Count() == 0));
            int prodId = 11;
            marketManagerFacade.RemoveProduct(1, token1, prodId);
            store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store.Products.Count == 0);
        }

        [TestMethod]

        public void AddProductToBasket()
        {
            marketManagerFacade.AddToCart(token1, 1, 11, 1);
            Client client = clientManager.GetClientByIdentifier(token1);
            Dictionary<int, Basket> baskets = client.Cart.GetBaskets();
            Basket relevantBasket = baskets[1];
            Assert.IsTrue(relevantBasket.products[productID1] == 1);
        }

        [TestMethod]

        public void RemoveProductFromBasket()
        {
            marketManagerFacade.AddToCart(token1, 1, 11, 1);
            marketManagerFacade.RemoveFromCart(token1, 11, 1, 1);
            Client client = clientManager.GetClientByIdentifier(token1);
            Dictionary<int, Basket> baskets = client.Cart.GetBaskets();
            Basket relevantBasket = baskets[1];
            Assert.IsFalse(relevantBasket.products[productID1] == 1);
        }

        [TestMethod]

        public void AddProductToBasketAndLogout()
        {
            marketManagerFacade.AddToCart(token1, 1, 11, 1);
            Client client = clientManager.GetClientByIdentifier(token1);
            Dictionary<int, Basket> baskets = client.Cart.GetBaskets();
            Basket relevantBasket = baskets[1];
            Assert.IsTrue(relevantBasket.products[productID1] == 1);
            marketManagerFacade.LogoutClient(token1);
            token1 = marketManagerFacade.LoginClient(userName, userPassword);
            client = clientManager.GetClientByIdentifier(token1);
            baskets = client.Cart.GetBaskets();
            relevantBasket = baskets[1];
            Assert.IsTrue(relevantBasket.products[productID1] == 1);
        }

        [TestMethod]
        public void PurchaseCartFail_Payment_OrderCancel()
        {
            marketManagerFacade.AddToCart(token1, 1, 11, 1);
            mockPaymentSystem.Setup(pay =>pay.Pay(It.IsAny<PaymentDetails>(), It.IsAny<double>())).Returns(-1);
            Assert.ThrowsException<Exception>(() => marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails));
            
            Member client = clientManager.GetMemberByIdentifier(token1);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(client.OrderHistory.IsEmpty);
            Assert.IsTrue(client.Cart.GetBaskets()[1].products.ContainsKey(productID1));
            Assert.IsTrue(store.Products.Count == 1);
        }

        [TestMethod]
        public void PurchaseCartFail_Shipping_OrderCancel()
        {
            marketManagerFacade.AddToCart(token1, 1, 11, 1);
            mockShippingSystem.Setup(ship =>ship.OrderShippment(It.IsAny<ShippingDetails>())).Returns(-1);
            Assert.ThrowsException<Exception>(() => marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails));
            Member client = clientManager.GetMemberByIdentifier(token1);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(client.OrderHistory.IsEmpty);
            Assert.IsTrue(client.Cart.GetBaskets()[1].products.ContainsKey(productID1));
            Assert.IsTrue(store.Products.Count == 1);
        }

        [TestMethod]
        public void Offline_Notifications_Success(){
            marketManagerFacade.AddToCart(token1, 1, productID1, 1);
            marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Member client = clientManager.GetMemberByIdentifier(token1);
            Assert.IsTrue(client.alerts.Count == 1);
        }

        [TestMethod]
        public void Offline_Notifications_Fail_NotOffine(){
            marketManagerFacade.NotificationOn(token1);
            marketManagerFacade.AddToCart(token1, 1, productID1, 1);
            marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Member client = clientManager.GetMemberByIdentifier(token1);
            Assert.IsTrue(client.alerts.Count == 0);
        }
    }
}