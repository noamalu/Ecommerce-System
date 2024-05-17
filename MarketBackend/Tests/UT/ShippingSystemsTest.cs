using MarketBackend.Domain.Shipping;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class ShippingSystemsTest
    {
        string address = "Reuven Rubin 3/6";
        string name = "Shaked Matityahu";
        string city = "Be'er Shave";
        string country = "Israel";
        string zipcode = "1111111";

        private IShippingSystemFacade shippingSystem;

        private ShippingDetails shippingDetails;

        [TestInitialize]
        public void SetUp()
        {
            var mockShippingSystemFacade = new Mock<IShippingSystemFacade>();
            shippingSystem = new ShippingSystemProxy(mockShippingSystemFacade.Object);
            shippingDetails = new ShippingDetails(name, city, address, country, zipcode);
        }

        [TestCleanup]
        public void CleanUp()
        {
            //TODO
        }
        [TestMethod]
        public void TestRequestShipmentSuccess()
        {
            int transactionId = shippingSystem.OrderShippment(shippingDetails);
            Assert.AreNotEqual(1, transactionId);
        }

        [TestMethod]
        public void TestCancelSupplySuccess()
        {
            int transactionId = shippingSystem.OrderShippment(shippingDetails);
            Assert.AreEqual(1, shippingSystem.CancelShippment(transactionId));
        }

        // [TestMethod]
        // public void TestShippingContactFailureNoContact()
        // {
        //     ShippingSystem.LoseContact = true;
        //     Assert.IsFalse(ShippingSystem.Handshake());
        //     Assert.AreEqual(-1, ShippingSystem.Pay("12345678", "04", "2021", "me", "777", "123123123"));
        //     Assert.AreEqual(-1, ShippingSystem.CancelPay(123));
        //     ShippingSystem.LoseContact = false;
        // }

        // [TestMethod]
        // public void TestSupplyContactFailureNoContact()
        // {
        //     SupplySystem.LoseContact = true;
        //     Assert.IsFalse(supplySystem.Handshake());
        //     Assert.AreEqual(-1, supplySystem.Supply("Michael", "1725 Slough Avenue", "Scranton", "PA, United States", "12345"));
        //     Assert.AreEqual(-1, supplySystem.CancelSupply(123));
        //     SupplySystem.LoseContact = false;
        // }
    
    }


}