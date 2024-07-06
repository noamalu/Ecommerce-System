using System.Collections.Concurrent;
using System.Text;
using MarketBackend.DAL;
using MarketBackend.Domain.Models;

namespace MarketBackend.Domain.Market_Client
{
 public class StoreRatingFilter : FilterSearchType
    {
        private double _lowRate;
        private double _highRate;
        private List<Store> _relevantstores;

        public StoreRatingFilter(double lowRate, double highRate)
        {
            _lowRate = lowRate;
            _highRate = highRate;
            asyneStoreRatingFilter();
        }

        public async void asyneStoreRatingFilter(){
            _relevantstores = await FindRelevantstores();
        }

        protected override bool Predicate(Product product)
        {
            return _relevantstores.ToList().Find((store) => store.StoreId == product.StoreId) != null;
        }

        private async Task<List<Store>> FindRelevantstores()
        {
            List<Store> stores = (await StoreRepositoryRAM.GetInstance().getAll()).ToList();
            return stores.FindAll((store) => store._raiting >= _lowRate && store._raiting <= _highRate);
        }

    }
}
