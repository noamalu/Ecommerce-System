using System.IO.Compression;
using MarketBackend.DAL.DTO;
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
    public class StoreOwnerAT
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
        double price2 = 10.0;
        double negPrice = -10.0;
        int quantity1 = 10;
        int quantity2 = 20;
        int negQuantity = -10;
        double discount1 = 0.5;
        double discount2 = 0.3;  
        int userAge = 20;
        int userAge2 = 16;
        string storeName = "Rami Levi";
        string storeEmail = "RamiLevi@gmail.com";
        string phoneNum  = "0522458976";
        string sellmethod = "RegularSell";
        string desc = "nice";
        int shopID = 1;

        [TestInitialize()]
        public async void Setup(){
            DBcontext.GetInstance().Dispose();
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
            await proxy.EnterAsGuest(session1);
            await proxy.Register(userName, userPassword, email1, userAge);
            token1 = await proxy.LoginWithToken(userName, userPassword);
            userId = await proxy.GetMembeIDrByUserName(userName);
            await proxy.CreateStore(token1, storeName, storeEmail, phoneNum);
        }

        [TestCleanup]
        public void CleanUp(){
            DBcontext.GetInstance().Dispose();
            proxy.Dispose();
        }

        [TestMethod]
        public async void AddProductSuccess()
        {
            Assert.IsTrue(await proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false));
            Assert.IsTrue(await proxy.GetProductInfo(shopID, 11));
        }

        [TestMethod]
        public async void RemoveProductSuccess()
        {
            Assert.IsTrue(await proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false));
            Assert.IsTrue(await proxy.RemoveProduct(shopID, token1, 11));
            Assert.IsFalse(await proxy.GetProductInfo(shopID, 11));
        }

        [TestMethod]
        public async void RemoveProductFail_NoProduct()
        {
            Assert.IsFalse(await proxy.RemoveProduct(shopID, token1, 11));
        }

        [TestMethod]
        public async void UpdateProductPriceSuccess()
        {
            Assert.IsTrue(await proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false)); 
            Assert.IsTrue(await proxy.UpdateProductPrice(shopID, token1, productID1, price2));
            Assert.AreEqual(price2, (await proxy.GetProduct(shopID, productID1))._price, 
                $"Expected product price to be {price2}, but found {(await proxy.GetProduct(shopID, productID1))._price}.");     
        }

        [TestMethod]
        public async void UpdateProductPriceFail_NegativePrice()
        {
            Assert.IsTrue(await proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false)); 
            Assert.IsFalse(await proxy.UpdateProductPrice(shopID, token1, productID1, negPrice));
            Assert.AreEqual(price1, (await proxy.GetProduct(shopID, productID1))._price, 
                $"Expected product price to remain {price1}, but found {(await proxy.GetProduct(shopID, productID1))._price}.");     
        }

        [TestMethod]
        public async void UpdateProductPriceFail_NoProduct()
        {
            Assert.IsFalse(await proxy.UpdateProductPrice(shopID, token1, productID1, negPrice));     
        }

        [TestMethod]
        public async void UpdateProductQuantitySuccess()
        {
            Assert.IsTrue(await proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false)); 
            Assert.IsTrue(await proxy.UpdateProductQuantity(shopID, token1, productID1, quantity2));
            Assert.AreEqual(quantity2, (await proxy.GetProduct(shopID, productID1))._quantity, 
                $"Expected product quantity to be {quantity2}, but found {(await proxy.GetProduct(shopID, productID1))._quantity}.");      
        }

        [TestMethod]
        public async void UpdateProductQuantityFail_NegativeQuantity()
        {
            Assert.IsTrue(await proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false)); 
            Assert.IsFalse(await proxy.UpdateProductQuantity(shopID, token1, productID1, negQuantity));
            Assert.AreEqual(quantity1, (await proxy.GetProduct(shopID, productID1))._quantity, 
                $"Expected product quantity to remain {quantity1}, but found {(await proxy.GetProduct(shopID, productID1))._quantity}.");     
        }

        [TestMethod]
        public async void UpdateProductQuantityFail_NoProduct()
        {
            Assert.IsFalse(await proxy.UpdateProductQuantity(shopID, token1, productID1, negQuantity));     
        }

        [TestMethod]
        public async void GetPurchaseHistorySuccess()
        {
            Assert.IsTrue(await proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false));
            int userId2 = proxy.GetUserId();
            Assert.IsTrue(await proxy.EnterAsGuest(session2));
            Assert.IsTrue(await proxy.Register(userName2, pass2, email2, userAge));
            userId2 = await proxy.GetMembeIDrByUserName(userName2);
            token2 = await proxy.LoginWithToken(userName2, pass2);
            Assert.IsTrue(await proxy.AddToCart(token2, shopID, productID1, quantity1));
            PaymentDetails paymentDetails = new PaymentDetails("ILS", "5326888878675678", "2027", "10", "101", "3190876789", "Hadas");
            ShippingDetails shippingDetails = new ShippingDetails("name",  "city",  "address",  "country",  "zipcode");
            Assert.IsTrue(await proxy.PurchaseCart(token2, paymentDetails, shippingDetails));
            Assert.IsTrue(await proxy.GetPurchaseHistoryByClient(userName2));
            Assert.AreEqual(1, (await proxy.GetPurchaseHistory(userName2)).Count, 
                $"Expected purchase history count to be 1, but found {(await proxy.GetPurchaseHistory(userName2)).Count}.");
        }

        [TestMethod]
        public async void AddStaffMemberSuccess()
        {
            int userId2 = proxy.GetUserId();
            Assert.IsTrue(await proxy.EnterAsGuest(session2));
            Assert.IsTrue(await proxy.Register(userName2, pass2, email2, userAge));
            userId2 = await proxy.GetMembeIDrByUserName(userName2);
            token2 = await proxy.LoginWithToken(userName2, pass2);
            Assert.IsTrue(await proxy.AddOwner(token1, shopID, userName2));
            Assert.AreEqual(1, (await proxy.GetOwners(shopID)).Count, 
                $"Expected owner count to be 1, but found {(await proxy.GetOwners(shopID)).Count}.");
        }

        [TestMethod]
        public async void RemoveStaffMemberSuccess()
        {
            int userId2 = proxy.GetUserId();
            Assert.IsTrue(await proxy.EnterAsGuest(session2));
            Assert.IsTrue(await proxy.Register(userName2, pass2, email2, userAge));
            userId2 = await proxy.GetMembeIDrByUserName(userName2);
            token2 = await proxy.LoginWithToken(userName2, pass2);
            Assert.IsTrue(await proxy.AddOwner(token1, shopID, userName2));
            Assert.IsTrue(await proxy.RemoveStaffMember(shopID, token1, userName2));
            Assert.AreEqual(0, (await proxy.GetOwners(shopID)).Count, 
                $"Expected owner count to be 0, but found {(await proxy.GetOwners(shopID)).Count}.");
        }

        // [TestMethod]
        // public void RunMultyTimes(){
        //     for (int i=0; i<5; i++){
        //         AddProductSuccess();
        //         CleanUp();
        //         Setup();
        //         RemoveProductSuccess();
        //         CleanUp();
        //         Setup();
        //         RemoveProductFail_NoProduct();
        //         CleanUp();
        //         Setup();
        //         UpdateProductPriceSuccess();
        //         CleanUp();
        //         Setup();
        //         UpdateProductPriceFail_NegativePrice();
        //         CleanUp();
        //         Setup();
        //         UpdateProductPriceFail_NoProduct();
        //         CleanUp();
        //         Setup();
        //         UpdateProductQuantitySuccess();
        //         CleanUp();
        //         Setup();
        //         UpdateProductQuantityFail_NegativeQuantity();
        //         CleanUp();
        //         Setup();
        //         UpdateProductQuantityFail_NoProduct();
        //         CleanUp();
        //         Setup();
        //         GetPurchaseHistorySuccess();
        //         CleanUp();
        //         Setup();
        //         AddStaffMemberSuccess();
        //         CleanUp();
        //         Setup();
        //         RemoveStaffMemberSuccess();
        //         CleanUp();
        //         Setup();
        //     }
        // }
    }
}
