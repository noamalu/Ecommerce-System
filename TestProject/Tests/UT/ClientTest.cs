﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Mail;
using MarketBackend.Domain.Market_Client;
using MarketBackend.DAL;
using MarketBackend.DAL.DTO;

namespace UnitTests
{
    [TestClass]
    public class ClientTest
    {
        [TestInitialize]
        public void SetUp()
        {
            DBcontext.GetInstance().Dispose();
            ProductRepositoryRAM productRepositoryRAM = ProductRepositoryRAM.GetInstance();
            productRepositoryRAM.Add(new Product(11, 0, "product", "Regular", "nice", 1.0, "fruit", 10, false));
        }

        [TestCleanup]
        public void Cleanup()
        {
            DBcontext.GetInstance().Dispose();
        }



        [TestMethod]
        public void TestAddToCart()
        {
            var client = new Guest(1); // Testing with Guest as an example

            client.AddToCart(0, 11, 10); // basket, productId, quantity
            var productsInBasket = BasketRepositoryRAM.GetInstance().getBasketsByCartId(client.Cart._shoppingCartId).Where(basket => basket._storeId == 0).FirstOrDefault()?.products;
            Assert.AreEqual(10, productsInBasket?[11],
            $"Expected products in basket to be 10, but got {productsInBasket?[11]}");
        }

        [TestMethod]
        public void TestRemoveFromCart()
        {
            var client = new Guest(2);
            client.AddToCart(0, 11, 10);
            var basket = BasketRepositoryRAM.GetInstance().getBasketsByCartId(client.Cart._shoppingCartId).Where(basket => basket._storeId == 0).FirstOrDefault();
            client.RemoveFromCart(0, 11, 10);
            var productsInBasket = basket.products;
            Assert.AreEqual(0, productsInBasket.Count,
            $"Expected count of products to be 0 but got {productsInBasket.Count}");
        }
    }

    [TestClass]
    public class MemberTest
    {
        private Member member;

        [TestInitialize]
        public void SetUp()
        {
            DBcontext.GetInstance().Dispose();
            member = new Member(1, "username", new MailAddress("email@test.com"), "password");
        }

        [TestCleanup]
        public void Cleanup()
        {
            DBcontext.GetInstance().Dispose();
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

        public void SetUp()
        {
            DBcontext.GetInstance().Dispose();
        }

        [TestCleanup]
        public void Cleanup()
        {
            DBcontext.GetInstance().Dispose();
        }
        
        [TestMethod]
        public void GuestCreationTest()
        {
            var guest = new Guest(1);
            Assert.AreEqual(1, guest.Id,
            $"Expected guest id to be 1 but got {guest.Id}");
        }
    }
}
