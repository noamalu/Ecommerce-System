using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Payment
{
    public class PaymentSystemProxy : IPaymentSystemFacade
    {
        //  TODO : when we will connect the system to real payment system
        private readonly IPaymentSystemFacade _realPaymentSystem;
        private int receiptID;

        public static bool succeedPayment = true; 
        private static int fakeTransactionId = 10000;

        public PaymentSystemProxy(IPaymentSystemFacade realPaymentSystem)
        {
            _realPaymentSystem = realPaymentSystem ?? throw new ArgumentNullException(nameof(realPaymentSystem));
            receiptID = 1;
        }

        public bool Connect()
        {
           if (_realPaymentSystem == null)
                return true;
           else
                return _realPaymentSystem.Connect();
        }

        public int Pay(PaymentDetails cardDetails, double totalAmount)
        {
            if (_realPaymentSystem == null)
            {
                if (succeedPayment)
                    return fakeTransactionId++;
                
                return -1;
            }
            else
            {
                return _realPaymentSystem.Pay(cardDetails, totalAmount);
            }

        }

         public int CancelPayment(int paymentID)
        {
            if (_realPaymentSystem == null)
                return 1;
            
            else
            {
                if (_realPaymentSystem.Connect())
                    return _realPaymentSystem.CancelPayment(paymentID);
            }
            return -1;

        }

    }
}