using MarketBackend.Domain.Payment;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests
{
    [TestClass]
    public class PaymentSystemsTest
    {
        string cardNumber = "1234567890123456";
        string exprYear = "2027";

        string exprMonth = "01";
        string cvv = "123";
        string cardID = "206971997";
        string name = "Shaked Matityahu";
        double totalAmount = 100.00;

        private IPaymentSystemFacade paymentSystem;

        private PaymentDetails paymentDetails;

        [TestInitialize]
        public void SetUp()
        {
            var mockPaymentSystemFacade = new Mock<IPaymentSystemFacade>();
            paymentSystem = new PaymentSystemProxy(mockPaymentSystemFacade.Object);
            paymentDetails = new PaymentDetails(cardNumber, exprYear, exprMonth, cvv, cardID, name);
            paymentSystem.Connect();
        }

        [TestMethod]
        public void TestAttemptPaymentSuccess()
        {
            int transactionId = paymentSystem.Pay(paymentDetails, totalAmount);
            Assert.IsTrue(transactionId > 0);
        }

        [TestMethod]
        public void TestRequestRefundSuccess()
        {
            int transactionId = paymentSystem.Pay(paymentDetails, totalAmount);
            Assert.AreEqual(1, paymentSystem.CancelPayment(transactionId));
        }

        [TestMethod]
        public void TestPaymentWithoutConnection()
        {
            paymentSystem.Disconnect();
            Assert.AreEqual(-1, paymentSystem.Pay(paymentDetails, totalAmount));
            Assert.AreEqual(-1, paymentSystem.CancelPayment(123));
            
        }
    }
}