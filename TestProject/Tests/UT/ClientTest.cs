using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using MarketBackend.Domain.Market_Client;
using MarketBackend.DAL;

namespace UnitTests
{
    [TestClass]
    public class ClientTest
    {
        [TestMethod]
        public void TestAddToCart()
        {
            var client = new Guest(1); // Testing with Guest as an example
            client.AddToCart(0, 100, 1); // basket, productId, quantity
            var productsInBasket = BasketRepositoryRAM.GetInstance().getBasketsByCartId(client.Cart._shoppingCartId).Where(basket => basket._storeId == 0).FirstOrDefault()?.products;
            Assert.AreEqual(1, productsInBasket?[100]);
        }

        [TestMethod]
        public void TestRemoveFromCart()
        {
            var client = new Guest(1);
            client.AddToCart(0, 100, 1);
            var basket = BasketRepositoryRAM.GetInstance().getBasketsByCartId(client.Cart._shoppingCartId).Where(basket => basket._storeId == 0).FirstOrDefault();
            client.RemoveFromCart(basket._basketId, 100, 1);
            var productsInBasket = basket.products;
            Assert.AreEqual(0, productsInBasket?[100]);
        }
    }

    [TestClass]
    public class MemberTest
    {
        private Member member;

        [TestInitialize]
        public void SetUp()
        {
            member = new Member(1, "username", new MailAddress("email@test.com"), "password");
        }

        [TestMethod]
        public void MemberLoginTest()
        {
            member.IsLoggedIn = true;
            Assert.IsTrue(member.IsLoggedIn);
        }

    }

    [TestClass]
    public class GuestTest
    {
        [TestMethod]
        public void GuestCreationTest()
        {
            var guest = new Guest(1);
            Assert.AreEqual(1, guest.Id);
        }
    }
}
