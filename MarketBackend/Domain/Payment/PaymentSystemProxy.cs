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
           if (realPaymentSystem == null)
                return true;
           else
                return realPaymentSystem.Connect();
        }

        public int Pay(PaymentDetails cardDetails, double totalAmount)
        {
            if (realPaymentSystem == null)
            {
                if (succeedPayment)
                    return fakeTransactionId++;
                
                return -1;
            }
            else
            {
                return realPaymentSystem.Pay(cardDetails, totalAmount);
            }

        }

         public int CancelPayment(int paymentID)
        {
            if (realPaymentSystem == null)
                return 1;
            
            else
            {
                if (realPaymentSystem.Connect())
                    return realPaymentSystem.CancelPayment(paymentID);
            }
            return -1;

        }

    }
}