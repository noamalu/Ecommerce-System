using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.DAL;
using MarketBackend.Domain.Market_Client;

namespace MarketBackend.Services.Models
{
    public class BasketResultDto
    {
        public int StoreId { get; set; }
        public List<ProductResultDto> Products { get; set; }
        public double Price { get; set; }
        private Store store;

        public BasketResultDto(Basket basket){
            asyncBasketResultDto(basket);
        }

        public async void asyncBasketResultDto(Basket basket){
            StoreId = basket._storeId;
            store = await StoreRepositoryRAM.GetInstance().GetById(StoreId);
            Price = store.CalculateBasketPrice(basket);
            Products = store.Products.Where(product => basket.products.Keys.Contains(product.ProductId))
                .Select(product => new ProductResultDto(product){Quantity = basket.products[product.ProductId]}).ToList();
        }
    }
}