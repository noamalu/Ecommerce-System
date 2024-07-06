using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using MarketBackend.DAL;
using MarketBackend.Domain.Models;

namespace MarketBackend.Domain.Market_Client{
    public static class SearchingManager
    {
        public static async Task<HashSet<Product>> serachByNameWithStore(int storeId, string productName)
        {
            HashSet<Product> resultProducts = new HashSet<Product>();
            Store store = await StoreRepositoryRAM.GetInstance().GetById(storeId);
            if (store == null)
            {
                return resultProducts;
            }
            return store.SearchByName(productName);
        }

        public static async Task<HashSet<Product>> searchByCategoryWithStore(int storeId, string category)
        {
            HashSet<Product> resultProducts = new HashSet<Product>();
            Store store = await StoreRepositoryRAM.GetInstance().GetById(storeId);
            if (store == null)
            {
                return resultProducts;
            }
            return store.SearchByCategory(category);
        }

        public static async Task<HashSet<Product>> searchByKeywordWithStore(int storeId, string keyword)
        {
            HashSet<Product> resultProducts = new HashSet<Product>();
            Store store = await StoreRepositoryRAM.GetInstance().GetById(storeId);
            if (store == null)
            {
                return resultProducts;
            }
            return store.SearchByKeywords(keyword);
        }     

        public static async Task<HashSet<Product>> serachByName(string productName){
            HashSet<Product> resultProducts = new HashSet<Product>();
            List<Store> relevantstores = (await StoreRepositoryRAM.GetInstance().getAll()).ToList();
            foreach (Store store in relevantstores)
            {
                resultProducts.UnionWith(await serachByNameWithStore(store.StoreId, productName));
            }
            return resultProducts;
        }

        public static async Task<HashSet<Product>> searchByCategory(string category){
            HashSet<Product> resultProducts = new HashSet<Product>();
            List<Store> relevantstores = (await StoreRepositoryRAM.GetInstance().getAll()).ToList();
            foreach (Store store in relevantstores)
            {
                resultProducts.UnionWith(await searchByCategoryWithStore(store.StoreId, category));
            }
            return resultProducts;
        }

        public static async Task<HashSet<Product>> searchByKeyword(string keyword){
            HashSet<Product> resultProducts = new HashSet<Product>();
            List<Store> relevantstores = (await StoreRepositoryRAM.GetInstance().getAll()).ToList();
            foreach (Store store in relevantstores)
            {
                resultProducts.UnionWith(await searchByKeywordWithStore(store.StoreId, keyword));
            }
            return resultProducts;
        }
    }
}