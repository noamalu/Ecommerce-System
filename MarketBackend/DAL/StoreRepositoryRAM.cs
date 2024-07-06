using MarketBackend.Domain.Models;
using MarketBackend.Services.Interfaces;
using MarketBackend.Domain.Market_Client;
using System.Collections.Concurrent;
using MarketBackend.DAL.DTO;

namespace MarketBackend.DAL
{
    public class StoreRepositoryRAM : IStoreRepository
    {
        private static ConcurrentDictionary<int, Store> _stores = new ConcurrentDictionary<int, Store>();
        private static StoreRepositoryRAM StoreRepository = null;
        private DBcontext dBcontext;
        private StoreDTO storeDTO;
        List<StoreDTO> storesList;

        public ConcurrentDictionary<int, Store> Stores { get => _stores; set => _stores = value; }
        private object _lock ;

        private StoreRepositoryRAM()
        {
            _stores = new ConcurrentDictionary<int, Store>();
            _lock = new object();
           
        }
        public static StoreRepositoryRAM GetInstance()
        {
            if (StoreRepository == null)
                StoreRepository = new StoreRepositoryRAM();
            return StoreRepository;
        }

        public static void Dispose(){
            StoreRepository = new StoreRepositoryRAM();
        }
        public async Task Add(Store store)
        {
            dBcontext = DBcontext.GetInstance();
            await dBcontext.PerformTransactionalOperationAsync(async () =>
            {
                dBcontext.Stores.Add(new StoreDTO(store));
            });
            _stores.TryAdd(store.StoreId, store);
            // DBcontext.GetInstance().Stores.Add(new StoreDTO(store));
            // DBcontext.GetInstance().SaveChanges();
        
        }

        public async Task Delete(Store store)
        {
            bool shopInDomain = _stores.TryRemove(store.StoreId, out _);
            dBcontext = DBcontext.GetInstance();
            await dBcontext.PerformTransactionalOperationAsync(async () =>
            {
                StoreDTO storeDTO = dBcontext.Stores.Find(store.StoreId);
                if (shopInDomain)
                {
                    dBcontext.Stores.Remove(storeDTO);
                }
                else if (storeDTO != null)
                {
                    dBcontext.Stores.Remove(storeDTO);
                }
                _stores.TryRemove(store.StoreId, out store);
            });
        }

        public async Task<IEnumerable<Store>> getAll()
        {
            dBcontext = DBcontext.GetInstance();
            await dBcontext.PerformTransactionalOperationAsync(async () =>
            {
                storesList = dBcontext.Stores.ToList();
            });
            foreach (StoreDTO storeDTO in storesList)
            {
                _stores.TryAdd(storeDTO.Id, new Store(storeDTO));
            }
            return _stores.Values.ToList();
        }

        public async Task<Store> GetById(int id)
        {
            if (_stores.ContainsKey(id))
            {
                return _stores[id];
            }
            else{
                dBcontext = DBcontext.GetInstance();
                await dBcontext.PerformTransactionalOperationAsync(async () =>
                {
                    storeDTO = dBcontext.Stores.Find(id);
                });
                if (storeDTO != null)
                {
                    Store store = new Store(storeDTO);
                    _stores.TryAdd(id, store);
                    store.Initialize(storeDTO);
                    return store;
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task Update(Store store)
        {
             _stores[store._storeId] = store;
             dBcontext = DBcontext.GetInstance();
            await dBcontext.PerformTransactionalOperationAsync(async () =>
            {
                StoreDTO storeDTO = dBcontext.Stores.Find(store._storeId);
                StoreDTO newStore = new StoreDTO(store);
                if (storeDTO != null)
                {
                    storeDTO.Active = newStore.Active;
                    storeDTO.Purchases = newStore.Purchases;
                    storeDTO.Products = newStore.Products;
                    storeDTO.Rules = newStore.Rules;
                    storeDTO.Name = newStore.Name;
                    storeDTO.Rating = newStore.Rating;
                }
                else dBcontext.Stores.Add(newStore);
            });
        }
    }
}