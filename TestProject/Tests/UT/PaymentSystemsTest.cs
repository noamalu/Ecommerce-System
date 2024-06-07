using MarketBackend.Domain.Payment;
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
            paymentSystem = new PaymentSystemProxy();
            paymentDetails = new PaymentDetails(cardNumber, exprYear, exprMonth, cvv, cardID, name);
            paymentSystem.Connect();
            
        }

        [TestMethod]
        // Success - Test that the payment system can connect
        public void TestAttemptToConnect()
        {
            Assert.IsTrue(paymentSystem.Connect());
        }

        [TestMethod]
        // Success - Test that the payment system returns a transaction ID when the payment is successful
        public void TestAttemptPaymentSuccess()
        {
            paymentSystem.Connect();
            int transactionId = paymentSystem.Pay(paymentDetails, totalAmount);
            Console.WriteLine($"Transaction ID: {transactionId}");  
            Assert.IsTrue(transactionId > 0);
        }

        [TestMethod]
        // Success - Test that the payment system returns -1 when the payment fails due to a missing connection
        public void TestRequestRefundSuccess()
        {
            int transactionId = paymentSystem.Pay(paymentDetails, totalAmount);
            Assert.AreEqual(1, paymentSystem.CancelPayment(transactionId));
        }

        [TestMethod]
        // Fails - Test that the payment system returns -1 when the payment fails due to a missing connection
        public void TestPaymentWithoutConnection()
        {
            paymentSystem.Disconnect();
            Assert.AreEqual(-1, paymentSystem.Pay(paymentDetails, totalAmount));
            Assert.AreEqual(-1, paymentSystem.CancelPayment(123));
            
        }

        [TestMethod]
        // Fails - Test with Mock that the payment system returns -1 when the payment fails due to a missing contact
        public void TestMockedPaymentSystemRejection()
        {
            var mockPaymentSystem = new Mock<RealPaymentSystem>();
            mockPaymentSystem.Setup(ps => ps.Pay(It.IsAny<PaymentDetails>(), It.IsAny<double>())).Returns(-1);
            paymentSystem = new PaymentSystemProxy(mockPaymentSystem.Object);
            int transactionId = paymentSystem.Pay(paymentDetails, totalAmount);

            Assert.AreEqual(-1, transactionId);
        }

        [TestMethod]
        // Fails - Test that the payment system returns -1 when the payment fails due to wrong total amount
        public void TestPaymentWithWrongTotalAmount()
        {
            totalAmount = -100.00;
            Assert.AreEqual(-1, paymentSystem.Pay(paymentDetails, totalAmount));
        }
    }
}
