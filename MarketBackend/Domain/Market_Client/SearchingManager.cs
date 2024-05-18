using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using MarketBackend.Domain.Models;

namespace MarketBackend.Domain.Market_Client{
    public static class SearchingManager
    {
        public static HashSet<Product> serachByNameWithStore(int storeId, string productName)
        {
            HashSet<Product> resultProducts = new HashSet<Product>();
            Store store = StoreRepo.GetInstance().GetStoreById(storeId);
            foreach (Product product in store.Products)
            {
                if  (product.Name==productName)
                {
                    resultProducts.Add(product);
                }
            }
            return resultProducts;
        }

        public static HashSet<Product> searchByCategoryWithStore(int storeId, string category)
        {
            HashSet<Product> resultProducts = new HashSet<Product>();
            Store store = StoreRepo.GetInstance().GetStoreById(storeId);
            foreach (Product product in store.Products)
            {
                if  (product.Category==category)
                {
                    resultProducts.Add(product);
                }
            }
            return resultProducts;
        }

        public static HashSet<Product> searchByKeywordWithStore(int storeId, string keyword)
        {
            HashSet<Product> resultProducts = new HashSet<Product>();
            Store store = StoreRepo.GetInstance().GetStoreById(storeId);
            foreach (Product product in store.Products)
            {
                if  (product.Keywords.Contains(keyword))
                {
                    resultProducts.Add(product);
                }
            }
            return resultProducts;
        }     

        public static HashSet<Product> serachByName(string productName){
            HashSet<Product> resultProducts = new HashSet<Product>();
            List<Store> relevantstores = StoreRepo.GetInstance().GetAll();
            foreach (Store store in relevantstores)
            {
                resultProducts=resultProducts.UnionWith(serachByNameWithStore(store.StoreId, productName));
            }
            return resultProducts;
        }

        public static HashSet<Product> searchByCategory(string category){
            HashSet<Product> resultProducts = new HashSet<Product>();
            List<Store> relevantstores = StoreRepo.GetInstance().GetAll();
            foreach (Store store in relevantstores)
            {
                resultProducts=resultProducts.UnionWith(searchByCategoryWithStore(store.StoreId, category));
            }
            return resultProducts;
        }

        public static HashSet<Product> searchByKeyword(string keyword){
            HashSet<Product> resultProducts = new HashSet<Product>();
            List<Store> relevantstores = StoreRepo.GetInstance().GetAll();
            foreach (Store store in relevantstores)
            {
                resultProducts=resultProducts.UnionWith(searchByKeywordWithStore(store.StoreId, keyword));
            }
            return resultProducts;
        }
    }
}