using System.IO.Compression;
using MarketBackend.Domain.Market_Client;
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

        [TestInitialize]
        public void Setup()
        {
            // Initialize the managers
            MarketManagerFacade.Dispose();
            marketManagerFacade = MarketManagerFacade.GetInstance();
            clientManager = ClientManager.GetInstance();
            var mockShippingSystem = new Mock<IShippingSystemFacade>();
            var mockPaymentSystem = new Mock<IPaymentSystemFacade>();
            mockShippingSystem.SetReturnsDefault(true);
            mockPaymentSystem.SetReturnsDefault(true);
            marketManagerFacade.InitiateSystemAdmin();
        }
        [TestCleanup]
        public void Cleanup()
        {
            MarketManagerFacade.Dispose();
        }

        [TestMethod]
        public void TestConcurrentShopManager()
        {
            marketManagerFacade.EnterAsGuest(userId);
            marketManagerFacade.Register(userId, userName, userPassword, email1, userAge);
            marketManagerFacade.LoginClient(userId, userName, userPassword);
            userId = marketManagerFacade.GetMemberIDrByUserName(userName);
            Client mem = clientManager.GetClientById(userId);
            marketManagerFacade.CreateStore(userId, storeName, email1, phoneNum);
            int storeId = 1;
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
            marketManagerFacade.EnterAsGuest(userId);
            marketManagerFacade.Register(userId, userName, userPassword, email1, userAge);
            marketManagerFacade.LoginClient(userId, userName, userPassword);
            userId = marketManagerFacade.GetMemberIDrByUserName(userName);
            Client mem1 = clientManager.GetClientById(userId);
            marketManagerFacade.CreateStore(userId, storeName, email1, phoneNum);
            Product product = marketManagerFacade.AddProduct(1, userId, productName1, sellmethod, desc, price1, category1, 1, false);
            int storeId = 1;
            int userId2 = mem1.Id + 1;
            marketManagerFacade.EnterAsGuest(userId2);
            marketManagerFacade.Register(userId2, userName2, userPassword, email2, userAge);
            marketManagerFacade.LoginClient(userId2, userName2, userPassword);
            userId2 = marketManagerFacade.GetMemberIDrByUserName(userName2);
            Client mem2 = clientManager.GetClientById(userId2);
            // Create multiple threads that add and remove products from the shop
            var threads = new List<Thread>();
            foreach (int userId in new int[]{userId, userId2})
            {
                string pName = $"{productname1}-{userId}-";
                threads.Add(new Thread(() =>
                {
                    try
            {
                marketManagerFacade.AddToCart(userId, storeId, product._productid, 1);
                marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            }
            catch (Exception ex)
            {
                // Handle exception here (e.g., log error)
                Console.WriteLine($"Purchase failed for user {userId}: {ex.Message}");
                    }
                }));
            }

            // Start the threads and wait for them to finish
            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());

            // Check that only one product remains in the shop
            int expectedInventory = 0;
            Assert.AreEqual(expectedInventory, product._quantity, "Shop should have only 0 products remaining");

        }

        [TestMethod]
        public void RemoveProductAndPurchaseProductTogether()
        {
            marketManagerFacade.EnterAsGuest(userId);
            marketManagerFacade.Register(userId, userName, userPassword, email1, userAge);
            marketManagerFacade.LoginClient(userId, userName, userPassword);
            userId = marketManagerFacade.GetMemberIDrByUserName(userName);
            Client mem1 = clientManager.GetClientById(userId);
            marketManagerFacade.CreateStore(userId, storeName, email1, phoneNum);
            Product product = marketManagerFacade.AddProduct(1, userId, productName1, sellmethod, desc, price1, category1, 1, false);
            int storeId = 1;
            int userId2 = mem1.Id + 1;
            marketManagerFacade.EnterAsGuest(userId2);
            marketManagerFacade.Register(userId2, userName2, userPassword, email1, userAge);
            marketManagerFacade.LoginClient(userId2, userName2, userPassword);
            userId2 = marketManagerFacade.GetMemberIDrByUserName(userName2);
            Client mem2 = clientManager.GetClientById(userId);
            // Create multiple threads that add and remove products from the shop
            Boolean thorwnExeption  = false;
            var threads = new List<Thread>
            {
                new Thread(() =>
                {
                    marketManagerFacade.RemoveProduct(storeId, mem1.Id, productID1);
                        
                }),
                new Thread(() =>
                {
                    try{
                        marketManagerFacade.AddToCart(userId, storeId, productID1, 1);
                        Thread.Sleep(500);
                        marketManagerFacade.PurchaseCart(mem2.Id, paymentDetails, shippingDetails);

                    }catch{
                        thorwnExeption = true;
                    }
                    }),
            };

            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());

            Assert.AreEqual(true, thorwnExeption);
        }

        [TestMethod]
        public void TwoStoreOwnerAppointThirdToManagerTogether()
        {
            marketManagerFacade.EnterAsGuest(userId);
            marketManagerFacade.Register(userId, userName, userPassword, email1, userAge);
            marketManagerFacade.LoginClient(userId, userName, userPassword);
            userId = marketManagerFacade.GetMemberIDrByUserName(userName);
            Client mem1 = clientManager.GetClientById(userId);
            marketManagerFacade.CreateStore(userId, storeName, email1, phoneNum);
            int storeId = 1;
            int userId2 = userId++;
            marketManagerFacade.EnterAsGuest(userId2);
            marketManagerFacade.Register(userId2, userName, userPassword, email1, userAge);
            marketManagerFacade.LoginClient(userId2, userName, userPassword);
            userId2 = marketManagerFacade.GetMemberIDrByUserName(userName);
            Client mem2 = clientManager.GetClientById(userId);
            marketManagerFacade.AddManger(userId, storeId, userId2);
            Permission permission = Permission.all;
            marketManagerFacade.AddPermission(userId, storeId, userId2, permission);
            int userId3 = userId++;
            marketManagerFacade.EnterAsGuest(userId3);
            marketManagerFacade.Register(userId3, userName, userPassword, email1, userAge);
            marketManagerFacade.LoginClient(userId3, userName, userPassword);
            userId3 = marketManagerFacade.GetMemberIDrByUserName(userName);

            // Create multiple threads that add and remove products from the shop
            var threads = new List<Thread>();
            for (int i = 0; i < 2; i++)
            {
                string pName = $"{productname1}-{i}-";
                threads.Add(new Thread(() =>
                {
                    for (int j = 0; j < NumIterations; j++)
                    {
                        marketManagerFacade.AddManger(i, storeId, userId3);
                    }
                }));
            }

            // Start the threads and wait for them to finish
            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());

            // Assert that the shop has the correct number of products
            //need to check if only one succeed.
        }



        
    }
}