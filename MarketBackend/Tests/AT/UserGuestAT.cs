using MarketBackend.Domain.Payment;
using MarketBackend.Domain.Shipping;
using MarketBackend.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MarketBackend.Tests.AT
{
    [TestClass()]
    public class UserGuestAT
    {
        string userName = "user1";
        string userName2 = "user2";
        string userPassword = "pass1";
        string pass2 = "pass2";
        int userId;
        Proxy proxy;

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
        }

        [TestMethod]
        public void RegisterSuccess(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userName, userPassword));
        }

        [TestMethod]
        public void RegisterFail_RegisterTwice(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userName, userPassword));
            Assert.IsFalse(proxy.Register(userName, userPassword));
        }

        [TestMethod]
        public void LoginSuccess(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userName, userPassword));
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
            Assert.IsTrue(proxy.Register(userName, userPassword));
            Assert.IsFalse(proxy.Login(userId, userName2, userPassword));
        }

        [TestMethod]
        public void LoginFail_WrongPassword(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userName, userPassword));
            Assert.IsFalse(proxy.Login(userId, userName, pass2));
        }

        [TestMethod]
        public void LogOutSuccess(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userName, userPassword));
            Assert.IsFalse(proxy.Login(userId, userName, userPassword));
            Assert.IsTrue(proxy.LogOut(userId));
        }

        [TestMethod]
        public void LogOutFail_NotLoggedIn(){
            Assert.IsTrue(proxy.EnterAsGuest(userId));
            Assert.IsTrue(proxy.Register(userName, userPassword));
            Assert.IsFalse(proxy.LogOut(userId));
        }


    }
}