using System.IO.Compression;
using MarketBackend.DAL.DTO;
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
        PaymentDetails paymentDetails = new PaymentDetails("ILS", "5326888878675678", "2027", "10", "101", "3190876789", "Hadas");
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
        public async void Setup(){
            DBcontext.GetInstance().Dispose();
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
            await proxy.EnterAsGuest(session1);
            await proxy.Register(userName, userPassword, email1, userAge);
            token1 = await proxy.LoginWithToken(userName, userPassword);
            userId = await proxy.GetMembeIDrByUserName(userName);
            int userId2 = proxy.GetUserId();
            await proxy.EnterAsGuest(session2);
            await proxy.Register(userName2, pass2, email2, userAge);
            token2 = await proxy.LoginWithToken(userName2, pass2);
            userId2 = await proxy.GetMembeIDrByUserName(userName2);
            await proxy.CreateStore(token1, storeName, storeEmail, phoneNum);
            await proxy.AddProduct(1, token1, productName1, sellmethod, desc, price1, category1, quantity1, false);
        }

        [TestCleanup]
        public void CleanUp(){
            DBcontext.GetInstance().Dispose();
            proxy.Dispose();
        }

        [TestMethod]
        public async void AddSimpleRuleStoreName_Success(){
            Assert.IsTrue(await proxy.AddSimpleRule(token1, 1, storeName), 
                "Fail in adding rule, need not to throw exception.");
            Assert.AreEqual(1, (await proxy.GetStoreRules(1, token1)).Count, 
                $"Expected 1 rule(s) in the store, but found {(await proxy.GetStoreRules(1, token1)).Count}.");
        }

        [TestMethod]
        public async void AddSimpleRuleStoreName_FailNoStoreOrProductName(){
            Assert.IsFalse(await proxy.AddSimpleRule(token1, 1, "bread"),
                "Fail in adding rule, need to throw exception.");
            Assert.AreEqual(0, (await proxy.GetStoreRules(1, token1)).Count, 
                $"Expected 0 rules in the store, but found {(await proxy.GetStoreRules(1, token1)).Count}.");
        }

        // [TestMethod]
        // public void AddSimpleRuleProduct_Success(){
        //     Assert.IsTrue(proxy.AddSimpleRule(token1, 1, productName1));
        //     Assert.AreEqual(1, proxy.GetStoreRules(1, token1).Count);
        // }

        [TestMethod]
        public async void AddQuantityRuleProduct_Success(){
            Assert.IsTrue(await proxy.AddQuantityRule(token1, 1, productName1, 1, 10),
                "Fail in adding rule, need not to throw exception.");
            Assert.AreEqual(1, (await proxy.GetStoreRules(1, token1)).Count, 
                $"Expected 1 rule(s) in the store, but found {(await proxy.GetStoreRules(1, token1)).Count}.");
        }

        [TestMethod]
        public async void AddQuantityRuleProduct_FailNegativeQuantity(){
            Assert.IsFalse(await proxy.AddQuantityRule(token1, 1, productName1, 1, -10),
                "Fail in adding rule, need to throw exception.");
            Assert.AreEqual(0, (await proxy.GetStoreRules(1, token1)).Count, 
                $"Expected 0 rules in the store, but found {(await proxy.GetStoreRules(1, token1)).Count}.");
        }

        [TestMethod]
        public async void AddTotalPriceRuleProduct_Success(){
            Assert.IsTrue(await proxy.AddTotalPriceRule(token1, 1, productName1, 10),
                "Fail in adding rule, need not to throw exception.");
            Assert.AreEqual(1, (await proxy.GetStoreRules(1, token1)).Count, 
                $"Expected 1 rule(s) in the store, but found {(await proxy.GetStoreRules(1, token1)).Count}.");
        }

        [TestMethod]
        public async void AddTotalPriceRuleProduct_FailNegativePrice(){
            Assert.IsFalse(await proxy.AddTotalPriceRule(token1, 1, productName1, -10));
            Assert.AreEqual(0, (await proxy.GetStoreRules(1, token1)).Count, 
                $"Expected 0 rules in the store, but found {(await proxy.GetStoreRules(1, token1)).Count}.");
        }

        // [TestMethod]
        // public void RunMultyTimes()
        // {
        //     for (int i = 0; i< 5; i++){
        //         AddSimpleRuleStoreName_Success();
        //         CleanUp();
        //         Setup();
        //         AddSimpleRuleStoreName_FailNoStoreOrProductName();
        //         CleanUp();
        //         Setup();
        //         AddQuantityRuleProduct_Success();
        //         CleanUp();
        //         Setup();
        //         AddQuantityRuleProduct_FailNegativeQuantity();
        //         CleanUp();
        //         Setup();
        //         AddTotalPriceRuleProduct_Success();
        //         CleanUp();
        //         Setup();
        //         AddTotalPriceRuleProduct_FailNegativePrice();
        //         CleanUp();
        //         Setup();
        //     }
        // }
    }
}
