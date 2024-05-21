using MarketBackend.Domain.Payment;
using MarketBackend.Domain.Shipping;
using MarketBackend.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;

namespace MarketBackend.Tests.AT
{
    [TestClass()]
    public class UserGuestAT
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
        int userAge = 20;
        int userAge2 = 16;

        [TestInitialize()]
        public void Setup(){
            proxy = new Proxy();
            userId = proxy.GetUserId();
            
        }

        [TestCleanup]
        public void CleanUp(){
            proxy.Dispose();
        }

        [TestMethod]
        public void EnterAsGuestSuccess(){
            Assert.IsFalse(proxy.Login(userId, userName, userPassword));
            Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
        }

        [TestMethod]
        public void RegisterSuccess(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
        }

        [TestMethod]
        public void RegisterFail_RegisterTwice(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
            Assert.IsFalse(proxy.Register(userId, userName, userPassword, email1, userAge));
        }

        [TestMethod]
        public void RegisterFail_WrongEmail(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsFalse(proxy.Register(userId, userName, userPassword, wrongEmail, userAge));
        }

        [TestMethod]
        public void LoginSuccess(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
            Assert.IsTrue(proxy.Login(userId, userName, userPassword));
        }

        [TestMethod]
        public void LoginFail_NotRegister(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsFalse(proxy.Login(userId, userName, userPassword));
        }

        [TestMethod]
        public void LoginFail_WrongUserName(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
            Assert.IsFalse(proxy.Login(userId, userName2, userPassword));
        }

        [TestMethod]
        public void LoginFail_WrongPassword(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
            Assert.IsFalse(proxy.Login(userId, userName, pass2));
        }

        [TestMethod]
        public void LogOutSuccess(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
            Assert.IsFalse(proxy.Login(userId, userName, userPassword));
            Assert.IsTrue(proxy.LogOut(userId));
        }

        [TestMethod]
        public void LogOutFail_NotLoggedIn(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userId, userName, userPassword, email1, userAge));
            Assert.IsFalse(proxy.LogOut(userId));
        }


    }
}