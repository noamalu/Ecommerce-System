using MarketBackend.Domain.Models;
using MarketBackend.Services.Interfaces;
using MarketBackend.Domain.Market_Client;
using System.Collections.Concurrent;

namespace MarketBackend.DAL
{
    public class StoreRepositoryRAM : IStoreRepository
    {
          private static ConcurrentDictionary<int, Store> _stores;
        private static StoreRepositoryRAM StoreRepository = null;

        public ConcurrentDictionary<int, Store> Stores { get => _stores; set => _stores = value; }

        private StoreRepositoryRAM()
        {
            _stores = new ConcurrentDictionary<int, Store>();
           
        }
        public static StoreRepositoryRAM GetInstance()
        {
            if (StoreRepository == null)
                StoreRepository = new StoreRepositoryRAM();
            return StoreRepository;
        }
        public void Add(Store store)
        {
            _stores.TryAdd(store.StoreId, store);
        
        }

        public void Delete(Store store)
        {
                bool shopInDomain = _stores.TryRemove(store.StoreId, out _);
        }

        public IEnumerable<Store> getAll()
        {
            
            return _stores.Values.ToList();
        }

        public Store GetById(int id)
        {
            if (_stores.ContainsKey(id))
            {
                return _stores[id];
            }
            else{
                return null;
            }
        }

        public void Update(Store store)
        {
            _stores[store.StoreId] = store;
        }
    }
}