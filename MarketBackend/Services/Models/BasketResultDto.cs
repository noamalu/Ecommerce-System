using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.DAL;
using MarketBackend.Domain.Models;

namespace MarketBackend.Services.Models
{
    public class BasketResultDto
    {
        public int StoreId { get; set; }
        public List<ProductResultDto> Products { get; set; }
        public double Price { get; set; }

        public BasketResultDto(Basket basket){
            var store = StoreRepositoryRAM.GetInstance().GetById(StoreId);
            StoreId = basket._storeId;
            Price = store.CalculateBasketPrice(basket);
            Products = store.Products.Where(product => basket.products.Keys.Contains(product.ProductId))
                .Select(product => new ProductResultDto(product){Quantity = basket.products[product.ProductId]}).ToList();
        }
    }
}