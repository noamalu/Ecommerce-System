using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MarketBackend.Domain.Shipping;
using MarketBackend.Services.Interfaces;

namespace MarketBackend.DAL
{
    public class ShippingDetailsRepositoryRAM : IShippingDetailsRepository
    {
        // Using a Tuple<int, string, string> as the composite key (ShippingID, Address, Name)
        private readonly ConcurrentDictionary<Tuple<int, string, string>, ShippingDetails> _shipments;

        private static ShippingDetailsRepositoryRAM _shippingRepository = null;

        public ShippingDetailsRepositoryRAM()
        {
            _shipments = new ConcurrentDictionary<Tuple<int, string, string>, ShippingDetails>();
        }

        public static ShippingDetailsRepositoryRAM GetInstance()
        {
            _shippingRepository ??= new ShippingDetailsRepositoryRAM();
            return _shippingRepository;
        }

        public static void Dispose(){
            _shippingRepository = new ShippingDetailsRepositoryRAM();
        }

        public void Add(ShippingDetails entity)
        {
            var key = Tuple.Create(entity.ShippingID, entity.Address, entity.Name);
            if (_shipments.ContainsKey(key))
            {
                throw new ArgumentException($"Shipping Details with the ID: {entity.ShippingID}, address: {entity.Address}, and name: {entity.Name} already exists.");
            }
            _shipments.TryAdd(key, entity);
        }

        public void Delete(ShippingDetails entity)
        {
            var key = Tuple.Create(entity.ShippingID, entity.Address, entity.Name);
            if (!_shipments.ContainsKey(key))
            {
                throw new KeyNotFoundException($"Shipping Details with the ID: {entity.ShippingID}, address: {entity.Address}, and name: {entity.Name} does not exist.");
            }
            _shipments.TryRemove(key, out _);
        }

        public IEnumerable<ShippingDetails> getAll()
        {
            return _shipments.Values.ToList();
        }

        public void Update(ShippingDetails entity)
        {
            var key = Tuple.Create(entity.ShippingID, entity.Address, entity.Name);
            if (_shipments.ContainsKey(key))
            {
                _shipments[key] = entity;
            }
            else
            {
                throw new KeyNotFoundException($"Shipping Details with the ID: {entity.ShippingID}, address: {entity.Address}, and name: {entity.Name} not found.");
            }
        }

        public ShippingDetails GetById(int shippingID, string address, string name)
        {
            var key = Tuple.Create(shippingID, address, name);
            if (_shipments.TryGetValue(key, out var shippingDetails))
            {
                return shippingDetails;
            }
            return null;
        }

        // public List<ShippingDetails> GetById(int id)
        // {
        //     List<ShippingDetails> result = _shipments.Values.ToList();
        //     foreach (var shipping in result)
        //     {
        //         if (shipping.ShippingID == id)
        //         {
        //             result.Add(shipping);
        //         }
        //     }
        //     return result;
        // }

        public ShippingDetails GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
