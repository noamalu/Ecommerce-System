using System.Collections.Concurrent;
using System.IO.Compression;
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
    public class CorrecntessIT
    {
        string userName = "user1";
        string userName2 = "user2";
        string userName3 = "user3";

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
        int storeId = 1;
        int userId2;
        

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
            marketManagerFacade.EnterAsGuest(userId);
            marketManagerFacade.Register(userId, userName, userPassword, email1, userAge);
            marketManagerFacade.LoginClient(userId, userName, userPassword);
            userId = marketManagerFacade.GetMemberIDrByUserName(userName);
            marketManagerFacade.CreateStore(userId, storeName, email1, phoneNum);
            userId2 = userId + 1;
            marketManagerFacade.EnterAsGuest(userId2);
            marketManagerFacade.Register(userId2, userName2, userPassword, email2, userAge);
            marketManagerFacade.LoginClient(userId2, userName2, userPassword);
            userId2 = marketManagerFacade.GetMemberIDrByUserName(userName2);
        }
        [TestCleanup]
        public void Cleanup()
        {
            MarketManagerFacade.Dispose();
        }

        [TestMethod]
        public void TestConcurrentShopManager()
        {
            Client mem = clientManager.GetClientById(userId);
            // Create multiple threads that add and remove products from the shop
            var threads = new List<Thread>();
            for (int i = 0; i < NumThreads; i++)
            {
                string pName = $"{productname1}-{i}-";
                threads.Add(new Thread(() =>
                {
                    for (int j = 0; j < NumIterations; j++)
                    {
                        Product product = marketManagerFacade.AddProduct(1, userId, productName1, sellmethod, desc, price1, category1, quantity1, false);
                        marketManagerFacade.RemoveProduct(1, userId, product._productid);
                    }
                }));
            }

            // Start the threads and wait for them to finish
            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());

            // Assert that the shop has the correct number of products
            Assert.AreEqual(0, marketManagerFacade.GetStore(storeId)._products.Count);
        }

        [TestMethod]
        public void TwoClientsByLastProductTogether()
        {
            Client mem1 = clientManager.GetClientById(userId);
            Product product = marketManagerFacade.AddProduct(1, userId, productName1, sellmethod, desc, price1, category1, 1, false);
            int storeId = 1;
            Client mem2 = clientManager.GetClientById(userId2);
            marketManagerFacade.AddToCart(userId, storeId, product._productid, 1);
            marketManagerFacade.AddToCart(userId2, storeId, product._productid, 1);
            // Create multiple threads that add and remove products from the shop
            var threads = new List<Thread>();
            foreach (int userId in new int[]{userId, userId2})
            {
                string pName = $"{productname1}-{userId}-";
                threads.Add(new Thread(() =>
                {
                    for (int j = 0; j < NumIterations; j++)
                    {
                        try
                        {
                            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Purchase failed for user {userId}: {ex.Message}");
                        }    
                    }                 
                }));
            }

            // Start the threads and wait for them to finish
            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());

            Dictionary<int, Basket> basket1 = mem1.Cart.GetBaskets();
            Dictionary<int, Basket> basket2 = mem2.Cart.GetBaskets();

            Assert.IsTrue(basket1.Count == 0 || basket2.Count == 0);

        }

        [TestMethod]
        public void RemoveProductAndPurchaseProductTogether()
        {
            Client mem1 = clientManager.GetClientById(userId);
            Product product = marketManagerFacade.AddProduct(1, userId, productName1, sellmethod, desc, price1, category1, 1, false);
            Client mem2 = clientManager.GetClientById(userId);
            marketManagerFacade.AddToCart(userId, storeId, productID1, 1);
            bool thorwnExeptionStore  = false;
            bool thorwnExeptionClient = false;

            var threads = new List<Thread>
            {

                new Thread(() =>
                {
                    try
                    {
                        marketManagerFacade.RemoveProduct(storeId, mem1.Id, productID1);
                    }catch{
                        thorwnExeptionStore = true;
                    }
                        
                }),
                new Thread(() =>
                {
                    try{
                        marketManagerFacade.PurchaseCart(mem2.Id, paymentDetails, shippingDetails);

                    }catch{
                        thorwnExeptionClient = true;
                    }
                    }),
            };

            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());

            Assert.IsTrue(thorwnExeptionStore || thorwnExeptionClient);
            Assert.IsFalse(thorwnExeptionStore && thorwnExeptionClient);
            Dictionary<int, Basket> basket = mem2.Cart.GetBaskets();
            Assert.IsTrue((basket.Count == 1 && thorwnExeptionClient) || (basket.Count == 0 && thorwnExeptionStore));
            
        }

        [TestMethod]
        public void TwoStoreOwnerAppointThirdToManagerTogether()
        {
            Client mem1 = clientManager.GetClientById(userId);
            Client mem2 = clientManager.GetClientById(userId);
            marketManagerFacade.AddManger(userId, storeId, userId2);
            Permission permission = Permission.all;
            marketManagerFacade.AddPermission(userId, storeId, userId2, permission);
            int userId3 = mem2.Id + 1;
            marketManagerFacade.EnterAsGuest(userId3);
            marketManagerFacade.Register(userId3, userName3, userPassword, email1, userAge);
            marketManagerFacade.LoginClient(userId3, userName3, userPassword);
            userId3 = marketManagerFacade.GetMemberIDrByUserName(userName);
            bool thorwnExeption  = false;
            ConcurrentBag<bool> results = new ConcurrentBag<bool>();
            var threads = new List<Thread>()
            {
                new Thread(() =>
                {
                    try
                    {
                        marketManagerFacade.AddManger(userId, storeId, userId3);
                        results.Add(true);
                    }
                    catch{
                        thorwnExeption  = true;
                        results.Add(false);
                    }
                }),
                new Thread(() =>
                {
                    try
                    {
                        marketManagerFacade.AddManger(userId2, storeId, userId3);
                        results.Add(true);
                    }
                    catch{
                        thorwnExeption  = true;
                        results.Add(false);
                    }
                })  
            };
            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());
            int successCount = results.Count(r => r == true);
            int exceptionCount = results.Count(r => r == false);
            Store store = marketManagerFacade.GetStore(storeId);
            Assert.AreEqual(true, thorwnExeption);
            Assert.AreEqual(1, successCount, "Exactly one thread should succeed in adding the manager.");
            Assert.AreEqual(1, exceptionCount, "Exactly one thread should throw an exception.");
            Assert.IsTrue(store.roles.ContainsKey(userId));
        }
    }
}