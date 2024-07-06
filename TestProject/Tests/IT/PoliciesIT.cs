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
using NuGet.Frameworks;

namespace MarketBackend.Tests.IT
{
    [TestClass()]
    public class PoliciesIT
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
        Mock<IShippingSystemFacade> mockShippingSystem;
        Mock<IPaymentSystemFacade> mockPaymentSystem;
        int or_operator = 0;
        int xor_operator = 1;
        int and_operator = 2;

        [TestInitialize]
        public void Setup()
        {
            Task.Run(async () => await AsyncSetUp()).GetAwaiter().GetResult();
        }
        public async Task AsyncSetUp()
        {
            // Initialize the managers
            DBcontext.GetInstance().Dispose();
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
            // await marketManagerFacade.InitiateSystemAdmin();
            await marketManagerFacade.EnterAsGuest(session1);
            await marketManagerFacade.Register(userName, userPassword, email1, userAge);
            token1 = await marketManagerFacade.LoginClient(userName, userPassword);
            userId = await marketManagerFacade.GetMemberIDrByUserName(userName);
            await marketManagerFacade.CreateStore(token1, storeName, email1, phoneNum);
            await marketManagerFacade.AddProduct(1, token1, productName1, sellmethod, desc, price1, category1, quantity1, false);
        }
        [TestCleanup]
        public async Task Cleanup()
        {
            // DBcontext.GetInstance().Dispose();
            MarketManagerFacade.Dispose();
        }

        [TestMethod]
        public async Task AddCompositeRulePurchaseCart_success()
        {
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 10);
            int rule2 = await marketManagerFacade.AddSimpleRule(token1, 1, storeName);
            List<int> rules = [rule1, rule2];
            await marketManagerFacade.AddCompositeRule(token1, 1, or_operator, rules);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._rules.Count == 3);
            await marketManagerFacade.AddToCart(token1, 1, productID1, 1);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddPurchasePolicy(token1, 1, expirationDate, category1, rule1);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Assert.AreEqual(1, store._history._purchases.Count,
            $"Expected puchase history count to be 1 but got {store._history._purchases.Count}");
        }

        [TestMethod]
        public async Task AddDiscountPurchaseCart_success()
        {
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 10);
            int rule2 = await marketManagerFacade.AddSimpleRule(token1, 1, storeName);
            List<int> rules = [rule1, rule2];
            await marketManagerFacade.AddCompositeRule(token1, 1, or_operator, rules);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._rules.Count == 3);
            await marketManagerFacade.AddToCart(token1, 1, productID1, 1);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddDiscountPolicy(token1, 1, expirationDate, category1, rule1, 0.5);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Assert.AreEqual(2.5, store._history._purchases[0].Price,
            $"Expected price to be 2.5 but got {store._history._purchases[0].Price}");
        }

        [TestMethod]
        public async Task PurchaseCart_Quantity_Role_Product_Fail()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, "apple", 1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddPurchasePolicy(token1, 1, expirationDate, "apple", rule1);
            await marketManagerFacade.AddToCart(token1, 1, 12, 10);
            Assert.ThrowsException<Exception>(() => marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails));
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(0, store._history._purchases.Count,
            $"Expected puchase history count to be 0 but got {store._history._purchases.Count}");
        }

        [TestMethod]
        public async Task PurchaseCart_Quantity_Role__product_Success()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, "apple", 1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddPurchasePolicy(token1, 1, expirationDate, "apple", rule1);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(1, store._history._purchases.Count,
            $"Expected puchase history count to be 1 but got {store._history._purchases.Count}");
        }

        [TestMethod]
        public async Task PurchaseCart_Quantity_Role__simple_Fail()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, storeName, 1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddPurchasePolicy(token1, 1, expirationDate, storeName, rule1);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 5);
            Assert.ThrowsException<Exception>(() => marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails));
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(0, store._history._purchases.Count,
            $"Expected puchase history count to be 0 but got {store._history._purchases.Count}");
        }

        [TestMethod]
        public async Task PurchaseCart_Quantity_Role__category_Success()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddPurchasePolicy(token1, 1, expirationDate, category1, rule1);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 2);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(1, store._history._purchases.Count,
            $"Expected puchase history count to be 1 but got {store._history._purchases.Count}");
        }

        [TestMethod]
        public async Task PurchaseCart_Quantity_Role__category_Fail()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddPurchasePolicy(token1, 1, expirationDate, category1, rule1);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 5);
            Assert.ThrowsException<Exception>(() => marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails));
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(0, store._history._purchases.Count,
            $"Expected puchase history count to be 0 but got {store._history._purchases.Count}");
        }

        [TestMethod]
        public async Task PurchaseCart_composite_rule__and_Success()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            int rule2 = await marketManagerFacade.AddTotalPriceRule(token1, 1, category1, 5);
            List<int> rules = [rule1, rule2];
            int composite = await marketManagerFacade.AddCompositeRule(token1, 1, and_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddPurchasePolicy(token1, 1, expirationDate, category1, composite);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 2);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(1, store._history._purchases.Count,
            $"Expected puchase history count to be 1 but got {store._history._purchases.Count}");
        }

        [TestMethod]
        public async Task PurchaseCart_composite_rule__and_Fail_OneNotTrue()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            int rule2 = await marketManagerFacade.AddTotalPriceRule(token1, 1, category1, 5);
            List<int> rules = [rule1, rule2];
            int composite = await marketManagerFacade.AddCompositeRule(token1, 1, and_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddPurchasePolicy(token1, 1, expirationDate, category1, composite);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 5);
            Assert.ThrowsException<Exception>(() => marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails));
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(0, store._history._purchases.Count,
            $"Expected puchase history count to be 0 but got {store._history._purchases.Count}");
        }

        [TestMethod]
        public async Task PurchaseCart_composite_rule__or_Success()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            int rule2 = await marketManagerFacade.AddTotalPriceRule(token1, 1, category1, 5);
            List<int> rules = [rule1, rule2];
            int composite = await marketManagerFacade.AddCompositeRule(token1, 1, or_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddPurchasePolicy(token1, 1, expirationDate, category1, composite);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 2);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(1, store._history._purchases.Count,
            $"Expected puchase history count to be 1 but got {store._history._purchases.Count}");
        }

        [TestMethod]
        public async Task PurchaseCart_composite_rule__and_or_AllFalse()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            int rule2 = await marketManagerFacade.AddTotalPriceRule(token1, 1, category1, 100);
            List<int> rules = [rule1, rule2];
            int composite = await marketManagerFacade.AddCompositeRule(token1, 1, or_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddPurchasePolicy(token1, 1, expirationDate, category1, composite);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 5);
            Assert.ThrowsException<Exception>(() => marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails));
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(0, store._history._purchases.Count,
            $"Expected puchase history count to be 0 but got {store._history._purchases.Count}");
        }

        [TestMethod]
        public async Task PurchaseCart_composite_rule__xor_Success1()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            int rule2 = await marketManagerFacade.AddTotalPriceRule(token1, 1, category1, 5);
            List<int> rules = [rule1, rule2];
            int composite = await marketManagerFacade.AddCompositeRule(token1, 1, xor_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddPurchasePolicy(token1, 1, expirationDate, category1, composite);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 5);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(1, store._history._purchases.Count,
            $"Expected puchase history count to be 1 but got {store._history._purchases.Count}");
        }

        [TestMethod]
        public async Task PurchaseCart_composite_rule__xor_Success2()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            int rule2 = await marketManagerFacade.AddTotalPriceRule(token1, 1, category1, 100);
            List<int> rules = [rule1, rule2];
            int composite = await marketManagerFacade.AddCompositeRule(token1, 1, xor_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddPurchasePolicy(token1, 1, expirationDate, category1, composite);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 2);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(1, store._history._purchases.Count,
            $"Expected puchase history count to be 1 but got {store._history._purchases.Count}");
        }

        [TestMethod]
        public async Task PurchaseCart_composite_rule__and_xor_AllFalse()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            int rule2 = await marketManagerFacade.AddTotalPriceRule(token1, 1, category1, 5);
            List<int> rules = [rule1, rule2];
            int composite = await marketManagerFacade.AddCompositeRule(token1, 1, xor_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddPurchasePolicy(token1, 1, expirationDate, category1, composite);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 2);
            Assert.ThrowsException<Exception>(() => marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails));
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(0, store._history._purchases.Count,
            $"Expected puchase history count to be 0 but got {store._history._purchases.Count}");
        }

        [TestMethod]
        public async Task PurchaseCart_Discount_Role__category_Success()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddDiscountPolicy(token1, 1, expirationDate, category1, rule1, 0.5);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 2);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(1, store._history._purchases.Count,
            $"Expected puchase history count to be 1 but got {store._history._purchases.Count}");
            Assert.AreEqual(10, store._history._purchases[0].Price,
            $"Expected purchase price to be 10, but got {store._history._purchases[0].Price}");
        }

        [TestMethod]
        public async Task PurchaseCart_Discount_Role__category_Fail()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddDiscountPolicy(token1, 1, expirationDate, category1, rule1, 0.5);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 5);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(1, store._history._purchases.Count,
            $"Expected puchase history count to be 1 but got {store._history._purchases.Count}");
            Assert.AreEqual(35, store._history._purchases[0].Price,
            $"Expected purchase price to be 35, but got {store._history._purchases[0].Price}");
        }

        [TestMethod]
        public async Task PurchaseCart_Discount_rule__and_Success()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            int rule2 = await marketManagerFacade.AddTotalPriceRule(token1, 1, category1, 5);
            List<int> rules = [rule1, rule2];
            int composite = await marketManagerFacade.AddCompositeRule(token1, 1, and_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddPurchasePolicy(token1, 1, expirationDate, category1, composite);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 2);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(1, store._history._purchases.Count,
            $"Expected puchase history count to be 1 but got {store._history._purchases.Count}");
            Assert.AreEqual(20, store._history._purchases[0].Price,
            $"Expected purchase price to be 20, but got {store._history._purchases[0].Price}");
        }

        [TestMethod]
        public async Task PurchaseCart_Discount_rule__and_Fail_OneNotTrue()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            int rule2 =await  marketManagerFacade.AddTotalPriceRule(token1, 1, category1, 5);
            List<int> rules = [rule1, rule2];
            int composite =await marketManagerFacade.AddCompositeRule(token1, 1, and_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddDiscountPolicy(token1, 1, expirationDate, category1, composite, 0.5);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 5);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(1, store._history._purchases.Count,
            $"Expected puchase history count to be 1 but got {store._history._purchases.Count}");
            Assert.AreEqual(35, store._history._purchases[0].Price,
            $"Expected purchase price to be 35, but got {store._history._purchases[0].Price}");
        }

        [TestMethod]
        public async Task PurchaseCart_Discount_rule__or_Success()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            int rule2 = await marketManagerFacade.AddTotalPriceRule(token1, 1, category1, 5);
            List<int> rules = [rule1, rule2];
            int composite = await marketManagerFacade.AddCompositeRule(token1, 1, or_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddDiscountPolicy(token1, 1, expirationDate, category1, composite, 0.5);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 2);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(1, store._history._purchases.Count,
            $"Expected puchase history count to be 1 but got {store._history._purchases.Count}");
            Assert.AreEqual(10, store._history._purchases[0].Price,
            $"Expected purchase price to be 10, but got {store._history._purchases[0].Price}");
        }

        [TestMethod]
        public async Task PurchaseCart_Discount_rule__and_or_AllFalse()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            int rule2 = await marketManagerFacade.AddTotalPriceRule(token1, 1, category1, 100);
            List<int> rules = [rule1, rule2];
            int composite = await marketManagerFacade.AddCompositeRule(token1, 1, or_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddDiscountPolicy(token1, 1, expirationDate, category1, composite, 0.5);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 5);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(1, store._history._purchases.Count,
            $"Expected puchase history count to be 1 but got {store._history._purchases.Count}");
            Assert.AreEqual(35, store._history._purchases[0].Price,
            $"Expected purchase price to be 35, but got {store._history._purchases[0].Price}");
        }

        [TestMethod]
        public async Task PurchaseCart_Discount_rule__xor_Success1()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            int rule2 = await marketManagerFacade.AddTotalPriceRule(token1, 1, category1, 5);
            List<int> rules = [rule1, rule2];
            int composite =await  marketManagerFacade.AddCompositeRule(token1, 1, xor_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddDiscountPolicy(token1, 1, expirationDate, category1, composite, 0.5);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 5);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(1, store._history._purchases.Count,
            $"Expected puchase history count to be 1 but got {store._history._purchases.Count}");
            Assert.AreEqual(17.5, store._history._purchases[0].Price,
            $"Expected purchase price to be 17.5, but got {store._history._purchases[0].Price}");
        }

        [TestMethod]
        public async Task PurchaseCart_Discount_rule__xor_Success2()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            int rule2 = await marketManagerFacade.AddTotalPriceRule(token1, 1, category1, 100);
            List<int> rules = [rule1, rule2];
            int composite = await marketManagerFacade.AddCompositeRule(token1, 1, xor_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddDiscountPolicy(token1, 1, expirationDate, category1, composite, 0.5);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 2);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(1, store._history._purchases.Count,
            $"Expected puchase history count to be 1 but got {store._history._purchases.Count}");
            Assert.AreEqual(10, store._history._purchases[0].Price,
            $"Expected purchase price to be 10, but got {store._history._purchases[0].Price}");
        }

        [TestMethod]
        public async Task PurchaseCart_Discount_rule__and_xor_AllFalse()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            int rule2 = await marketManagerFacade.AddTotalPriceRule(token1, 1, category1, 5);
            List<int> rules = [rule1, rule2];
            int composite = await marketManagerFacade.AddCompositeRule(token1, 1, xor_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddDiscountPolicy(token1, 1, expirationDate, category1, composite, 0.5);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 2);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(1, store._history._purchases.Count,
            $"Expected puchase history count to be 1 but got {store._history._purchases.Count}");
            Assert.AreEqual(20, store._history._purchases[0].Price,
            $"Expected purchase price to be 20, but got {store._history._purchases[0].Price}");
        }

        [TestMethod]
        public async Task PurchaseCart_Discount_Success_Purchase_fail()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            int rule2 = await marketManagerFacade.AddTotalPriceRule(token1, 1, category1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddPurchasePolicy(token1, 1, expirationDate, category1, rule1);
            await marketManagerFacade.AddDiscountPolicy(token1, 1, expirationDate, category1, rule2, 0.5);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 5);
            Assert.ThrowsException<Exception>(() => marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails));
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(0, store._history._purchases.Count,
            $"Expected puchase history count to be 0 but got {store._history._purchases.Count}");
        }

        [TestMethod]
        public async Task PurchaseCart_Discount_Fail_Purchase_Success()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            int rule2 = await marketManagerFacade.AddTotalPriceRule(token1, 1, category1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddPurchasePolicy(token1, 1, expirationDate, category1, rule2);
            await marketManagerFacade.AddDiscountPolicy(token1, 1, expirationDate, category1, rule1, 0.5);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 5);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(1, store._history._purchases.Count,
            $"Expected puchase history count to be 1 but got {store._history._purchases.Count}");
            Assert.AreEqual(35, store._history._purchases[0].Price,
            $"Expected purchase price to be 35, but got {store._history._purchases[0].Price}");
        }

        [TestMethod]
        public async Task PurchaseCart_Discount_Success_Purchase_Success()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule2 = await marketManagerFacade.AddTotalPriceRule(token1, 1, category1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            await marketManagerFacade.AddPurchasePolicy(token1, 1, expirationDate, category1, rule2);
            await marketManagerFacade.AddDiscountPolicy(token1, 1, expirationDate, category1, rule2, 0.5);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(1, store._history._purchases.Count,
            $"Expected puchase history count to be 1 but got {store._history._purchases.Count}");
            Assert.AreEqual(17.5, store._history._purchases[0].Price,
            $"Expected purchase price to be 17.5, but got {store._history._purchases[0].Price}");
        }

        [TestMethod]
        public async Task PurchaseCart_Composite_Policies_add()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddTotalPriceRule(token1, 1, category1, 10);
            int rule2 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            int policy1 = await marketManagerFacade.AddDiscountPolicy(token1, 1, expirationDate, category1, rule2, 0.5);
            int policy2 = await marketManagerFacade.AddDiscountPolicy(token1, 1, expirationDate, category1, rule1, 0.1);
            List<int> policies = [policy1, policy2];
            await marketManagerFacade.AddCompositePolicy(token1, 1, expirationDate, category1, 0, policies);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 2);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(1, store._history._purchases.Count,
            $"Expected puchase history count to be 1 but got {store._history._purchases.Count}");
            Assert.AreEqual(8, store._history._purchases[0].Price,
            $"Expected purchase price to be 8, but got {store._history._purchases[0].Price}");
        }

        [TestMethod]
        public async Task PurchaseCart_Composite_Policies_max()
        {
            await marketManagerFacade.AddProduct(1, token1, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = await marketManagerFacade.AddTotalPriceRule(token1, 1, category1, 10);
            int rule2 = await marketManagerFacade.AddQuantityRule(token1, 1, category1, 1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            int policy1 = await marketManagerFacade.AddDiscountPolicy(token1, 1, expirationDate, category1, rule2, 0.5);
            int policy2 = await marketManagerFacade.AddDiscountPolicy(token1, 1, expirationDate, category1, rule1, 0.1);
            List<int> policies = [policy1, policy2];
            await marketManagerFacade.AddCompositePolicy(token1, 1, expirationDate, category1, 1, policies);
            await marketManagerFacade.AddToCart(token1, 1, 12, 2);
            await marketManagerFacade.AddToCart(token1, 1, 11, 2);
            await marketManagerFacade.PurchaseCart(token1, paymentDetails, shippingDetails);
            Store store = await marketManagerFacade.GetStore(1);
            Assert.AreEqual(1, store._history._purchases.Count,
            $"Expected puchase history count to be 1 but got {store._history._purchases.Count}");
            Assert.AreEqual(10, store._history._purchases[0].Price,
            $"Expected purchase price to be 10, but got {store._history._purchases[0].Price}");
        }

        // [TestMethod]
        // public void RunMultyTimes()
        // {
        //     for (int i=0; i<5; i++){
        //         AddCompositeRulePurchaseCart_success();
        //         Cleanup();
        //         Setup();
        //         AddDiscountPurchaseCart_success();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_Quantity_Role_Product_Fail();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_Quantity_Role__product_Success();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_Quantity_Role__simple_Fail();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_Quantity_Role__category_Success();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_Quantity_Role__category_Fail();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_composite_rule__and_Success();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_composite_rule__and_Fail_OneNotTrue();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_composite_rule__or_Success();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_composite_rule__and_or_AllFalse();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_composite_rule__xor_Success1();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_composite_rule__xor_Success2();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_composite_rule__and_xor_AllFalse();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_Discount_Role__category_Success();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_Discount_Role__category_Fail();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_Discount_rule__and_Success();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_Discount_rule__and_Fail_OneNotTrue();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_Discount_rule__or_Success();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_Discount_rule__and_or_AllFalse();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_Discount_rule__xor_Success1();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_Discount_rule__xor_Success2();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_Discount_rule__and_xor_AllFalse();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_Discount_Success_Purchase_fail();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_Discount_Fail_Purchase_Success();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_Discount_Success_Purchase_Success();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_Composite_Policies_add();
        //         Cleanup();
        //         Setup();
        //         PurchaseCart_Composite_Policies_max();
        //         Cleanup();
        //         Setup();
        //     }
        // }
    }
}