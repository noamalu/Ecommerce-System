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
    public class StoreOwnerAT
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
      string storeName = "Rami Levi";
      string storeEmail = "RamiLevi@gmail.com";
      string phoneNum  = "0522458976";
      string sellmethod = "RegularSell";
      string desc = "nice";

      

        [TestInitialize()]
        public void Setup(){
            proxy = new Proxy();
            userId = proxy.GetUserId();
            var mockShippingSystem = new Mock<IShippingSystemFacade>();
            var mockPaymentSystem = new Mock<IPaymentSystemFacade>();
            mockShippingSystem.SetReturnsDefault(true);
            mockPaymentSystem.SetReturnsDefault(true);

            

        }

        [TestCleanup]
        public void CleanUp(){
            proxy.Dispose();
        }


        [TestMethod]
        public void AddProductSuccess()
        {
           Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(1, userId, productName1, sellmethod, desc, price1, category1, quantity1, false));
        }

        [TestMethod]
        public void RemoveProductSuccess()
        {
           Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(1, userId, productName1, sellmethod, desc, price1, category1, quantity1, false));
           Assert.IsTrue(proxy.RemoveProduct(shopID, userId, productID1));
        }

        [TestMethod]
        public void RemoveProductFail_NoProduct()
        {
           Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           Assert.IsFalse(proxy.RemoveProduct(shopID, userId, productID1));
        }

        [TestMethod]
        public void UpdateProductPriceSuccess()
        {
           Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(1, userId, productName1, sellmethod, desc, price1, category1, quantity1, false)); 
           Assert.IsTrue(proxy.UpdateProductPrice(shopID, userId, productID1, price2));     
        }

        [TestMethod]
        public void UpdateProductPriceFail_NegativePrice()
        {
           Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(1, userId, productName1, sellmethod, desc, price1, category1, quantity1, false)); 
           Assert.IsFalse(proxy.UpdateProductPrice(shopID, userId, productID1, negPrice));     
        }

        [TestMethod]
        public void UpdateProductQuantitySuccess()
        {
           Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(1, userId, productName1, sellmethod, desc, price1, category1, quantity1, false)); 
           Assert.IsTrue(proxy.UpdateProductQuantity(shopID, userId, productID1, quantity2));     
        }

        [TestMethod]
        public void UpdateProductQuantityFail_NegativePrice()
        {
           Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(1, userId, productName1, sellmethod, desc, price1, category1, quantity1, false)); 
           Assert.IsFalse(proxy.UpdateProductQuantity(shopID, userId, productID1, negQuantity));     
        }

      //   [TestMethod]
      //   public void UpdateProductDiscountSuccess()
      //   {
      //      Assert.IsTrue(proxy.EnterAsGuest(userId));
      //      Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
      //      Assert.IsTrue(proxy.Login(userId, userName, userPassword));
      //      int shopID = 1;
      //      Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
      //      Assert.IsTrue(proxy.AddProduct(productID1, productName1, shopID, category1, price1, quantity1, discount1)); 
      //    //   Assert.IsTrue(proxy.UpdateProductDiscount(shopID, userId, productID1, discount2));     
      //   }

      //   [TestMethod]
      //   public void UpdateProductDiscountFail_NoProduct()
      //   {
      //      Assert.IsTrue(proxy.EnterAsGuest(userId));
      //      Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
      //      Assert.IsTrue(proxy.Login(userId, userName, userPassword));
      //      int shopID = 1;
      //      Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
      //    //   Assert.IsFalse(proxy.UpdateProductDiscount(shopID, userId, productID1, discount2));     
      //   }

        [TestMethod]
        public void GetPurchaseHistorySuccess()
        {
           Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           Assert.IsTrue(proxy.AddProduct(1, userId, productName1, sellmethod, desc, price1, category1, quantity1, false));
           int userId2 = proxy.GetUserId();
           Assert.IsTrue(proxy.EnterAsGuest(userId2));
           Assert.IsTrue(proxy.Register(userId2, userName2, pass2, email2, userAge));
           Assert.IsTrue(proxy.Login(userId2, userName2, pass2));
           Assert.IsTrue(proxy.AddToCart(userId2, shopID, productID1, quantity1));
           PaymentDetails paymentDetails = new PaymentDetails("5326888878675678", "2027", "10", "101", "3190876789", "Hadas");
           Assert.IsTrue(proxy.PurchaseCart(userId2, paymentDetails));
           Assert.IsTrue(proxy.GetPurchaseHistory(userId));
        }

        [TestMethod]
        public void AddStaffMemberSuccess()
        {
            Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           int userId2 = proxy.GetUserId();
           Assert.IsTrue(proxy.EnterAsGuest(userId2));
           Assert.IsTrue(proxy.Register(userId2, userName2, pass2, email2, userAge));
           Assert.IsTrue(proxy.Login(userId2, userName2, pass2));
           //add role and add staff member
        }

        [TestMethod]
        public void RemoveStaffMemberSuccess()
        {
            Assert.IsTrue(proxy.EnterAsGuest(userId));
           Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
           Assert.IsTrue(proxy.Login(userId, userName, userPassword));
           int shopID = 1;
           Assert.IsTrue(proxy.CreateStore(userId, storeName, storeEmail, phoneNum));
           int userId2 = proxy.GetUserId();
           Assert.IsTrue(proxy.EnterAsGuest(userId2));
           Assert.IsTrue(proxy.Register(userId2, userName2, pass2, email2, userAge));
           Assert.IsTrue(proxy.Login(userId2, userName2, pass2));
           //add role and remove staff member
        }

        
    }
}