using Moq;
using System;
using MarketBackend.Domain.Payment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;


namespace Market.Tests
{
    public class PaymentSystemProxyTests
    {
        public void TestPayWithMock()
        {
            // Arrange
            var mockPaymentSystemFacade = new Mock<IPaymentSystemFacade>();
            mockPaymentSystemFacade.Setup(x => x.Pay(It.IsAny<PaymentDetails>(), It.IsAny<double>())).Returns(12345);
            var paymentSystemProxy = new PaymentSystemProxy(mockPaymentSystemFacade.Object);

            var paymentDetails = new PaymentDetails(
                "1234567890123456",
                "12",
                "25",
                "John Doe",
                "123",
                "123456789");
            double totalAmount = 100.00;

            // Act
            int result = paymentSystemProxy.Pay(paymentDetails, totalAmount);

            // Assert
            Assert.Equal(12345, result);
        }

        [Fact]
        public void TestCancelPaymentWithMock()
        {
            // Arrange
            var mockPaymentSystemFacade = new Mock<IPaymentSystemFacade>();
            mockPaymentSystemFacade.Setup(x => x.CancelPayment(It.IsAny<int>())).Returns(1);
            var paymentSystemProxy = new PaymentSystemProxy(mockPaymentSystemFacade.Object);

            int paymentID = 12345;

            // Act
            int result = paymentSystemProxy.CancelPayment(paymentID);

            // Assert
            Assert.Equals(1, result);
        }

        [Fact]
        public void TestConnectWithMock()
        {
            // Arrange
            var mockPaymentSystemFacade = new Mock<IPaymentSystemFacade>();
            mockPaymentSystemFacade.Setup(x => x.Connect()).Returns(true);
            var paymentSystemProxy = new PaymentSystemProxy(mockPaymentSystemFacade.Object);

            // Act
            bool result = paymentSystemProxy.Connect();

            // Assert
            Assert.IsTrue(result);
        }
    }
}
