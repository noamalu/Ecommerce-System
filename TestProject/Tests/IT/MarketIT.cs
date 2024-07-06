using System.IO.Compression;
using MarketBackend.DAL.DTO;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Models;
using MarketBackend.Domain.Payment;
using MarketBackend.Domain.Shipping;
using MarketBackend.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog;

namespace MarketBackend.Tests.IT
{
    [TestClass()]
    public class MarketIT
    {
        // Define test data
        string userName = "user1";
        string session1 = "1";
        string token1;
        string userName2 = "user2";
        string session2 = "2";
        string token2;
        string userPassword = "pass1";
        string email1 = "printz@post.bgu.ac.il";
        string email2 = "hadaspr100@gmail.com";
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
        PaymentDetails paymentDetails = new PaymentDetails("ILS", "5326888878675678", "2027", "10", "101", "3190876789", "Hadas");
        ShippingDetails shippingDetails = new ShippingDetails("name", "city", "address", "country", "zipcode");
        private MarketManagerFacade marketManagerFacade;
        private ClientManager clientManager;
        string sellmethod = "RegularSell";
        string desc = "nice";
        Mock<IShippingSystemFacade> mockShippingSystem;
        Mock<IPaymentSystemFacade> mockPaymentSystem;

        [TestInitialize]
        public void Setup()
        {
            Task.Run(async () => await AsyncSetUp()).GetAwaiter().GetResult();
        }
        public async Task AsyncSetUp()
        {
            // Initialize the managers and mock systems
            DBcontext.GetInstance().Dispose();
            MarketManagerFacade.Dispose();
            mockShippingSystem = new Mock<IShippingSystemFacade>();
            mockPaymentSystem = new Mock<IPaymentSystemFacade>();
            mockPaymentSystem.Setup(pay => pay.Connect()).Returns(true);
            mockShippingSystem.Setup(ship => ship.Connect()).Returns(true);
            mockPaymentSystem.Setup(pay => pay.Pay(It.IsAny<PaymentDetails>(), It.IsAny<double>())).Returns(1);
            mockShippingSystem.Setup(ship => ship.OrderShippment(It.IsAny<ShippingDetails>())).Returns(1);
            mockShippingSystem.SetReturnsDefault(true);
            mockPaymentSystem.SetReturnsDefault(true);

            marketManagerFacade = MarketManagerFacade.GetInstance(mockShippingSystem.Object, mockPaymentSystem.Object);
            clientManager = ClientManager.GetInstance();
            // await marketManagerFacade.InitiateSystemAdmin();
            await marketManagerFacade.EnterAsGuest(session1);
            await marketManagerFacade.Register(userName, userPassword, email1, userAge);
            token1 = await marketManagerFacade.LoginClient(userName, userPassword);
            userId = await marketManagerFacade.GetMemberIDrByUserName(userName);
            await marketManagerFacade.CreateStore(token1, storeName, email1, phoneNum);
            await marketManagerFacade.AddProduct(1, token1, productName1, sellmethod, desc, price1, category1, quantity1, false);
        }

        [TestCleanup]
        public void Cleanup()
        {
            DBcontext.GetInstance().Dispose();
            MarketManagerFacade.Dispose();
        }

        [TestMethod]
        public async Task AddProductToShop()
        {
            Store store = await marketManagerFacade.GetStore(1);
            Assert.IsTrue(store.Products.Count() > 0, "Expected the store to have products after adding one.");
        }

        [TestMethod]
        public async Task RemoveProductFromShop()
        {
            Store store = await marketManagerFacade.GetStore(1);
            Assert.IsTrue(store.Products.Count() > 0, "Expected the store to have products before removal.");
            int prodId = 11;
            await marketManagerFacade.RemoveProduct(1, token1, prodId);
            store = await marketManagerFacade.GetStore(1);
            Assert.IsTrue(store.Products.Count == 0, "Expected the store to have no products after removal.");
        }

        [TestMethod]
        public async Task AddProductToBasket()
        {
            await marketManagerFacade.AddToCart(token1, 1, productID1, 1);
            Client client = clientManager.GetClientByIdentifier(token1);
            Dictionary<int, Basket> baskets = await client.Cart.GetBaskets();
            Basket relevantBasket = baskets[1];
            Assert.IsTrue(relevantBasket.products[productID1] == 1, "Expected the product to be added to the basket.");
        }

        [TestMethod]
        public async Task RemoveProductFromBasket()
        {
            await marketManagerFacade.AddToCart(token1, 1, productID1, 1);
            await marketManagerFacade.RemoveFromCart(token1, 11, 1, 1);
            Client client = clientManager.GetClientByIdentifier(token1);
            Dictionary<int, Basket> baskets = await client.Cart.GetBaskets();
            Basket relevantBasket = baskets[1];
            Assert.IsFalse(relevantBasket.products.ContainsKey(productID1), "Expected the product to be removed from the basket.");
        }

        [TestMethod]
        public async Task AddProductToBasketAndLogout()
        {
            await marketManagerFacade.AddToCart(token1, 1, 11, 1);
            Client client = clientManager.GetClientByIdentifier(token1);
            Dictionary<int, Basket> baskets = await client.Cart.GetBaskets();
            Basket relevantBasket = baskets[1];
            Assert.IsTrue(relevantBasket.products[productID1] == 1, "Expected the product to be added to the basket.");
            await marketManagerFacade.LogoutClient(token1);
            token1 = await marketManagerFacade.LoginClient(userName, userPassword);
            client = clientManager.GetClientByIdentifier(token1);
            baskets = await client.Cart.GetBaskets();
            relevantBasket = baskets[1];
            Assert.IsTrue(relevantBasket.products[productID1] == 1, "Expected the product to persist in the basket after logout and login.");
        }

        [TestMethod]
        public async Task PurchaseCartFail_Payment_OrderCancel()
        {
            await marketManagerFacade.AddToCart(token1, 1, 11, 1);
            mockPaymentSystem.Setup(pay => pay.Pay(It.IsAny<PaymentDetails>(), It.IsAny<double>())).Returns(-1);
            await Assert.ThrowsExceptionAsync<Exception>(() => marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails), "Expected purchase to fail due to payment issue.");
            
            Member client = await clientManager.GetMemberByIdentifier(token1);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.IsTrue(client.OrderHistory.IsEmpty, "Expected no orders in client history after payment failure.");
            Assert.IsTrue((await client.Cart.GetBaskets())[1].products.ContainsKey(productID1), "Expected the product to remain in the cart after payment failure.");
            Assert.IsTrue(store.Products.Count == 1, "Expected the product to remain in the store after payment failure.");
        }

        [TestMethod]
        public async Task PurchaseCartFail_Shipping_OrderCancel()
        {
            await marketManagerFacade.AddToCart(token1, 1, 11, 1);
            mockShippingSystem.Setup(ship => ship.OrderShippment(It.IsAny<ShippingDetails>())).Returns(-1);
            await Assert.ThrowsExceptionAsync<Exception>(() => marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails), "Expected purchase to fail due to shipping issue.");
            
            Member client = await clientManager.GetMemberByIdentifier(token1);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.IsTrue(client.OrderHistory.IsEmpty, "Expected no orders in client history after shipping failure.");
            Assert.IsTrue((await client.Cart.GetBaskets())[1].products.ContainsKey(productID1), "Expected the product to remain in the cart after shipping failure.");
            Assert.IsTrue(store.Products.Count == 1, "Expected the product to remain in the store after shipping failure.");
        }

        [TestMethod]
        public async Task Offline_Notifications_Success()
        {
            await marketManagerFacade.NotificationOff(token1);
            await marketManagerFacade.AddToCart(token1, 1, productID1, 1);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Member client = await clientManager.GetMemberByIdentifier(token1);
            Assert.IsTrue(client.alerts.Count == 1, "Expected one notification after purchase when offline notifications are enabled.");
        }

        [TestMethod]
        public async Task Offline_Notifications_Fail_NotOffline()
        {
            await marketManagerFacade.AddToCart(token1, 1, productID1, 1);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Member client = await clientManager.GetMemberByIdentifier(token1);
            Assert.IsTrue(client.alerts.Count == 0, "Expected no notifications after purchase when offline notifications are disabled.");
        }

        // [TestMethod]
        // public void RunMultyTimes()
        // {
        //     for (int i=0; i<5; i++){
        //         AddProductToShop();
        //         Cleanup();
        //         Setup();
        //         RemoveProductFromShop();
        //         Cleanup();
        //         Setup();
        //         AddProductToBasket();
        //         Cleanup();
        //         Setup();
        //         RemoveProductFromBasket();
        //         Cleanup();
        //         Setup();
        //         AddProductToBasketAndLogout();
        //         Cleanup();
        //         Setup();
        //         PurchaseCartFail_Payment_OrderCancel();
        //         Cleanup();
        //         Setup();
        //         PurchaseCartFail_Shipping_OrderCancel();
        //         Cleanup();
        //         Setup();
        //         Offline_Notifications_Success();
        //         Cleanup();
        //         Setup();
        //         Offline_Notifications_Fail_NotOffline();
        //         Cleanup();
        //         Setup();
        //     }
        // }
    }
}
