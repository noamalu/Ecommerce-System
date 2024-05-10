using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Facades.Interfaces
{
    public interface IPaymentSystemFacade
    {
        void Pay();
        void CancelPayment(int paymentID);
    }
}