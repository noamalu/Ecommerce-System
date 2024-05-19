using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.DAL;
using MarketBackend.Services.Interfaces;

namespace MarketBackend.Domain.Models
{
    public class ShoppingCart
    {
        public int _shoppingCartId;
        private readonly IBasketRepository _basketRepository;
        public ShoppingCart(int shoppingCartId){
            _shoppingCartId = shoppingCartId;
            _basketRepository = BasketRepositoryRAM.GetInstance();
        }

        public void addToCart(int basketId, int productId, int quantity){
            Basket basket = _basketRepository.GetById(basketId);
            basket.addToBasket(productId, quantity);
        }

        public void removeFromCart(int basketId, int productId, int quantity){
            Basket basket = _basketRepository.GetById(basketId);
            basket.RemoveFromBasket(productId, quantity);
        }

        public Dictionary<int, Basket> GetBaskets() //returns a dictionary of store id to productIdxQuantity
        {
            var baskets = _basketRepository.getBasketsByCartId(_shoppingCartId);
            var retBaskets = new Dictionary<int, Basket>();
            foreach (var basket in baskets) {
                retBaskets.Add(basket._storeId, basket);
            };
            return retBaskets;
        }
    }
}