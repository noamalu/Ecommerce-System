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
            shippingSystem.Connect();
        }

        [TestMethod]
        public void TestRequestShipmentSuccess()
        {
            int transactionId = shippingSystem.OrderShippment(shippingDetails);
            Assert.AreNotEqual(1, transactionId);
        }

        [TestMethod]
        public void TestCancelShipmentSuccess()
        {
            int transactionId = shippingSystem.OrderShippment(shippingDetails);
            Assert.AreEqual(1, shippingSystem.CancelShippment(transactionId));
        }

        [TestMethod]
        public void TestShippingWithoutConnection()
        {
            shippingSystem.Disconnect();
            Assert.AreEqual(-1, shippingSystem.OrderShippment(shippingDetails));
            Assert.AreEqual(-1, shippingSystem.CancelShippment(123));
            
        }
    
    }


}