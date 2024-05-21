using MarketBackend.Domain.Models;
using MarketBackend.Services.Interfaces;
using MarketBackend.Domain.Market_Client;

namespace MarketBackend.DAL
{
    public class PurchaseRepositoryRAM : IPurchaseRepository
    {
        private static Dictionary<int, Purchase> _purchaseById;

        private static PurchaseRepositoryRAM _purchaseRepo = null;


        private PurchaseRepositoryRAM()
        {
            _purchaseById = new Dictionary<int, Purchase>();
        }
        public static PurchaseRepositoryRAM GetInstance()
        {
            if (_purchaseRepo == null)
                _purchaseRepo = new PurchaseRepositoryRAM();
            return _purchaseRepo;
        }

        public static void Dispose(){
            _purchaseRepo = new PurchaseRepositoryRAM();
        }
        public void Add(Purchase item)
        {
            _purchaseById.Add(item.PurchaseId, item);
        }
        public Purchase GetById(int id)
        {
            if (_purchaseById.ContainsKey(id))
                return _purchaseById[id];
            else return null;
        }
        public bool ContainsID(int id)
        {
            throw new NotImplementedException();
        }

        public bool ContainsValue(Purchase item)
        {
            throw new NotImplementedException();
        }

        public void Delete(Purchase Purchase)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Purchase> getAll()
        {
            return _purchaseById.Values.ToList();
        }

        public void Update(Purchase item)
        {
            _purchaseById[item.PurchaseId] = item;
           
        }
    
        public void Clear()
        {
            _purchaseById.Clear();
        }
        public void ResetDomainData()
        {
            _purchaseById = new Dictionary<int, Purchase>();
        }

        public SynchronizedCollection<Purchase> GetShopPurchaseHistory(int StoreId)
        {
            SynchronizedCollection<Purchase> result = new SynchronizedCollection<Purchase>();
            foreach (Purchase purchase in _purchaseById.Values)
            {
                if (purchase.StoreId == StoreId) result.Add(purchase);
            }
            return result;
        }
    }
}