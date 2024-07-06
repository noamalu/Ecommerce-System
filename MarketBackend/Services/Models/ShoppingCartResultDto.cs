using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Models;

namespace MarketBackend.Services.Models
{
    public class ShoppingCartResultDto
    {
        public double Price { get; set; }
        public List<BasketResultDto> Baskets { get; set; } = new List<BasketResultDto>();

        private ShoppingCartResultDto() { }

        public static async Task<ShoppingCartResultDto> CreateAsync(ShoppingCart cart)
        {
            var dto = new ShoppingCartResultDto();
            var baskets = await cart.GetBaskets();
            dto.Baskets = baskets.Values.Select(basket => new BasketResultDto(basket)).ToList();
            dto.Price = dto.Baskets.Select(basket => basket.Price).Sum();
            return dto;
        }

        public ShoppingCartResultDto(ShoppingCartHistory cart){
            Baskets = cart._baskets.Values.Select(basket => new BasketResultDto(basket)).ToList();
            Price = cart._products.Select(product => product.Key*product.Value._price).Sum();
        }
    }
}