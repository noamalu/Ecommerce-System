using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.DAL;
using MarketBackend.Domain.Market_Client;
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
            Basket? basket = _basketRepository.TryGetById(basketId) ?? _basketRepository.CreateBasket(basketId, _shoppingCartId);
            basket.addToBasket(productId, quantity);
        }

        public void AddBasketToCart(int basketId, int productId, int quantity){
            Basket? basket = _basketRepository.TryGetById(basketId) ?? _basketRepository.CreateBasket(basketId, _shoppingCartId);
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

        public void PurchaseBasket(int basketId){
            Basket basket = _basketRepository.GetById(basketId);
            _basketRepository.Delete(basket);
        }
    }

    public class ShoppingCartHistory
    {
        public int _shoppingCartId{get; set;}
        private ConcurrentDictionary<int, Basket> _baskets = new();
        public void AddBasket(Basket basket)
        {
            _baskets.TryAdd(basket._basketId, Basket.Clone(basket));
        }
    }
}