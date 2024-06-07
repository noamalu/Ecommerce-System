using System.IO.Compression;
using MarketBackend.Domain.Payment;
using MarketBackend.Domain.Shipping;
using MarketBackend.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace MarketBackend.Tests.AT
{
    [TestClass()]
    public class PoliciesAT
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
            proxy.EnterAsGuest(userId);
            proxy.Register(userId, userName, userPassword, email1, userAge);
            proxy.Login(userId, userName, userPassword);
            userId = proxy.GetMembeIDrByUserName(userName);
            int userId2 = proxy.GetUserId();
            proxy.EnterAsGuest(userId2);
            proxy.Register(userId2, userName2, pass2, email2, userAge);
            proxy.Login(userId2, userName2, pass2);
            userId2 = proxy.GetMembeIDrByUserName(userName2);
            proxy.CreateStore(userId, storeName, storeEmail, phoneNum);
            proxy.AddProduct(1, userId, productName1, sellmethod, desc, price1, category1, quantity1, false);
        }

        [TestCleanup]
        public void CleanUp(){
            proxy.Dispose();
        }

        [TestMethod]
        public void AddSimpleRuleStoreName_Success(){
            Assert.IsTrue(proxy.AddSimpleRule(userId, 1, storeName));
        }

        [TestMethod]
        public void AddSimpleRuleStoreName_FailNoStoreOrProductName(){
            Assert.IsFalse(proxy.AddSimpleRule(userId, 1, "bread"));
        }

        [TestMethod]
        public void AddSimpleRuleProduct_Success(){
            Assert.IsTrue(proxy.AddSimpleRule(userId, 1, productName1));
        }

        [TestMethod]
        public void AddQuantityRuleProduct_Success(){
            Assert.IsTrue(proxy.AddQuantityRule(userId, 1, productName1, 1, 10));
        }

        [TestMethod]
        public void AddQuantityRuleProduct_FailNegativeQuantity(){
            Assert.IsFalse(proxy.AddQuantityRule(userId, 1, productName1, 1, -10));
        }

        [TestMethod]
        public void AddTotalPriceRuleProduct_Success(){
            Assert.IsTrue(proxy.AddTotalPriceRule(userId, 1, productName1, 10));
        }

        [TestMethod]
        public void AddTotalPriceRuleProduct_FailNegativePrice(){
            Assert.IsFalse(proxy.AddTotalPriceRule(userId, 1, productName1, -10));
        }
    }
}