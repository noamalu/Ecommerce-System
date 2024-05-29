using System.IO.Compression;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Models;
using MarketBackend.Domain.Payment;
using MarketBackend.Domain.Shipping;
using MarketBackend.Services;
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
        string userName2 = "user2";
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
        private const int NumThreads = 10;
        private const int NumIterations = 100;
        string productname1 = "product1";
        private MarketManagerFacade marketManagerFacade;
        private ClientManager clientManager;
        string sellmethod = "RegularSell";
        string desc = "nice";
        int productCounter = 0;

        [TestInitialize]
        public void Setup()
        {
            // Initialize the managers
            MarketManagerFacade.Dispose();
            var mockShippingSystem = new Mock<IShippingSystemFacade>();
            var mockPaymentSystem = new Mock<IPaymentSystemFacade>();
            mockPaymentSystem.Setup(pay =>pay.Connect()).Returns(true);
            mockShippingSystem.Setup(ship => ship.Connect()).Returns(true);
            mockPaymentSystem.Setup(pay =>pay.Pay(It.IsAny<PaymentDetails>(), It.IsAny<double>())).Returns(1);
            mockShippingSystem.Setup(ship =>ship.OrderShippment(It.IsAny<ShippingDetails>())).Returns(1);
            mockShippingSystem.SetReturnsDefault(true);
            mockPaymentSystem.SetReturnsDefault(true);            
            marketManagerFacade = MarketManagerFacade.GetInstance(mockShippingSystem.Object, mockPaymentSystem.Object);
            clientManager = ClientManager.GetInstance();
            marketManagerFacade.InitiateSystemAdmin();
        }
        [TestCleanup]
        public void Cleanup()
        {
            MarketManagerFacade.Dispose();
        }

        [TestMethod]
        public void AddProductToShop()
        {
            marketManagerFacade.EnterAsGuest(userId);
            marketManagerFacade.Register(userId, userName, userPassword, email1, userAge);
            marketManagerFacade.LoginClient(userId, userName, userPassword);
            userId = marketManagerFacade.GetMemberIDrByUserName(userName);
            Client mem = clientManager.GetClientById(userId);
            marketManagerFacade.CreateStore(userId, storeName, email1, phoneNum);
            marketManagerFacade.AddProduct(1, userId, productName1, sellmethod, desc, price1, category1, quantity1, false);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(!(store.Products.Count() == 0));
        }

        [TestMethod]
        public void RemoveProductFromShop()
        {
            marketManagerFacade.EnterAsGuest(userId);
            marketManagerFacade.Register(userId, userName, userPassword, email1, userAge);
            marketManagerFacade.LoginClient(userId, userName, userPassword);
            userId = marketManagerFacade.GetMemberIDrByUserName(userName);
            Client mem = clientManager.GetClientById(userId);
            marketManagerFacade.CreateStore(userId, storeName, email1, phoneNum);
            marketManagerFacade.AddProduct(1, userId, productName1, sellmethod, desc, price1, category1, quantity1, false);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(!(store.Products.Count() == 0));
            int prodId = 11;
            marketManagerFacade.RemoveProduct(1, userId, prodId);
            store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store.Products.Count == 0);
        }

        [TestMethod]

        public void AddProductToBasket()
        {
            marketManagerFacade.EnterAsGuest(userId);
            marketManagerFacade.Register(userId, userName, userPassword, email1, userAge);
            marketManagerFacade.LoginClient(userId, userName, userPassword);
            userId = marketManagerFacade.GetMemberIDrByUserName(userName);
            marketManagerFacade.CreateStore(userId, storeName, email1, phoneNum);
            marketManagerFacade.AddProduct(1, userId, productName1, sellmethod, desc, price1, category1, quantity1, false);
            Product product = marketManagerFacade.GetStore(1).Products.ElementAt(0);
            marketManagerFacade.AddToCart(userId, 1, 11, 1);
            Client client = clientManager.GetClientById(userId);
            Dictionary<int, Basket> baskets = client.Cart.GetBaskets();
            Basket relevantBasket = baskets[1];
            Assert.IsTrue(relevantBasket.products[productID1] == 1);
        }

        [TestMethod]

        public void RemoveProductFromBasket()
        {
            marketManagerFacade.EnterAsGuest(userId);
            marketManagerFacade.Register(userId, userName, userPassword, email1, userAge);
            marketManagerFacade.LoginClient(userId, userName, userPassword);
            userId = marketManagerFacade.GetMemberIDrByUserName(userName);
            marketManagerFacade.CreateStore(userId, storeName, email1, phoneNum);
            marketManagerFacade.AddProduct(1, userId, productName1, sellmethod, desc, price1, category1, quantity1, false);
            Product product = marketManagerFacade.GetStore(1).Products.ElementAt(0);
            marketManagerFacade.AddToCart(userId, 1, 11, 1);
            marketManagerFacade.RemoveFromCart(userId, 11, 1, 1);
            Client client = clientManager.GetClientById(userId);
            Dictionary<int, Basket> baskets = client.Cart.GetBaskets();
            Basket relevantBasket = baskets[1];
            Assert.IsFalse(relevantBasket.products[productID1] == 1);
        }

        [TestMethod]

        public void AddProductToBasketAndLogout()
        {
            marketManagerFacade.EnterAsGuest(userId);
            marketManagerFacade.Register(userId, userName, userPassword, email1, userAge);
            marketManagerFacade.LoginClient(userId, userName, userPassword);
            userId = marketManagerFacade.GetMemberIDrByUserName(userName);
            marketManagerFacade.CreateStore(userId, storeName, email1, phoneNum);
            marketManagerFacade.AddProduct(1, userId, productName1, sellmethod, desc, price1, category1, quantity1, false);
            Product product = marketManagerFacade.GetStore(1).Products.ElementAt(0);
            marketManagerFacade.AddToCart(userId, 1, 11, 1);
            Client client = clientManager.GetClientById(userId);
            Dictionary<int, Basket> baskets = client.Cart.GetBaskets();
            Basket relevantBasket = baskets[1];
            Assert.IsTrue(relevantBasket.products[productID1] == 1);
            marketManagerFacade.LogoutClient(userId);
            marketManagerFacade.LoginClient(userId, userName, userPassword);
            client = clientManager.GetClientById(userId);
            baskets = client.Cart.GetBaskets();
            relevantBasket = baskets[1];
            Assert.IsTrue(relevantBasket.products[productID1] == 1);
        }
    }
}