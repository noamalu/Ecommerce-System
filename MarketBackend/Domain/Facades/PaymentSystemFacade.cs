using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.Domain.Facades.Interfaces;

namespace MarketBackend.Domain.Facades
{
    public class PaymentSystemFacade : IPaymentSystemFacade
    {
        public void CancelPayment(int paymentID)
        {
            throw new NotImplementedException();
        }

        public bool Connect()
        {
            throw new NotImplementedException();
        }

        public void Pay()
        {
            throw new NotImplementedException();
        }
    }
}