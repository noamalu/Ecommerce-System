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

        private DBcontext dBcontext;
        private PurchaseDTO purchaseDTO;


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
        public async Task Add(Purchase purchase)
        {
            dBcontext = DBcontext.GetInstance();
            await dBcontext.PerformTransactionalOperationAsync(async () =>
            {
            StoreDTO storeDTO = dBcontext.Stores.Include(s => s.Purchases).FirstOrDefault(s => s.Id == purchase.StoreId);
            BasketDTO basketDTO = dBcontext.Baskets.Find(purchase.Basket._basketId);
            PurchaseDTO purchaseDTO = new PurchaseDTO(purchase, basketDTO);
            storeDTO.Purchases.Add(purchaseDTO);
            dBcontext.Purchases.Add(purchaseDTO);
            });
            _purchaseById.Add(purchase.PurchaseId, purchase);
        }

        public async Task<Purchase> GetById(int id)
        {
            if (_purchaseById.ContainsKey(id))
                return _purchaseById[id];
            else
            {
                dBcontext = DBcontext.GetInstance();
                await dBcontext.PerformTransactionalOperationAsync(async () =>
                {
                purchaseDTO = DBcontext.GetInstance().Purchases.Find(id);
                });
                
                if (purchaseDTO != null)
                {
                    _purchaseById.Add(id, new Purchase(purchaseDTO));
                }
                return _purchaseById[id];
            }
        }
    
        public async Task Delete(Purchase Purchase)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Purchase>> getAll()
        {
            dBcontext = DBcontext.GetInstance();
            await dBcontext.PerformTransactionalOperationAsync(async () =>
            {
                foreach (PurchaseDTO purchaseDTO in dBcontext.Purchases)
                {
                    _purchaseById.TryAdd(purchaseDTO.Id, new Purchase(purchaseDTO));
                }
            });
            
            return _purchaseById.Values.ToList();
        }

        public async Task Update(Purchase purchase)
        { 
            dBcontext = DBcontext.GetInstance();
            await dBcontext.PerformTransactionalOperationAsync(async () =>
            {
                purchaseDTO = dBcontext.Purchases.Find(purchase.PurchaseId);
            });
            _purchaseById[purchase.PurchaseId] = purchase;
            purchaseDTO.Price = purchase.Price;
           
        }
    

        public async Task<SynchronizedCollection<Purchase>> GetShopPurchaseHistory(int storeId)
        {
            SynchronizedCollection<Purchase> result = new SynchronizedCollection<Purchase>();
            dBcontext = DBcontext.GetInstance();
            await dBcontext.PerformTransactionalOperationAsync(async () =>
            {
                List<PurchaseDTO> lp = dBcontext.Purchases.Where((p) => p.StoreId == storeId).ToList();
                foreach (PurchaseDTO purchaseDTO in lp)
                {
                    _purchaseById.TryAdd(purchaseDTO.Id, new Purchase(purchaseDTO));
                }
            });
            
            foreach (Purchase purchase in _purchaseById.Values)
            {
                if (purchase.StoreId == storeId) result.Add(purchase);
            }
            return result;
        }
    }
}