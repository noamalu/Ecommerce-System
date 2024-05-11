using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Shipping
{
    public interface IShippingSystemFacade
    {
        void OrderShippment();
        void CancelShippment();            
    }
}