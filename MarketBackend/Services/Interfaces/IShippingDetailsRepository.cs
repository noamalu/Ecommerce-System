using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.Domain.Shipping;

namespace MarketBackend.Services.Interfaces
{
    public interface IShippingDetailsRepository : IRepository<ShippingDetails>
    {
         public ShippingDetails GetById(string address);
    }
}