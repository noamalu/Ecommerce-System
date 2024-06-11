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
        int userAge = 20;
        int userAge2 = 16;

        [TestInitialize()]
        public void Setup(){
            proxy = new Proxy();
            userId = proxy.GetUserId();
            proxy.InitiateSystemAdmin();
        }

        [TestCleanup]
        public void CleanUp(){
            proxy.Dispose();
            proxy.ExitGuest(session1);
        }

        [TestMethod]
        public void EnterAsGuestSuccess(){
            Assert.IsFalse(proxy.Login(userName, userPassword));
            Assert.IsTrue(proxy.Register(userName, userPassword, email1, userAge));
        }

        [TestMethod]
        public void RegisterSuccess(){
            Assert.IsTrue(proxy.EnterAsGuest(session1));
            Assert.IsTrue(proxy.Register(userName, userPassword, email1, userAge));
        }

        [TestMethod]
        public void RegisterFail_RegisterTwice(){
            Assert.IsTrue(proxy.EnterAsGuest(session1));
            Assert.IsTrue(proxy.Register(userName, userPassword, email1, userAge));
            Assert.IsFalse(proxy.Register(userName, userPassword, email1, userAge));
        }

        [TestMethod]
        public void RegisterFail_WrongEmail(){
            Assert.IsTrue(proxy.EnterAsGuest(session1));
            Assert.IsFalse(proxy.Register(userName, userPassword, wrongEmail, userAge));
        }

        [TestMethod]
        public void LoginSuccess(){
            Assert.IsTrue(proxy.EnterAsGuest(session1));
            Assert.IsTrue(proxy.Register(userName, userPassword, email1, userAge));
            Assert.IsTrue(proxy.Login(userName, userPassword));
        }

        [TestMethod]
        public void LoginFail_NotRegister(){
            Assert.IsTrue(proxy.EnterAsGuest(session1));
            Assert.IsFalse(proxy.Login(userName, userPassword));
        }

        [TestMethod]
        public void LoginFail_WrongUserName(){
            Assert.IsTrue(proxy.EnterAsGuest(session1));
            Assert.IsTrue(proxy.Register(userName, userPassword, email1, userAge));
            Assert.IsFalse(proxy.Login(userName2, userPassword));
        }

        [TestMethod]
        public void LoginFail_WrongPassword(){
            Assert.IsTrue(proxy.EnterAsGuest(session1));
            Assert.IsTrue(proxy.Register(userName, userPassword, email1, userAge));
            Assert.IsFalse(proxy.Login(userName, pass2));
        }

        [TestMethod]
        public void LogOutSuccess(){
            Assert.IsTrue(proxy.EnterAsGuest(session1));
            Assert.IsTrue(proxy.Register(userName, userPassword, email1, userAge));
            string token1 = proxy.LoginWithToken(userName, userPassword);
            proxy.GetMembeIDrByUserName(userName);
            Assert.IsTrue(proxy.LogOut(token1));
        }

        [TestMethod]
        public void LogOutFail_NotLoggedIn(){
            Assert.IsTrue(proxy.EnterAsGuest(session1));
            Assert.IsTrue(proxy.Register(userName, userPassword, email1, userAge));
            Assert.IsFalse(proxy.LogOut(session1));
        }
    }
}