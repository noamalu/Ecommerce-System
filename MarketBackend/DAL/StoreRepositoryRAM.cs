using MarketBackend.Domain.Models;
using MarketBackend.Services.Interfaces;
using MarketBackend.Domain.Market_Client;
using System.Collections.Concurrent;
using MarketBackend.DAL.DTO;

namespace MarketBackend.DAL
{
    public class StoreRepositoryRAM : IStoreRepository
    {
          private static ConcurrentDictionary<int, Store> _stores;
        private static StoreRepositoryRAM StoreRepository = null;
        DBcontext dBcontext;

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
        public async Task Add2(Store store)
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

        public void Delete(Store store)
        {
            
        lock (_lock)
            {
                bool shopInDomain = _stores.TryRemove(store.StoreId, out _);
                DBcontext context = DBcontext.GetInstance();
                StoreDTO storeDTO = context.Stores.Find(store.StoreId);
                if (shopInDomain)
                {
                    context.Stores.Remove(storeDTO);
                    context.SaveChanges();
                }
                else if (storeDTO != null)
                {
                    context.Stores.Remove(storeDTO);
                    context.SaveChanges();
                }
            }
        }

        public IEnumerable<Store> getAll()
        {
            lock (_lock)
            {
                List<StoreDTO> storesList = DBcontext.GetInstance().Stores.ToList();
                foreach (StoreDTO storeDTO in storesList)
                {
                    _stores.TryAdd(storeDTO.Id, new Store(storeDTO));
                }
            }
            
            return _stores.Values.ToList();
        }

        public Store GetById(int id)
        {
            if (_stores.ContainsKey(id))
            {
                return _stores[id];
            }
            else{
                lock (_lock)
                {
                    StoreDTO storeDTO = DBcontext.GetInstance().Stores.Find(id);
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
        }

        public void Update(Store store)
        {
             _stores[store._storeId] = store;
            StoreDTO storeDTO = DBcontext.GetInstance().Stores.Find(store._storeId);
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
            else DBcontext.GetInstance().Stores.Add(newStore);
            DBcontext.GetInstance().SaveChanges();
        }

        void IRepository<Store>.Add(Store entity)
        {
            throw new NotImplementedException();
        }
    }
}