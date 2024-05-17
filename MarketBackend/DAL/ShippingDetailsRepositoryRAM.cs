using System;
using System.Collections.Generic;
using System.Linq;
using MarketBackend.Domain.Shipping;
using MarketBackend.Services.Interfaces;

namespace MarketBackend.DAL
{
    public class ShipingDetailsRepositoryRAM //: IShippingDetailsRepository TODO: need to think about because IShippingDetailsRepository.getByID(..) needts to be implmemted but cant 
    {
        private readonly Dictionary<string, ShippingDetails> shippedPurchases;

         public ShipingDetailsRepositoryRAM()
        {
            shippedPurchases = new Dictionary<string, ShippingDetails>();
        }

        public void Add(ShippingDetails entity)
        {
            if(shippedPurchases.ContainsKey(entity.Address)){
                throw new ArgumentException($"Shipping Details with the address: {entity.Address} already exists.");

            }
            shippedPurchases.Add(entity.Address, entity);
        }

        public void Delete(ShippingDetails entity)
        {
            if (!shippedPurchases.ContainsKey(entity.Address)){
                throw new KeyNotFoundException($"Shipping Details with the address: {entity.Address} does not exist.");
            }

            shippedPurchases.Remove(entity.Address);
        }

        public IEnumerable<ShippingDetails> getAll()
        {
            return shippedPurchases.Values.ToList();
        }

        public ShippingDetails GetById(string address)
        {
            bool exist = shippedPurchases.ContainsKey(address);
            if (exist)
            {
                return shippedPurchases[address];
            }
            return null;

        }


        public void Update(ShippingDetails entity)
        {
            if (shippedPurchases.ContainsKey(entity.Address))
            {
                shippedPurchases[entity.Address] = entity;
            }
            else
            {
                throw new KeyNotFoundException($"Shipping Details with the address: {entity.Address} not found.");
            }
        }
    }
}

