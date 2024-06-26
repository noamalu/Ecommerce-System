using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Services.Interfaces;

namespace MarketBackend.DAL
{
    public class ProductRepositoryRAM : IProductRepository
    {
        
        private static ConcurrentDictionary<int, Product> _productById;

        private static ProductRepositoryRAM _productRepo = null;
      

        private ProductRepositoryRAM()
        {
            _productById = new ConcurrentDictionary<int, Product>();
         
        }
        public static ProductRepositoryRAM GetInstance()
        {
            if (_productRepo == null)
                _productRepo = new ProductRepositoryRAM();
            return _productRepo;
        }

        public static void Dispose(){
            _productRepo = new ProductRepositoryRAM();
        }

        public void Add(Product item)
        {
            _productById.TryAdd(item._productId, item);
            
        }

        public bool ContainsID(int id)
        {
            if (_productById.ContainsKey(id))
            {
                return true;
            }
            return false;
        }

        public bool ContainsValue(Product item)
        {
            if (_productById.ContainsKey(item._productId))
            {
                return true;
            }
            return false;
        }

        public void Delete(Product product)
        {
            if (!_productById.TryRemove(new KeyValuePair<int, Product>(product._productId, product)))
            {
               
            }
        }

        public IEnumerable<Product> getAll()
        {
            return _productById.Values.ToList();
        }

        public Product GetById(int id)
        {
            if (_productById.ContainsKey(id))
            {
                return _productById[id];
            }
            else
            {
               return null;
            }
        }

        public void Update(Product product)
        {
            _productById[product.ProductId] = product;
           
        }
        /// <summary>
        /// returns all product of a given shop 
        /// </summary>
        /// <param name="shopId"></param> the Id of the shop
        /// <returns></returns>
        public SynchronizedCollection<Product> GetShopProducts(int shopId)
        {
            SynchronizedCollection<Product> products = new SynchronizedCollection<Product>();
            foreach(Product p in _productById.Values)
            {
                if (p._storeId == shopId) products.Add(p);
            }
            return products;
        }

        public void Clear()
        {
            _productById.Clear();
        }
        public void ResetDomainData()
        {
            _productById = new ConcurrentDictionary<int, Product>();
        }
    }
}