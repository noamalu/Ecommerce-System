using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Payment
{
    public interface IPaymentSystemFacade
    {
        void Pay();
        void CancelPayment(int paymentID);
    }
}