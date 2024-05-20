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
        public void TestAttemptToConnect()
        {
            Assert.IsTrue(paymentSystem.Connect());
        }

        [TestMethod]
        public void TestAttemptPaymentSuccess()
        {
            paymentSystem.Connect();
            int transactionId = paymentSystem.Pay(paymentDetails, totalAmount);
            Console.WriteLine($"Transaction ID: {transactionId}");  
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
