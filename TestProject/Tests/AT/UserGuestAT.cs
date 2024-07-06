using MarketBackend.DAL.DTO;
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
            DBcontext.GetInstance().Dispose();
            proxy = new Proxy();
            userId = proxy.GetUserId();
            proxy.InitiateSystemAdmin();
        }

        [TestCleanup]
        public async void CleanUp(){
            DBcontext.GetInstance().Dispose();
            proxy.Dispose();
            await proxy.ExitGuest(session1);
        }

        [TestMethod]
        public async void EnterAsGuestSuccess(){
            Assert.IsFalse(await proxy.Login(userName, userPassword), 
                "Expected login to fail for unregistered user.");
            Assert.IsTrue(await proxy.Register(userName, userPassword, email1, userAge), 
                "Expected registration to succeed.");
        }

        [TestMethod]
        public async void RegisterSuccess(){
            Assert.IsTrue(await proxy.EnterAsGuest(session1), 
                "Expected to enter as guest successfully.");
            Assert.IsTrue(await proxy.Register(userName, userPassword, email1, userAge), 
                "Expected registration to succeed.");
        }

        [TestMethod]
        public async void RegisterFail_RegisterTwice(){
            Assert.IsTrue(await proxy.EnterAsGuest(session1), 
                "Expected to enter as guest successfully.");
            Assert.IsTrue(await proxy.Register(userName, userPassword, email1, userAge), 
                "Expected first registration to succeed.");
            Assert.IsFalse(await proxy.Register(userName, userPassword, email1, userAge), 
                "Expected second registration attempt to fail.");
        }

        [TestMethod]
        public async void RegisterFail_WrongEmail(){
            Assert.IsTrue(await proxy.EnterAsGuest(session1), 
                "Expected to enter as guest successfully.");
            Assert.IsFalse(await proxy.Register(userName, userPassword, wrongEmail, userAge), 
                "Expected registration to fail with wrong email format.");
        }

        [TestMethod]
        public async void LoginSuccess(){
            Assert.IsTrue(await proxy.EnterAsGuest(session1), 
                "Expected to enter as guest successfully.");
            Assert.IsTrue(await proxy.Register(userName, userPassword, email1, userAge), 
                "Expected registration to succeed.");
            Assert.IsTrue(await proxy.Login(userName, userPassword), 
                "Expected login to succeed with correct credentials.");
        }

        [TestMethod]
        public async void LoginFail_NotRegister(){
            Assert.IsTrue(await proxy.EnterAsGuest(session1), 
                "Expected to enter as guest successfully.");
            Assert.IsFalse(await proxy.Login(userName, userPassword), 
                "Expected login to fail for unregistered user.");
        }

        [TestMethod]
        public async void LoginFail_WrongUserName(){
            Assert.IsTrue(await proxy.EnterAsGuest(session1), 
                "Expected to enter as guest successfully.");
            Assert.IsTrue(await proxy.Register(userName, userPassword, email1, userAge), 
                "Expected registration to succeed.");
            Assert.IsFalse(await proxy.Login(userName2, userPassword), 
                "Expected login to fail with wrong username.");
        }

        [TestMethod]
        public async void LoginFail_WrongPassword(){
            Assert.IsTrue(await proxy.EnterAsGuest(session1), 
                "Expected to enter as guest successfully.");
            Assert.IsTrue(await proxy.Register(userName, userPassword, email1, userAge), 
                "Expected registration to succeed.");
            Assert.IsFalse(await proxy.Login(userName, pass2), 
                "Expected login to fail with wrong password.");
        }

        [TestMethod]
        public async void LogOutSuccess(){
            Assert.IsTrue(await proxy.EnterAsGuest(session1), 
                "Expected to enter as guest successfully.");
            Assert.IsTrue(await proxy.Register(userName, userPassword, email1, userAge), 
                "Expected registration to succeed.");
            string token1 = await proxy.LoginWithToken(userName, userPassword);
            await proxy.GetMembeIDrByUserName(userName);
            Assert.IsTrue(await proxy.LogOut(token1), 
                "Expected logout to succeed for logged-in user.");
        }

        [TestMethod]
        public async void LogOutFail_NotLoggedIn(){
            Assert.IsTrue(await proxy.EnterAsGuest(session1), 
                "Expected to enter as guest successfully.");
            Assert.IsTrue(await proxy.Register(userName, userPassword, email1, userAge), 
                "Expected registration to succeed.");
            Assert.IsFalse(await proxy.LogOut(session1), 
                "Expected logout to fail for user not logged in.");
        }

        // [TestMethod]
        // public void RunMultyTimes()
        // {
        //     for (int i=0; i<5; i++){
        //         EnterAsGuestSuccess();
        //         CleanUp();
        //         Setup();
        //         RegisterSuccess();
        //         CleanUp();
        //         Setup();
        //         RegisterFail_RegisterTwice();
        //         CleanUp();
        //         Setup();
        //         RegisterFail_WrongEmail();
        //         CleanUp();
        //         Setup();
        //         LoginSuccess();
        //         CleanUp();
        //         Setup();
        //         LoginFail_NotRegister();
        //         CleanUp();
        //         Setup();
        //         LoginFail_WrongUserName();
        //         CleanUp();
        //         Setup();
        //         LoginFail_WrongPassword();
        //         CleanUp();
        //         Setup();
        //         LogOutSuccess();
        //         CleanUp();
        //         Setup();
        //         LogOutFail_NotLoggedIn();
        //         CleanUp();
        //         Setup();
        //     }
        // }
    }
}
