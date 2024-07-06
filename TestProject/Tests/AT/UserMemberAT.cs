using System.IO.Compression;
using MarketBackend.DAL.DTO;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Payment;
using MarketBackend.Domain.Shipping;
using MarketBackend.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MarketBackend.Tests.AT
{
    [TestClass()]
    public class UserMemberAT
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
        }

        [TestCleanup]
        public void CleanUp(){
            DBcontext.GetInstance().Dispose();
            proxy.Dispose();
        }

        [TestMethod]
        public async void LogOutSuccess(){
            Assert.IsTrue(await proxy.LogOut(token1), "Expected logout to succeed for logged-in user.");
        }

        [TestMethod]
        public async void LogOutFail_NotLoggedIn(){
            await proxy.LogOut(token1);
            Assert.IsFalse(await proxy.LogOut(token1), "Expected logout to fail for a user that is not logged in.");
        }

        [TestMethod]
        public void GetInfo(){
            // Implement the test logic for GetInfo if needed
        }

        [TestMethod]
        public async void CreateShopSuccess()
        {
            Assert.IsTrue(await proxy.CreateStore(token1, storeName, storeEmail, phoneNum), 
                "Expected store creation to succeed.");
            Assert.AreEqual(storeName, await proxy.GetStoreById(1), 
                $"Expected store name to be {storeName} but got {proxy.GetStoreById(1)}.");
        }

        [TestMethod]
        public async void CreateShopFail_NotLoggedIn()
        {
            await proxy.LogOut(token1);
            Assert.IsFalse(await proxy.CreateStore(token1, storeName, storeEmail, phoneNum), 
                "Expected store creation to fail for a user that is not logged in.");
        }

        [TestMethod]
        public async void GetInfoSuccess()
        {
            int shopID = 1;
            Assert.IsTrue(await proxy.CreateStore(token1, storeName, storeEmail, phoneNum), 
                "Expected store creation to succeed.");
            Assert.IsTrue(await proxy.GetInfo(shopID), 
                "Expected to retrieve information for the created store.");
        }

        [TestMethod]
        public async void AddToCartSuccess()
        {
            int shopID = 1;
            Assert.IsTrue(await proxy.CreateStore(token1, storeName, storeEmail, phoneNum), 
                "Expected store creation to succeed.");
            Assert.IsTrue(await proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false), 
                "Expected product addition to the store to succeed.");
            userId2 = await proxy.GetMembeIDrByUserName(userName2);
            Assert.IsTrue(await proxy.AddToCart(token2, shopID, productID1, quantity1), 
                "Expected product to be added to the cart successfully.");
            // Assert.AreEqual(1, (await proxy.GetMember(userName2)).Cart.GetBaskets().Count, 
            //     $"Expected cart to contain one basket but got {(await proxy.GetMember(userName2)).Cart.GetBaskets().Count}.");
        }

        [TestMethod]
        public async void AddToCartFail_NoProduct()
        {
            int shopID = 1;
            Assert.IsTrue(await proxy.CreateStore(token1, storeName, storeEmail, phoneNum), 
                "Expected store creation to succeed.");
            userId2 = await proxy.GetMembeIDrByUserName(userName2);
            Assert.IsFalse(await proxy.AddToCart(token2, shopID, productID1, quantity1), 
                "Expected adding product to cart to fail as product does not exist.");
            // Assert.AreEqual(0, (await proxy.GetMember(userName2)).Cart.GetBaskets().Count, 
            //     $"Expected cart to be empty but got basket count: {(await proxy.GetMember(userName2)).Cart.GetBaskets().Count}.");
        }

        [TestMethod]
        public async void RemoveFromCartSuccess()
        {
            int shopID = 1;
            Assert.IsTrue(await proxy.CreateStore(token1, storeName, storeEmail, phoneNum), 
                "Expected store creation to succeed.");
            Assert.IsTrue(await proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false), 
                "Expected product addition to the store to succeed.");
            userId2 = await proxy.GetMembeIDrByUserName(userName2);
            Assert.IsTrue(await proxy.AddToCart(token2, shopID, productID1, 1), 
                "Expected product to be added to the cart successfully.");
            Assert.IsTrue(await proxy.RemoveFromCart(token2, productID1, shopID, 1), 
                "Expected product to be removed from the cart successfully.");
            Member mem = await proxy.GetMember(userName2);
            Assert.AreEqual(0, (await mem.Cart.GetBaskets())[shopID].products.Count, 
                $"Expected cart to be empty after removing the product but got {(await mem.Cart.GetBaskets())[shopID].products.Count}.");
        }

        [TestMethod]
        public async void RemoveFromCartFail_NoProduct()
        {
            int shopID = 1;
            Assert.IsTrue(await proxy.CreateStore(token1, storeName, storeEmail, phoneNum), 
                "Expected store creation to succeed.");
            Assert.IsTrue(await proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false), 
                "Expected product addition to the store to succeed.");
            userId2 = await proxy.GetMembeIDrByUserName(userName2);
            Assert.IsFalse(await proxy.RemoveFromCart(token2, productID1, shopID, 1), 
                "Expected removing product from cart to fail as product does not exist in the cart.");
        }

        [TestMethod]
        public async void PurchaseCartSuccess()
        {
            int shopID = 1;
            Assert.IsTrue(await proxy.CreateStore(token1, storeName, storeEmail, phoneNum), 
                "Expected store creation to succeed.");
            Assert.IsTrue(await proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false), 
                "Expected product addition to the store to succeed.");
            userId2 = await proxy.GetMembeIDrByUserName(userName2);
            Assert.IsTrue(await proxy.AddToCart(token2, shopID, productID1, quantity1), 
                "Expected product to be added to the cart successfully.");
            Assert.IsTrue(await proxy.PurchaseCart(token2, paymentDetails, shippingDetails), 
                "Expected cart purchase to succeed.");
            Assert.AreEqual(1, (await proxy.GetPurchaseHistory(userName2)).Count, 
                $"Expected purchase history to contain one entry but got {(await proxy.GetPurchaseHistory(userName2)).Count}.");
        }

        [TestMethod]
        public async void PurchaseCartFail_NoProduct()
        {
            int shopID = 1;
            Assert.IsTrue(await proxy.CreateStore(token1, storeName, storeEmail, phoneNum), 
                "Expected store creation to succeed.");
            Assert.IsTrue(await proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false), 
                "Expected product addition to the store to succeed.");
            userId2 = await proxy.GetMembeIDrByUserName(userName2);
            Assert.IsTrue(await proxy.AddToCart(token2, shopID, productID1, quantity1), 
                "Expected product to be added to the cart successfully.");
            Assert.IsTrue(await proxy.RemoveProduct(shopID, token1, 11), 
                "Expected product to be removed from the store.");
            Assert.IsFalse(await proxy.PurchaseCart(token2, paymentDetails, shippingDetails), 
                "Expected cart purchase to fail as product does not exist in the store.");
            Assert.AreEqual(0, (await proxy.GetPurchaseHistory(userName2)).Count, 
                $"Expected purchase history to be empty but got {(await proxy.GetPurchaseHistory(userName2)).Count}.");
        }

        [TestMethod]
        public async void PurchaseCartFail_EmptyCart()
        {
            int shopID = 1;
            Assert.IsTrue(await proxy.CreateStore(token1, storeName, storeEmail, phoneNum), 
                "Expected store creation to succeed.");
            Assert.IsTrue(await proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false), 
                "Expected product addition to the store to succeed.");
            userId2 = await proxy.GetMembeIDrByUserName(userName2);
            Assert.IsFalse(await proxy.PurchaseCart(token2, paymentDetails, shippingDetails), 
                "Expected cart purchase to fail as cart is empty.");
            Assert.AreEqual(0, (await proxy.GetPurchaseHistory(userName2)).Count, 
                $"Expected purchase history to be empty but got {(await proxy.GetPurchaseHistory(userName2)).Count}.");
        }

        [TestMethod]
        public async void PurchaseCartFail_IlegalAge()
        {
            int shopID = 1;
            Assert.IsTrue(await proxy.CreateStore(token1, storeName, storeEmail, phoneNum), 
                "Expected store creation to succeed.");
            Assert.IsTrue(await proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, true), 
                "Expected product addition to the store to succeed.");
            userId2 = await proxy.GetMembeIDrByUserName(userName2);
            Assert.IsFalse(await proxy.PurchaseCart(token2, paymentDetails, shippingDetails), 
                "Expected cart purchase to fail due to illegal age for purchasing the product.");
            Assert.AreEqual(0, (await proxy.GetPurchaseHistory(userName2)).Count, 
                $"Expected purchase history to be empty but got {(await proxy.GetPurchaseHistory(userName2)).Count}.");
        }

        [TestMethod]
        public async void PurchaseCartFail_Payment(){
            int shopID = 1;
            Assert.IsTrue(await proxy.CreateStore(token1, storeName, storeEmail, phoneNum), 
                "Expected store creation to succeed.");
            Assert.IsTrue(await proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false), 
                "Expected product addition to the store to succeed.");
            userId2 = await proxy.GetMembeIDrByUserName(userName2);
            mockPaymentSystem.SetReturnsDefault(false);
            Assert.IsFalse(await proxy.PurchaseCart(token2, paymentDetails, shippingDetails), 
                "Expected cart purchase to fail due to payment failure.");
            Assert.AreEqual(0, (await proxy.GetPurchaseHistory(userName2)).Count, 
                $"Expected purchase history to be empty but got {(await proxy.GetPurchaseHistory(userName2)).Count}.");
        }

        [TestMethod]
        public async void PurchaseCartFail_Shipping(){
            int shopID = 1;
            Assert.IsTrue(await proxy.CreateStore(token1, storeName, storeEmail, phoneNum), 
                "Expected store creation to succeed.");
            Assert.IsTrue(await proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false), 
                "Expected product addition to the store to succeed.");
            userId2 = await proxy.GetMembeIDrByUserName(userName2);
            mockShippingSystem.SetReturnsDefault(false);
            Assert.IsFalse(await proxy.PurchaseCart(token2, paymentDetails, shippingDetails), 
                "Expected cart purchase to fail due to shipping failure.");
            Assert.AreEqual(0, (await proxy.GetPurchaseHistory(userName2)).Count, 
                $"Expected purchase history to be empty but got {(await proxy.GetPurchaseHistory(userName2)).Count}.");
        }

        [TestMethod]
        public async void GetPurchaseHistorySuccess_Permission()
        {
            int shopID = 1;
            Assert.IsTrue(await proxy.CreateStore(token1, storeName, storeEmail, phoneNum), 
                "Expected store creation to succeed.");
            Assert.IsTrue(await proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false), 
                "Expected product addition to the store to succeed.");
            userId2 = await proxy.GetMembeIDrByUserName(userName2);
            Assert.IsTrue(await proxy.AddToCart(token2, shopID, productID1, quantity1), 
                "Expected product to be added to the cart successfully.");
            Assert.IsTrue(await proxy.PurchaseCart(token2, paymentDetails, shippingDetails), 
                "Expected cart purchase to succeed.");
            Assert.IsTrue(await proxy.AddOwner(token1, 1, userName2), 
                "Expected adding owner to succeed.");
            Assert.IsTrue(await proxy.GetPurchaseHistory(shopID, token2), 
                "Expected to retrieve purchase history with valid permission.");
            Assert.AreEqual(1, (await proxy.GetPurchaseHistory(userName2)).Count, 
                $"Expected purchase history to contain one entry but got {(await proxy.GetPurchaseHistory(userName2)).Count}.");
        }

        [TestMethod]
        public async void GetPurchaseHistoryFail_Permission()
        {
            int shopID = 1;
            Assert.IsTrue(await proxy.CreateStore(token1, storeName, storeEmail, phoneNum), 
                "Expected store creation to succeed.");
            Assert.IsTrue(await proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false), 
                "Expected product addition to the store to succeed.");
            userId2 = await proxy.GetMembeIDrByUserName(userName2);
            Assert.IsTrue(await proxy.AddToCart(token2, shopID, productID1, quantity1), 
                "Expected product to be added to the cart successfully.");
            Assert.IsTrue(await proxy.PurchaseCart(token2, paymentDetails, shippingDetails), 
                "Expected cart purchase to succeed.");
            Assert.IsFalse(await proxy.GetPurchaseHistory(shopID, token2), 
                "Expected retrieving purchase history to fail due to lack of permission.");
        }

        // [TestMethod]
        // public void RunMultyTimes()
        // {
        //     for (int i=0; i<5; i++){
        //         LogOutSuccess();
        //         CleanUp();
        //         Setup();
        //         LogOutFail_NotLoggedIn();
        //         CleanUp();
        //         Setup();
        //         CreateShopSuccess();
        //         CleanUp();
        //         Setup();
        //         CreateShopFail_NotLoggedIn();
        //         CleanUp();
        //         Setup();
        //         GetInfoSuccess();
        //         CleanUp();
        //         Setup();
        //         AddToCartSuccess();
        //         CleanUp();
        //         Setup();
        //         AddToCartFail_NoProduct();
        //         CleanUp();
        //         Setup();
        //         RemoveFromCartSuccess();
        //         CleanUp();
        //         Setup();
        //         RemoveFromCartFail_NoProduct();
        //         CleanUp();
        //         Setup();
        //         PurchaseCartSuccess();
        //         CleanUp();
        //         Setup();
        //         PurchaseCartFail_NoProduct();
        //         CleanUp();
        //         Setup();
        //         PurchaseCartFail_EmptyCart();
        //         CleanUp();
        //         Setup();
        //         PurchaseCartFail_IlegalAge();
        //         CleanUp();
        //         Setup();
        //         PurchaseCartFail_IlegalAge();
        //         CleanUp();
        //         Setup();
        //         PurchaseCartFail_Payment();
        //         CleanUp();
        //         Setup();
        //         PurchaseCartFail_Shipping();
        //         CleanUp();
        //         Setup();
        //         GetPurchaseHistorySuccess_Permission();
        //         CleanUp();
        //         Setup();
        //         GetPurchaseHistoryFail_Permission();
        //         CleanUp();
        //         Setup();
        //     }
        // }

        // [TestMethod]
        // public void SearchByKeyWords(){
        //    int shopID = 1;
        //    Assert.IsTrue(proxy.CreateStore(token1, storeName, storeEmail, phoneNum));
        //    Assert.IsTrue(proxy.AddProduct(shopID, token1, productName1, sellmethod, desc, price1, category1, quantity1, false));
        //    Assert.IsTrue(proxy.AddKeyWord(token1, "nice", shopID, 11));
        //    Assert.IsTrue(proxy.SearchByKeywords("nice"));
        //    Assert.AreEqual(1, proxy.SearchByKey("nice").Count);
        // }
    }
}
