using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.DAL;
using MarketBackend.DAL.DTO;
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

        public async Task addToCart(int storeId, int productId, int quantity){
            Basket? basket = (await GetBaskets()).Values.Where(basket => basket._storeId == storeId).FirstOrDefault() ?? await _basketRepository.CreateBasket(storeId, _shoppingCartId);
            await basket.addToBasket(productId, quantity);
        }

        public async Task removeFromCart(int storeId, int productId, int quantity){
            var basket = (await GetBaskets()).Values.Where(basket => basket._storeId == storeId).FirstOrDefault();
            await basket.RemoveFromBasket(productId, quantity);
        }

        public async Task<Dictionary<int, Basket>> GetBaskets() //returns a dictionary of store id to productIdxQuantity
        {
            var baskets = await _basketRepository.getBasketsByCartId(_shoppingCartId);
            var retBaskets = new Dictionary<int, Basket>();
            foreach (var basket in baskets) {
                retBaskets.Add(basket._storeId, basket);
            };
            return retBaskets;
        }

        public async void PurchaseBasket(int basketId){
            Basket basket = await _basketRepository.GetById(basketId);
            _basketRepository.Delete(basket);
        }
    }

    public class ShoppingCartHistory
    {
        public int _shoppingCartId{get; set;}
        public ConcurrentDictionary<int, Basket> _baskets = new();
        public ConcurrentDictionary<int, Product> _products = new();

        public ShoppingCartHistory(){}
        public ShoppingCartHistory(ShoppingCartHistoryDTO other)
        {
            _shoppingCartId = other.ShoppingCartId;
            _baskets = new(other._baskets.Select(basket => new Basket(basket)).ToDictionary(basket => basket._storeId));
            _products = new(other._products.Select(product => new Product(product)).ToDictionary(product => product.ProductId));
        }

        public async void AddBasket(Basket basket)
        {
            _baskets.TryAdd(basket._basketId, Basket.Clone(basket));
            foreach(var product in basket.products)
            {
                var storeProducts = await ProductRepositoryRAM.GetInstance().GetStoreProducts(basket._storeId);
                var productDetailsFromStore = storeProducts.Where(p => p.ProductId == product.Key).FirstOrDefault().Clone();
                if(productDetailsFromStore is not null) _products.TryAdd(product.Key, productDetailsFromStore);
            }
            
        }
    }
}