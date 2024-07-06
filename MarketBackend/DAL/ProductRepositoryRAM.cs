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
        private DBcontext dbcontext;
        private bool ans;
        private ProductDTO productDTO;
      

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

        public async Task Add(Product item)
        {
            try{
                dbcontext = DBcontext.GetInstance();
                await dbcontext.PerformTransactionalOperationAsync(async () =>
                {
                    StoreDTO store = dbcontext.Stores.Include(s => s.Products).FirstOrDefault(s => s.Id == item.StoreId);
                    store.Products.Add(new ProductDTO(item));
                });
                _productById.TryAdd(item.ProductId, item);
            }
            catch(Exception){
                throw;
            }
                      
        }

        public async Task<bool> ContainsID(int id)
        {
            if (!_productById.ContainsKey(id))
            {
                dbcontext = DBcontext.GetInstance();
                ProductDTO product = dbcontext.Products.Find(id);
                ans = product != null;
                return ans;
            }
            return true;
        }

        public async Task<bool> ContainsValue(Product item)
        {
           if (!_productById.ContainsKey(item.ProductId))
            {
                dbcontext = DBcontext.GetInstance();
                await dbcontext.PerformTransactionalOperationAsync(async () =>
                {
                    ans = dbcontext.Products.Find(item.ProductId) != null;
                });
                return ans;
            }
            return true;
        }

        public async Task Delete(Product product)
        {
            if (_productById.TryRemove(product.ProductId, out Product _))
            {
                dbcontext = DBcontext.GetInstance();
                await dbcontext.PerformTransactionalOperationAsync(async () =>
                {
                    ProductDTO productdto = dbcontext.Products.Find(product.ProductId);
                    dbcontext.Products.Remove(productdto);
                });
            }
        }

        public async Task<IEnumerable<Product>> getAll()
        {
            List<Store> stores = (await StoreRepositoryRAM.GetInstance().getAll()).ToList();
            foreach (Store s in stores) await UploadStoreProductsFromContext(s.StoreId);
            return _productById.Values.ToList();
        }

        private async Task UploadStoreProductsFromContext(int storeId)
        {
            dbcontext = DBcontext.GetInstance();
            await dbcontext.PerformTransactionalOperationAsync(async () =>
            {
                StoreDTO store = dbcontext.Stores.Find(storeId);
                if (store != null)
                {
                    List<ProductDTO> products = dbcontext.Stores.Find(storeId).Products;
                    if (products != null)
                    {
                        foreach (ProductDTO product in products)
                        {
                            _productById.TryAdd(product.ProductId, new Product(product));
                        }
                    }
                }
            });
        }

        public async Task<Product> GetById(int id)
        {
            if (_productById.ContainsKey(id))
            {
                return _productById[id];
            }
            else
            {
                dbcontext = DBcontext.GetInstance();
                await dbcontext.PerformTransactionalOperationAsync(async () =>
                {
                    productDTO = dbcontext.Products.Find(id);
                });
                
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

        public async Task Update(Product product)
        {
            dbcontext = DBcontext.GetInstance();
            await dbcontext.PerformTransactionalOperationAsync(async () =>
            {
                productDTO = dbcontext.Products.Find(product.ProductId);
                if (productDTO != null)
            {
                if (product.Description != null) productDTO.Description = product.Description;
                if (product.Category != null) productDTO.Category = product.Category.ToString();
                if (product.Keywords != null) productDTO.Keywords = string.Join(", ", product.Keywords);
                productDTO.Quantity = product.Quantity;
                productDTO.Price = product.Price;
            }
            });
            
            _productById[product.ProductId] = product;
        }

        /// <summary>
        /// returns all product of a given shop 
        /// </summary>
        /// <param name="storeId"></param> the Id of the shop
        /// <returns></returns>
        public async Task<SynchronizedCollection<Product>> GetStoreProducts(int storeId)
        {
            
            SynchronizedCollection<Product> products = new SynchronizedCollection<Product>();
            foreach(Product p in _productById.Values)
            {
                if (p.StoreId == storeId) products.Add(p);
            }
            return products;
        }

    }
}