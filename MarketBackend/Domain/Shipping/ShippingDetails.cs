using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Shipping
{
    public class ShippingDetails
    {
        private static int counterIDs = 0;
        private static readonly object _lock = new object();

        private int _shippingID;
        string address;
        string name;
        string city;
        string country;
        string zipcode;

        public ShippingDetails(string name, string city, string address, string country, string zipcode)
        {
            this._shippingID = GenerateShippingID();
            this.name = name;
            this.city = city;
            this.address = address;
            this.country = country;
            this.zipcode = zipcode;
        }
         private static int GenerateShippingID()
        {
            lock (_lock)
            {
                return ++counterIDs;
            }
        }

        public int ShippingID {get => _shippingID; }
        public String Name {get => name; set => name = value; }
        public String City {get => city; set => city = value; }
        public String Address {get => address; set => address = value; }
        public String Country {get => country; set => country = value; }
        public String Zipcode {get => zipcode; set => zipcode = value; }
    

        public override string ToString()
        {
            return $"ID: {_shippingID}, Name: {Name}, City: {City}, Address: {Address}, Country: {Country}, Zipcode: {Zipcode}";
        }
    }
}