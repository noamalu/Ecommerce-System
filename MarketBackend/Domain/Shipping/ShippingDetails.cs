using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Shipping
{
    public class ShippingDetails
    {
        string name;
        string city;
        string address;

        string country;

        string zipcode;

        public ShippingDetails(string name, string city, string address, string country, string zipcode)
        {
            this.name = name;
            this.city = city;
            this.address = address;
            this.country = country;
            string.zipcode = zipcode;
        }
        public String Name {get => name; set => name = value; }
        public String City {get => city; set => city = value; }
        public String Address {get => address; set => address = value; }
        public String Country {get => country; set => country = value; }
        public String Zipcode {get => zipcode; set => zipcode = value; }
    
    }
}