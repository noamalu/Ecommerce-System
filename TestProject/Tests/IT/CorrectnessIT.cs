using System.Collections.Concurrent;
using System.IO.Compression;
using MarketBackend.DAL;
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
    public class CorrecntessIT
    {
        string userName = "user1";
        string session1 = "1";
        string token1;
        string userName2 = "user2";
        string session2 = "2";
        string token2;

        string userName3 = "user3";
        string session3 = "3";
        string token3;

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
        PaymentDetails paymentDetails = new PaymentDetails("ILS", "5326888878675678", "2027", "10", "101", "3190876789", "Hadas"); 
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
            Task.Run(async () => await AsyncSetUp()).GetAwaiter().GetResult();
        }
        public async Task AsyncSetUp()
        {
            // Initialize the managers and mock systems
            DBcontext.GetInstance().Dispose();
            MarketManagerFacade.Dispose();
            BasketRepositoryRAM.Dispose();
            ClientRepositoryRAM.Dispose();
            PolicyRepositoryRAM.Dispose();
            ProductRepositoryRAM.Dispose();
            PurchaseRepositoryRAM.Dispose();
            RoleRepositoryRAM.Dispose();
            RuleRepositoryRAM.Dispose();
            StoreRepositoryRAM.Dispose();
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
            // await marketManagerFacade.InitiateSystemAdmin();
            await marketManagerFacade.EnterAsGuest(session1);
            await marketManagerFacade.Register(userName, userPassword, email1, userAge);
            token1 = await marketManagerFacade.LoginClient(userName, userPassword);
            userId = await marketManagerFacade.GetMemberIDrByUserName(userName);
            await marketManagerFacade.CreateStore(token1, storeName, email1, phoneNum);
            userId2 = userId + 1;
            await marketManagerFacade.EnterAsGuest(session2);
            await marketManagerFacade.Register(userName2, userPassword, email2, userAge);
            token2 = await marketManagerFacade.LoginClient(userName2, userPassword);
            userId2 = await marketManagerFacade.GetMemberIDrByUserName(userName2);
        }

        [TestCleanup]
        public void Cleanup()
        {
            DBcontext.GetInstance().Dispose();
            MarketManagerFacade.Dispose();
            BasketRepositoryRAM.Dispose();
            ClientRepositoryRAM.Dispose();
            PolicyRepositoryRAM.Dispose();
            ProductRepositoryRAM.Dispose();
            PurchaseRepositoryRAM.Dispose();
            RoleRepositoryRAM.Dispose();
            RuleRepositoryRAM.Dispose();
            StoreRepositoryRAM.Dispose();
        }

        [TestMethod]
        public async Task TestConcurrentShopManager()
        {
            Client mem = clientManager.GetClientByIdentifier(token1);
            // Create multiple threads that add and remove products from the shop
            var threads = new List<Thread>();
            for (int i = 0; i < NumThreads; i++)
            {
                string pName = $"{productname1}-{i}-";
                threads.Add(new Thread(async () =>
                {
                    for (int j = 0; j < NumIterations; j++)
                    {
                        Product product = await marketManagerFacade.AddProduct(1, token1, productName1, sellmethod, desc, price1, category1, quantity1, false);
                        await marketManagerFacade.RemoveProduct(1, token1, product._productId);
                    }
                }));
            }

            // Start the threads and wait for them to finish
            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());

            // Assert that the shop has the correct number of products
            Assert.AreEqual(0, (await marketManagerFacade.GetStore(storeId))._products.Count, 
            $"Expected the store to have zero products but got: {(await marketManagerFacade.GetStore(storeId))._products.Count}.");
        }

        [TestMethod]
        public async Task TwoClientsByLastProductTogether()
        {
            Client mem1 = clientManager.GetClientByIdentifier(token1);
            Product product = await marketManagerFacade.AddProduct(1, token1, productName1, sellmethod, desc, price1, category1, 1, false);
            int storeId = 1;
            Client mem2 = clientManager.GetClientByIdentifier(token2);
            await marketManagerFacade.AddToCart(token1, storeId, product._productId, 1);
            await marketManagerFacade.AddToCart(token2, storeId, product._productId, 1);

            // Create multiple threads that attempt to purchase the product
            var threads = new List<Thread>();
            foreach (int userId in new int[]{userId, userId2})
            {
                string pName = $"{productname1}-{userId}-";
                threads.Add(new Thread(async () =>
                {
                    for (int j = 0; j < NumIterations; j++)
                    {
                        try
                        {
                            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
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

            Dictionary<int, Basket> basket1 = await mem1.Cart.GetBaskets();
            Dictionary<int, Basket> basket2 = await mem2.Cart.GetBaskets();

            Assert.IsTrue(basket1.Count == 0 || basket2.Count == 0, "Expected that one of the clients has an empty cart, indicating only one successful purchase.");
        }

        [TestMethod]
        public async Task RemoveProductAndPurchaseProductTogether()
        {
            Client mem1 = clientManager.GetClientByIdentifier(token1);
            Product product = await marketManagerFacade.AddProduct(1, token1, productName1, sellmethod, desc, price1, category1, 1, false);
            Client mem2 = clientManager.GetClientByIdentifier(token2);
            await marketManagerFacade.AddToCart(token2, storeId, productID1, 1);
            bool thorwnExeptionStore  = false;
            bool thorwnExeptionClient = false;

            // Create threads for removing product and purchasing product concurrently
            var threads = new List<Thread>
            {
                new Thread(async () =>
                {
                    try
                    {
                        await marketManagerFacade.RemoveProduct(storeId, token1, productID1);
                    }catch{
                        thorwnExeptionStore = true;
                    }
                }),
                new Thread(async () =>
                {
                    try{
                        await marketManagerFacade.PurchaseCart(token2, paymentDetails, shippingDetails);
                    }catch{
                        thorwnExeptionClient = true;
                    }
                }),
            };

            // Start the threads and wait for them to finish
            threads.ForEach(t => t.Start());
            threads.ForEach(t => t.Join());

            Assert.IsTrue(thorwnExeptionStore || thorwnExeptionClient, "Expected at least one exception to be thrown, either in removing product or purchasing product.");
            Assert.IsFalse(thorwnExeptionStore && thorwnExeptionClient, "Expected only one of the operations to throw an exception, not both.");
            Dictionary<int, Basket> basket = await mem2.Cart.GetBaskets();
            Assert.IsTrue((basket.Count == 1 && thorwnExeptionClient) || (basket.Count == 0 && thorwnExeptionStore), "Expected the cart to be consistent with the operations.");
        }

        [TestMethod]
        public async Task TwoStoreOwnerAppointThirdToManagerTogether()
        {
            Client mem1 = clientManager.GetClientByIdentifier(token1);
            Client mem2 = clientManager.GetClientByIdentifier(token2);
            await marketManagerFacade.AddManger(token1, storeId, userName2);
            Permission permission = Permission.all;
            await marketManagerFacade.AddPermission(token1, storeId, userName2, permission);
            int userId3 = mem2.Id + 1;
            await marketManagerFacade.EnterAsGuest(session3);
            await marketManagerFacade.Register(userName3, userPassword, email1, userAge);
            token3 = await marketManagerFacade.LoginClient(userName3, userPassword);
            userId3 = await marketManagerFacade.GetMemberIDrByUserName(userName);
            bool thorwnExeption  = false;
            ConcurrentBag<bool> results = new ConcurrentBag<bool>();

            // Create threads to appoint the third user as a manager
            var threads = new List<Thread>()
            {
                new Thread(async () =>
                {
                    try
                    {
                        await marketManagerFacade.AddManger(token1, storeId, userName3);
                        results.Add(true);
                    }
                    catch{
                        thorwnExeption  = true;
                        results.Add(false);
                    }
                }),
                new Thread(async () =>
                {
                    try
                    {
                        await marketManagerFacade.AddManger(token2, storeId, userName3);
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
            Store store = await marketManagerFacade.GetStore(storeId);

            Assert.AreEqual(true, thorwnExeption, "Expected one of the threads to throw an exception.");
            Assert.AreEqual(1, successCount, "Exactly one thread should succeed in adding the manager.");
            Assert.AreEqual(1, exceptionCount, "Exactly one thread should throw an exception.");
            Assert.IsTrue(store.roles.ContainsKey(userName), "Expected the new manager to be added to the store roles.");
        }

        // [TestMethod]
        // public void RunMultyTimes()
        // {
        //     for (int i=0; i<5; i++){
        //         TestConcurrentShopManager();
        //         Cleanup();
        //         Setup();
        //         TwoClientsByLastProductTogether();
        //         Cleanup();
        //         Setup();
        //         RemoveProductAndPurchaseProductTogether();
        //         Cleanup();
        //         Setup();
        //         TwoStoreOwnerAppointThirdToManagerTogether();
        //         Cleanup();
        //         Setup();
        //     }
        // }
    }
}
