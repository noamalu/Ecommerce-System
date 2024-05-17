using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.Domain.Models;
using MarketBackend.Domain.Payment;

namespace MarketBackend.Services.Interfaces
{
    public interface IPaymentDetailsRepository : IRepository<PaymentDetails>
    {
    }
}