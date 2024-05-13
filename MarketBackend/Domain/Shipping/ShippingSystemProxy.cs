using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Shipping
{
    public class ShippingSystemProxy : IShippingSystemFacade
    {
         //  TODO : when we will connect the system to real shipping system -> change realShippingtSystem
        private  IShippingSystemFacade? _realShippingtSystem = null;
        private int orderId;

        public static bool succeedShipping = true;

        private static int fakeTransactionId = 10000;

        public ShippingSystemProxy(IShippingSystemFacade realShippingtSystem)
        {
            _realShippingtSystem = realShippingtSystem ?? throw new ArgumentNullException(nameof(realShippingtSystem));
            orderID = 1;
        }

        public bool Conect()
        {
             if (realShippingtSystem == null)
                return true;
           else
                return realShippingtSystem.Connect();
        }
        }

        public int CancelShippment(int orderID)
        {
            if (realShippingtSystem == null)
                return 1;
            
            else
            {
                if (realShippingtSystem.Connect())
                    return realShippingtSystem.CancelShippment(orderID);
            }
            return -1;

        }

        public void OrderShippment(ShippingDeatails details)
        {
           if (realShippingtSystem == null)
            {
                if (succeedShipping)
                    return fakeTransactionId++;
                
                return -1;
            }
            else
            {
                return realShippingtSystem.OrderShippment(details);
            }

        }
    }

     
