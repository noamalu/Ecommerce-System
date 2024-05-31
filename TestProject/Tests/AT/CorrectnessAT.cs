using System.IO.Compression;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Payment;
using MarketBackend.Domain.Shipping;
using MarketBackend.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog;

namespace MarketBackend.Tests.AT
{
    [TestClass()]
    public class CorrectnessAT
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
        int productID1 = 111;
        string productName1 = "Banana";
        string category1 = "Fruit";
        double price1 = 5.0;
        double price2 = 10.0;
        double negPrice = -10.0;
        int quantity1 = 10;
        int quantity2 = 20;
        int negQuantity = -10;
        double discount1 = 0.5;
        double discount2 = 0.3;  
        int userAge = 20;
        int userAge2 = 16;



        [TestInitialize()]
        public void Setup(){
            proxy = new Proxy();
            userId = proxy.GetUserId();
            var mockShippingSystem = new Mock<IShippingSystemFacade>();
            var mockPaymentSystem = new Mock<IPaymentSystemFacade>();
            mockPaymentSystem.Setup(pay =>pay.Connect()).Returns(true);
            mockShippingSystem.Setup(ship => ship.Connect()).Returns(true);
            mockPaymentSystem.Setup(pay =>pay.Pay(It.IsAny<PaymentDetails>(), It.IsAny<double>())).Returns(1);
            mockShippingSystem.Setup(ship =>ship.OrderShippment(It.IsAny<ShippingDetails>())).Returns(1);
            mockShippingSystem.SetReturnsDefault(true);
            mockPaymentSystem.SetReturnsDefault(true);
            proxy.InitiateSystemAdmin();
            proxy.EnterAsGuest(userId);
            proxy.Register(userId, userName, userPassword, email1, userAge);
            proxy.Login(userId, userName, userPassword);
        }

        [TestCleanup]
        public void CleanUp(){
            proxy.Dispose();
        }
 
        [TestMethod]
        public void UniqueUsername_GoodCase()
        {
            int userId2 = proxy.GetUserId();
            Assert.IsTrue(proxy.EnterAsGuest(userId2));
            Assert.IsTrue(proxy.Register(userId2, userName2, pass2, email2, userAge2));

        }

        [TestMethod]
        public void UniqueUsername_BadCase()
        {
            int userId2 = proxy.GetUserId();
            Assert.IsTrue(proxy.EnterAsGuest(userId2));
            Assert.IsFalse(proxy.Register(userId2, userName, userPassword, email1, userAge));
        }
    }
}