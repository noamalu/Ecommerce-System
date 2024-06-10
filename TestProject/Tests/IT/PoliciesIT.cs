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
    public class PoliciesIT
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
        Mock<IShippingSystemFacade> mockShippingSystem;
        Mock<IPaymentSystemFacade> mockPaymentSystem;
        int or_operator = 0;
        int xor_operator = 1;
        int and_operator = 2;

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
            marketManagerFacade.EnterAsGuest(userId);
            marketManagerFacade.Register(userId, userName, userPassword, email1, userAge);
            marketManagerFacade.LoginClient(userId, userName, userPassword);
            userId = marketManagerFacade.GetMemberIDrByUserName(userName);
            marketManagerFacade.CreateStore(userId, storeName, email1, phoneNum);
            marketManagerFacade.AddProduct(1, userId, productName1, sellmethod, desc, price1, category1, quantity1, false);
        }
        [TestCleanup]
        public void Cleanup()
        {
            MarketManagerFacade.Dispose();
        }

        [TestMethod]
        public void AddCompositeRulePurchaseCart_success()
        {
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 10);
            int rule2 = marketManagerFacade.AddSimpleRule(userId, 1, storeName);
            List<int> rules = [rule1, rule2];
            marketManagerFacade.AddCompositeRule(userId, 1, or_operator, rules);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._rules.Count == 3);
            marketManagerFacade.AddToCart(userId, 1, productID1, 1);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddPurchasePolicy(userId, 1, expirationDate, storeName, rule1);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Assert.IsTrue(store._history._purchases.Count == 1);
        }

        [TestMethod]
        public void AddDiscountPurchaseCart_success()
        {
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 10);
            int rule2 = marketManagerFacade.AddSimpleRule(userId, 1, storeName);
            List<int> rules = [rule1, rule2];
            marketManagerFacade.AddCompositeRule(userId, 1, or_operator, rules);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._rules.Count == 3);
            marketManagerFacade.AddToCart(userId, 1, productID1, 1);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddDiscountPolicy(userId, 1, expirationDate, category1, rule1, 0.5);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Assert.IsTrue(store._history._purchases[0].Price == 2.5);
        }

        [TestMethod]
        public void PurchaseCart_Quantity_Role_Product_Fail()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, "apple", 1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddPurchasePolicy(userId, 1, expirationDate, "apple", rule1);
            marketManagerFacade.AddToCart(userId, 1, 12, 10);
            Assert.ThrowsException<Exception>(() => marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails));
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 0);
        }

        [TestMethod]
        public void PurchaseCart_Quantity_Role__product_Success()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, "apple", 1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddPurchasePolicy(userId, 1, expirationDate, "apple", rule1);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 1);
        }

        [TestMethod]
        public void PurchaseCart_Quantity_Role__simple_Fail()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, storeName, 1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddPurchasePolicy(userId, 1, expirationDate, storeName, rule1);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 5);
            Assert.ThrowsException<Exception>(() => marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails));
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 0);
        }

        //to do: need to fix role by store name
        // [TestMethod]
        // public void PurchaseCart_Quantity_Role__simple_Success()
        // {
        //     marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
        //     int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, storeName, 1, 5);
        //     DateTime expirationDate = DateTime.Now.AddDays(2);
        //     marketManagerFacade.AddPurchasePolicy(userId, 1, expirationDate, storeName, rule1);
        //     marketManagerFacade.AddToCart(userId, 1, 12, 2);
        //     marketManagerFacade.AddToCart(userId, 1, 11, 2);
        //     marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
        //     Store store = marketManagerFacade.GetStore(1);
        //     Assert.IsTrue(store._history._purchases.Count == 1);
        // }

        [TestMethod]
        public void PurchaseCart_Quantity_Role__category_Success()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddPurchasePolicy(userId, 1, expirationDate, category1, rule1);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 2);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 1);
        }

        [TestMethod]
        public void PurchaseCart_Quantity_Role__category_Fail()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddPurchasePolicy(userId, 1, expirationDate, category1, rule1);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 5);
            Assert.ThrowsException<Exception>(() => marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails));
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 0);
        }

        [TestMethod]
        public void PurchaseCart_composite_rule__and_Success()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            int rule2 = marketManagerFacade.AddTotalPriceRule(userId, 1, category1, 5);
            List<int> rules = [rule1, rule2];
            int composite = marketManagerFacade.AddCompositeRule(userId, 1, and_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddPurchasePolicy(userId, 1, expirationDate, category1, composite);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 2);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 1);
        }

        [TestMethod]
        public void PurchaseCart_composite_rule__and_Fail_OneNotTrue()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            int rule2 = marketManagerFacade.AddTotalPriceRule(userId, 1, category1, 5);
            List<int> rules = [rule1, rule2];
            int composite = marketManagerFacade.AddCompositeRule(userId, 1, and_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddPurchasePolicy(userId, 1, expirationDate, category1, composite);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 5);
            Assert.ThrowsException<Exception>(() => marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails));
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 0);
        }
        

        [TestMethod]
        public void PurchaseCart_composite_rule__or_Success()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            int rule2 = marketManagerFacade.AddTotalPriceRule(userId, 1, category1, 5);
            List<int> rules = [rule1, rule2];
            int composite = marketManagerFacade.AddCompositeRule(userId, 1, or_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddPurchasePolicy(userId, 1, expirationDate, category1, composite);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 2);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 1);
        }

        [TestMethod]
        public void PurchaseCart_composite_rule__and_or_AllFalse()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            int rule2 = marketManagerFacade.AddTotalPriceRule(userId, 1, category1, 100);
            List<int> rules = [rule1, rule2];
            int composite = marketManagerFacade.AddCompositeRule(userId, 1, or_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddPurchasePolicy(userId, 1, expirationDate, category1, composite);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 5);
            Assert.ThrowsException<Exception>(() => marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails));
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 0);
        }

        [TestMethod]
        public void PurchaseCart_composite_rule__xor_Success1()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            int rule2 = marketManagerFacade.AddTotalPriceRule(userId, 1, category1, 5);
            List<int> rules = [rule1, rule2];
            int composite = marketManagerFacade.AddCompositeRule(userId, 1, xor_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddPurchasePolicy(userId, 1, expirationDate, category1, composite);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 5);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 1);
        }

        [TestMethod]
        public void PurchaseCart_composite_rule__xor_Success2()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            int rule2 = marketManagerFacade.AddTotalPriceRule(userId, 1, category1, 100);
            List<int> rules = [rule1, rule2];
            int composite = marketManagerFacade.AddCompositeRule(userId, 1, xor_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddPurchasePolicy(userId, 1, expirationDate, category1, composite);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 2);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 1);
        }

        [TestMethod]
        public void PurchaseCart_composite_rule__and_xor_AllFalse()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            int rule2 = marketManagerFacade.AddTotalPriceRule(userId, 1, category1, 5);
            List<int> rules = [rule1, rule2];
            int composite = marketManagerFacade.AddCompositeRule(userId, 1, xor_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddPurchasePolicy(userId, 1, expirationDate, category1, composite);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 2);
            Assert.ThrowsException<Exception>(() => marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails));
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 0);
        }

        [TestMethod]
        public void PurchaseCart_Discount_Role__category_Success()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddDiscountPolicy(userId, 1, expirationDate, category1, rule1, 0.5);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 2);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 1);
            Assert.IsTrue(store._history._purchases[0].Price == 10);
        }

        [TestMethod]
        public void PurchaseCart_Discount_Role__category_Fail()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddDiscountPolicy(userId, 1, expirationDate, category1, rule1, 0.5);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 5);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 1);
            Assert.IsTrue(store._history._purchases[0].Price == 35);
        }

        [TestMethod]
        public void PurchaseCart_Discount_rule__and_Success()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            int rule2 = marketManagerFacade.AddTotalPriceRule(userId, 1, category1, 5);
            List<int> rules = [rule1, rule2];
            int composite = marketManagerFacade.AddCompositeRule(userId, 1, and_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddPurchasePolicy(userId, 1, expirationDate, category1, composite);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 2);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 1);
            Assert.IsTrue(store._history._purchases[0].Price == 20);
        }

        [TestMethod]
        public void PurchaseCart_Discount_rule__and_Fail_OneNotTrue()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            int rule2 = marketManagerFacade.AddTotalPriceRule(userId, 1, category1, 5);
            List<int> rules = [rule1, rule2];
            int composite = marketManagerFacade.AddCompositeRule(userId, 1, and_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddDiscountPolicy(userId, 1, expirationDate, category1, composite, 0.5);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 5);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 1);
            Assert.IsTrue(store._history._purchases[0].Price == 35);
        }

        [TestMethod]
        public void PurchaseCart_Discount_rule__or_Success()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            int rule2 = marketManagerFacade.AddTotalPriceRule(userId, 1, category1, 5);
            List<int> rules = [rule1, rule2];
            int composite = marketManagerFacade.AddCompositeRule(userId, 1, or_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddDiscountPolicy(userId, 1, expirationDate, category1, composite, 0.5);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 2);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 1);
            Assert.IsTrue(store._history._purchases[0].Price == 10);
        }

        [TestMethod]
        public void PurchaseCart_Discount_rule__and_or_AllFalse()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            int rule2 = marketManagerFacade.AddTotalPriceRule(userId, 1, category1, 100);
            List<int> rules = [rule1, rule2];
            int composite = marketManagerFacade.AddCompositeRule(userId, 1, or_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddDiscountPolicy(userId, 1, expirationDate, category1, composite, 0.5);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 5);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 1);
            Assert.IsTrue(store._history._purchases[0].Price == 35);
        }

        [TestMethod]
        public void PurchaseCart_Discount_rule__xor_Success1()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            int rule2 = marketManagerFacade.AddTotalPriceRule(userId, 1, category1, 5);
            List<int> rules = [rule1, rule2];
            int composite = marketManagerFacade.AddCompositeRule(userId, 1, xor_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddDiscountPolicy(userId, 1, expirationDate, category1, composite, 0.5);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 5);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 1);
            Assert.IsTrue(store._history._purchases[0].Price == 17.5);
        }

        [TestMethod]
        public void PurchaseCart_Discount_rule__xor_Success2()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            int rule2 = marketManagerFacade.AddTotalPriceRule(userId, 1, category1, 100);
            List<int> rules = [rule1, rule2];
            int composite = marketManagerFacade.AddCompositeRule(userId, 1, xor_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddDiscountPolicy(userId, 1, expirationDate, category1, composite, 0.5);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 2);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 1);
            Assert.IsTrue(store._history._purchases[0].Price == 10);
        }

        [TestMethod]
        public void PurchaseCart_Discount_rule__and_xor_AllFalse()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            int rule2 = marketManagerFacade.AddTotalPriceRule(userId, 1, category1, 5);
            List<int> rules = [rule1, rule2];
            int composite = marketManagerFacade.AddCompositeRule(userId, 1, xor_operator, rules);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddDiscountPolicy(userId, 1, expirationDate, category1, composite, 0.5);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 2);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 1);
            Assert.IsTrue(store._history._purchases[0].Price == 20);
        }

        [TestMethod]
        public void PurchaseCart_Discount_Success_Purchase_fail()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            int rule2 = marketManagerFacade.AddTotalPriceRule(userId, 1, category1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddPurchasePolicy(userId, 1, expirationDate, category1, rule1);
            marketManagerFacade.AddDiscountPolicy(userId, 1, expirationDate, category1, rule2, 0.5);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 5);
            Assert.ThrowsException<Exception>(() => marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails));
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 0);
        }

        [TestMethod]
        public void PurchaseCart_Discount_Fail_Purchase_Success()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            int rule2 = marketManagerFacade.AddTotalPriceRule(userId, 1, category1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddPurchasePolicy(userId, 1, expirationDate, category1, rule2);
            marketManagerFacade.AddDiscountPolicy(userId, 1, expirationDate, category1, rule1, 0.5);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 5);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 1);
            Assert.IsTrue(store._history._purchases[0].Price == 35);
        }

        [TestMethod]
        public void PurchaseCart_Discount_Success_Purchase_Success()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule2 = marketManagerFacade.AddTotalPriceRule(userId, 1, category1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            marketManagerFacade.AddPurchasePolicy(userId, 1, expirationDate, category1, rule2);
            marketManagerFacade.AddDiscountPolicy(userId, 1, expirationDate, category1, rule2, 0.5);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 5);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 1);
            Assert.IsTrue(store._history._purchases[0].Price == 17.5);
        }

        [TestMethod]
        public void PurchaseCart_Composite_Policies_add()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddTotalPriceRule(userId, 1, category1, 10);
            int rule2 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            int policy1 = marketManagerFacade.AddDiscountPolicy(userId, 1, expirationDate, category1, rule2, 0.5);
            int policy2 = marketManagerFacade.AddDiscountPolicy(userId, 1, expirationDate, category1, rule1, 0.1);
            List<int> policies = [policy1, policy2];
            marketManagerFacade.AddCompositePolicy(userId, 1, expirationDate, category1, 0, policies);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 2);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 1);
            Assert.IsTrue(store._history._purchases[0].Price == 8);
        }

        [TestMethod]
        public void PurchaseCart_Composite_Policies_max()
        {
            marketManagerFacade.AddProduct(1, userId, "apple", "RegularSell", "nice", 5, category1, 200, false);
            int rule1 = marketManagerFacade.AddTotalPriceRule(userId, 1, category1, 10);
            int rule2 = marketManagerFacade.AddQuantityRule(userId, 1, category1, 1, 5);
            DateTime expirationDate = DateTime.Now.AddDays(2);
            int policy1 = marketManagerFacade.AddDiscountPolicy(userId, 1, expirationDate, category1, rule2, 0.5);
            int policy2 = marketManagerFacade.AddDiscountPolicy(userId, 1, expirationDate, category1, rule1, 0.1);
            List<int> policies = [policy1, policy2];
            marketManagerFacade.AddCompositePolicy(userId, 1, expirationDate, category1, 1, policies);
            marketManagerFacade.AddToCart(userId, 1, 12, 2);
            marketManagerFacade.AddToCart(userId, 1, 11, 2);
            marketManagerFacade.PurchaseCart(userId, paymentDetails, shippingDetails);
            Store store = marketManagerFacade.GetStore(1);
            Assert.IsTrue(store._history._purchases.Count == 1);
            Assert.IsTrue(store._history._purchases[0].Price == 10);
        }
    }
}