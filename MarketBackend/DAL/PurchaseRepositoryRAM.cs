using MarketBackend.Domain.Models;
using MarketBackend.Services.Interfaces;
using MarketBackend.Domain.Market_Client;
using MarketBackend.DAL.DTO;
using Microsoft.EntityFrameworkCore;

namespace MarketBackend.DAL
{
    public class PurchaseRepositoryRAM : IPurchaseRepository
    {
        private static Dictionary<int, Purchase> _purchaseById;

        private static PurchaseRepositoryRAM _purchaseRepo = null;
        private object _lock;


        private PurchaseRepositoryRAM()
        {
            _purchaseById = new Dictionary<int, Purchase>();
            _lock = new object();
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
        public void Add(Purchase purchase)
        {
            lock (_lock){
            _purchaseById.Add(purchase.PurchaseId, purchase);
            DBcontext context = DBcontext.GetInstance();
            StoreDTO storeDTO = context.Stores.Include(s => s.Purchases).FirstOrDefault(s => s.Id == purchase.StoreId);
            BasketDTO basketDTO = context.Baskets.Find(purchase.Basket._basketId);
            PurchaseDTO purchaseDTO = new PurchaseDTO(purchase, basketDTO);
            storeDTO.Purchases.Add(purchaseDTO);
            context.Purchases.Add(purchaseDTO);
            context.SaveChanges();
            }
            
        }
        public Purchase GetById(int id)
        {
            if (_purchaseById.ContainsKey(id))
                return _purchaseById[id];
            else
            {
                lock (_lock)
                {
                    PurchaseDTO purchaseDTO = DBcontext.GetInstance().Purchases.Find(id);
                    if (purchaseDTO != null)
                    {
                        _purchaseById.Add(id, new Purchase(purchaseDTO));
                    }
                    return _purchaseById[id];
                }
            }
        }
    
        public void Delete(Purchase Purchase)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Purchase> getAll()
        {
             lock (_lock)
            {
                foreach (PurchaseDTO purchaseDTO in DBcontext.GetInstance().Purchases)
                {
                    _purchaseById.TryAdd(purchaseDTO.Id, new Purchase(purchaseDTO));
                }
            }
            return _purchaseById.Values.ToList();
        }

        public void Update(Purchase purchase)
        {
            _purchaseById[purchase.PurchaseId] = purchase;
             lock (_lock)
            {
                DBcontext context = DBcontext.GetInstance();
                PurchaseDTO purchaseDTO = context.Purchases.Find(purchase.PurchaseId);
                // purchaseDTO.PurchaseStatus = purchase.PurchaseStatus.ToString();
                purchaseDTO.Price = purchase.Price;
                context.SaveChanges();
            }
           
        }
    

        public SynchronizedCollection<Purchase> GetShopPurchaseHistory(int storeId)
        {
            SynchronizedCollection<Purchase> result = new SynchronizedCollection<Purchase>();
            lock (_lock)
            {
                List<PurchaseDTO> lp = DBcontext.GetInstance().Purchases.Where((p) => p.StoreId == storeId).ToList();
                foreach (PurchaseDTO purchaseDTO in lp)
                {
                    _purchaseById.TryAdd(purchaseDTO.Id, new Purchase(purchaseDTO));
                }
            }
            foreach (Purchase purchase in _purchaseById.Values)
            {
                if (purchase.StoreId == storeId) result.Add(purchase);
            }
            return result;
        }
    }
}