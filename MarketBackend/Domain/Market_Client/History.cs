using MarketBackend.DAL;
namespace MarketBackend.Domain.Market_Client
{
    public class History
    {
        public SynchronizedCollection<Purchase> _purchases {get; set;}

        public History(int StoreId)
        {
            asyncHistory(StoreId);
        }

        public async void asyncHistory(int StoreId){
            _purchases = await PurchaseRepositoryRAM.GetInstance().GetShopPurchaseHistory(StoreId);
        }

        public async Task AddPurchase(Purchase p)
        {
                _purchases.Add(p);
                await PurchaseRepositoryRAM.GetInstance().Add(p);
        }
    }
}