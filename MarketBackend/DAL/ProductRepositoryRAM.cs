using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MarketBackend.DAL.DTO;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarketBackend.DAL
{
    public class ProductRepositoryRAM : IProductRepository
    {
        
        private static ConcurrentDictionary<int, Product> _productById;

        private static ProductRepositoryRAM _productRepo = null;

        private object _lock;
      

        private ProductRepositoryRAM()
        {
            _productById = new ConcurrentDictionary<int, Product>();
            _lock = new object();
         
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
            _productById.TryAdd(item.ProductId, item);
            lock (_lock)
            {
                StoreDTO store = DBcontext.GetInstance().Stores.Include(s => s.Products).FirstOrDefault(s => s.Id == item.StoreId);
                store.Products.Add(new ProductDTO(item));
                DBcontext.GetInstance().SaveChanges();
            }
            
        }

        public bool ContainsID(int id)
        {
            if (!_productById.ContainsKey(id))
            {
                lock (_lock)
                {
                    return DBcontext.GetInstance().Products.Find(id) != null;
                }
            }
            return true;
        }

        public bool ContainsValue(Product item)
        {
           if (!_productById.ContainsKey(item.ProductId))
            {
                lock (_lock)
                {
                    return DBcontext.GetInstance().Products.Find(item.ProductId) != null;
                }
            }
            return true;
        }

        public void Delete(Product product)
        {
            if (_productById.TryRemove(product.ProductId, out Product _))
            {
                lock (_lock)
                {
                    ProductDTO productdto = DBcontext.GetInstance().Products.Find(product.ProductId);
                    DBcontext.GetInstance().Products.Remove(productdto);
                    DBcontext.GetInstance().SaveChanges();
                }
            }
        }

        public IEnumerable<Product> getAll()
        {
            List<Store> stores = StoreRepositoryRAM.GetInstance().getAll().ToList();
            foreach (Store s in stores) UploadStoreProductsFromContext(s.StoreId);
            return _productById.Values.ToList();
        }

        private void UploadStoreProductsFromContext(int storeId)
        {
            lock (_lock)
            {
                StoreDTO store = DBcontext.GetInstance().Stores.Find(storeId);
                if (store != null)
                {
                    List<ProductDTO> products = DBcontext.GetInstance().Stores.Find(storeId).Products;
                    if (products != null)
                    {
                        foreach (ProductDTO product in products)
                        {
                            _productById.TryAdd(product.ProductId, new Product(product));
                        }
                    }
                }
            }
        }

        public Product GetById(int id)
        {
            if (_productById.ContainsKey(id))
            {
                return _productById[id];
            }
            else
            {
                lock (_lock)
                {
                    ProductDTO productDTO = DBcontext.GetInstance().Products.Find(id);
                    if (productDTO != null)
                    {
                        Product product = new Product(productDTO);
                        _productById.TryAdd(id, product);
                        return product;
                    }
                    else
                    {
                        throw new Exception("Invalid product Id.");
                    }
                }
            }
        }

        public void Update(Product product)
        {
            if (_productById.ContainsKey(product.ProductId)){
                _productById[product.ProductId] = product;
                lock (_lock)
                {
                    ProductDTO p = DBcontext.GetInstance().Products.Find(product.ProductId);
                    if (p != null)
                    {
                        if (product.Description != null) p.Description = product.Description;
                        if (product.Category != null) p.Category = product.Category.ToString();
                        if (product.Keywords != null) p.Keywords = string.Join(", ", product.Keywords);
                        p.Quantity = product.Quantity;
                        p.Price = product.Price;
                        DBcontext.GetInstance().SaveChanges();
                    }
                }
            }
            else{
                throw new KeyNotFoundException($"Product with ID {product.ProductId} not found.");
            }
            
        }

        /// <summary>
        /// returns all product of a given shop 
        /// </summary>
        /// <param name="storeId"></param> the Id of the shop
        /// <returns></returns>
        public SynchronizedCollection<Product> GetStoreProducts(int storeId)
        {
            UploadStoreProductsFromContext(storeId);
            SynchronizedCollection<Product> products = new SynchronizedCollection<Product>();
            foreach(Product p in _productById.Values)
            {
                if (p.StoreId == storeId) products.Add(p);
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