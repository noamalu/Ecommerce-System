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
        private int orderID;

        public static bool connected;
        private static bool succeedShipping = true;

        private static int fakeTransactionId = 10000;

        public ShippingSystemProxy(IShippingSystemFacade realShippingtSystem)
        {
            _realShippingtSystem = realShippingtSystem ?? throw new ArgumentNullException(nameof(realShippingtSystem));
            orderID = 1;
            connected = false;
        }

        public bool Connect()
        {
            if (_realShippingtSystem == null)
            {
                connected = true;
                return true;
            }
           else
                if (_realShippingtSystem.Connect())
                {
                    connected = true;
                    return true;
                }
                else
                {
                    connected = false;
                    return false;
                }  
                
        }
        

        public int CancelShippment(int orderID)
        {
            if (connected)
            {
                if (_realShippingtSystem == null)
                    return 1;
                else
                    return _realShippingtSystem.CancelShippment(orderID);
            }
            return -1;
        }

        public int OrderShippment(ShippingDetails details)
        {
           if (connected)
           {
                if (_realShippingtSystem == null)
                    return fakeTransactionId++;
                else
                    return _realShippingtSystem.OrderShippment(details);
            }
            return -1;
        }

        public void Disconnect () //For testing
        {
            connected = false;
        }
    }
}

     
