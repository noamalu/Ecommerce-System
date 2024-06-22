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
            shippingSystem = new ShippingSystemProxy();
            shippingDetails = new ShippingDetails(name, city, address, country, zipcode);
            shippingSystem.Connect();
        }

        [TestMethod]
        // Success - Test that the shipping system can connect
        public void TestAttemptToConnect()
        {
            Assert.IsTrue(shippingSystem.Connect());
        }

        [TestMethod]
        // Success - Test that the shipping system returns a transaction ID when the order shipment is successful
        public void TestRequestShipmentSuccess()
        {
            int transactionId = shippingSystem.OrderShippment(shippingDetails);
            Assert.AreNotEqual(1, transactionId,
            $"Expected id not to be 1 but got {transactionId}");
        }

        [TestMethod]
        // Success - Test that the shipping system returns 1 when the order shipment  is successful
        public void TestCancelShipmentSuccess()
        {
            int transactionId = shippingSystem.OrderShippment(shippingDetails);
            Assert.AreEqual(1, shippingSystem.CancelShippment(transactionId),
            $"Expected 1 but got {shippingSystem.CancelShippment(transactionId)}");
        }

        [TestMethod]
        // Fails - Test that the shipping system returns -1 when the shipping fails due to a missing connection
        public void TestShippingWithoutConnection()
        {
            shippingSystem.Disconnect();
            Assert.AreEqual(-1, shippingSystem.OrderShippment(shippingDetails),
            $"Expected -1 but got {shippingSystem.OrderShippment(shippingDetails)}");
            Assert.AreEqual(-1, shippingSystem.CancelShippment(123),
            $"Expected -1 but got {shippingSystem.CancelShippment(123)}");
            
        }

        [TestMethod]
        // Fails - Test with Mock that the shipping system returns -1 when the shippinh fails due to a missing connection
        public void TestMockedShippingSystemRejection()
        {
            var mockShippingSystem = new Mock<RealShippingSystem>();
            mockShippingSystem.Setup(ps => ps.OrderShippment(It.IsAny<ShippingDetails>())).Returns(-1);

            shippingSystem = new ShippingSystemProxy(mockShippingSystem.Object);
            int transactionId = shippingSystem.OrderShippment(shippingDetails);

            Assert.AreEqual(-1, transactionId,
            $"Expected -1 but got {transactionId}");
        }
    
    }


}