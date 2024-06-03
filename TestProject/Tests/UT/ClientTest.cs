using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using MarketBackend.Domain.Market_Client;
using MarketBackend.DAL;

namespace UnitTests
{
    [TestClass]
    public class ClientTest
    {
        [TestInitialize]
        public void SetUp()
        {
            ProductRepositoryRAM productRepositoryRAM = ProductRepositoryRAM.GetInstance();
            productRepositoryRAM.Add(new Product(11, 0, "product", "Regular", "nice", 1.0, "fruit", 10, false));
        }



        [TestMethod]
        public void TestAddToCart()
        {
            var client = new Guest(1); // Testing with Guest as an example

            client.AddToCart(0, 11, 10); // basket, productId, quantity
            var productsInBasket = BasketRepositoryRAM.GetInstance().getBasketsByCartId(client.Cart._shoppingCartId).Where(basket => basket._storeId == 0).FirstOrDefault()?.products;
            Assert.AreEqual(10, productsInBasket?[11]);
        }

        [TestMethod]
        public void TestRemoveFromCart()
        {
            var client = new Guest(1);
            client.AddToCart(0, 11, 10);
            var basket = BasketRepositoryRAM.GetInstance().getBasketsByCartId(client.Cart._shoppingCartId).Where(basket => basket._storeId == 0).FirstOrDefault();
            client.RemoveFromCart(basket._basketId, 11, 10);
            var productsInBasket = basket.products;
            Assert.AreEqual(0, productsInBasket?[11]);
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
