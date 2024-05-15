using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Shipping
{
    public interface IShippingSystemFacade
    {
        int OrderShippment(int orderID);
        int CancelShippment(ShippingDetails details);       

        bool Conect();     
    }
}