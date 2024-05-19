using System;
using System.Collections.Generic;
using System.Linq;
using MarketBackend.Domain.Shipping;
using MarketBackend.Services.Interfaces;

namespace MarketBackend.DAL
{
    public class ShipingDetailsRepositoryRAM //: IShippingDetailsRepository TODO: need to think about because IShippingDetailsRepository.getByID(..) needts to be implmemted but cant 
    {
        // Using a Tuple<string, string> as the composite key (Address, Name)
        private readonly Dictionary<(string Address, string Name), ShippingDetails> shippedPurchases;

        public ShipingDetailsRepositoryRAM()
        {
            shippedPurchases = new Dictionary<(string, string), ShippingDetails>();
        }

        public void Add(ShippingDetails entity)
        {
            var key = (entity.Address, entity.Name);
            if (shippedPurchases.ContainsKey(key))
            {
                throw new ArgumentException($"Shipping Details with the address: {entity.Address} and name: {entity.Name} already exists.");
            }
            shippedPurchases.Add(key, entity);
        }

        public void Delete(ShippingDetails entity)
        {
            var key = (entity.Address, entity.Name);
            if (!shippedPurchases.ContainsKey(key))
            {
                throw new KeyNotFoundException($"Shipping Details with the address: {entity.Address} and name: {entity.Name} does not exist.");
            }
            shippedPurchases.Remove(key);
        }

        public IEnumerable<ShippingDetails> getAll()
        {
            return shippedPurchases.Values.ToList();
        }

        public ShippingDetails GetById(string address, string name)
        {
            var key = (address, name);
            if (shippedPurchases.TryGetValue(key, out var shippingDetails))
            {
                return shippingDetails;
            }
            return null;
        }

        public void Update(ShippingDetails entity)
        {
            var key = (entity.Address, entity.Name);
            if (shippedPurchases.ContainsKey(key))
            {
                shippedPurchases[key] = entity;
            }
            else
            {
                throw new KeyNotFoundException($"Shipping Details with the address: {entity.Address} and name: {entity.Name} not found.");
            }
        }
    }
}
