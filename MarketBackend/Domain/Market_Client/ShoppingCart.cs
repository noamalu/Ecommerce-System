using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.Services.Interfaces;

namespace MarketBackend.Domain.Models
{
    public class ShoppingCart
    {
        public int _shoppingCartId;
        private readonly IBasketRepository _basketRepository;
        public ShoppingCart(int shoppingCartId,IBasketRepository basketRepository){
            _shoppingCartId = shoppingCartId;
            _basketRepository = basketRepository;
        }

        public void addToCart(int basketId, int productId, int quantity){
            Basket basket = _basketRepository.GetById(basketId);
            basket.addToBasket(productId, quantity);
        }

        public void removeFromCart(int basketId, int productId, int quantity){
            Basket basket = _basketRepository.GetById(basketId);
            basket.RemoveFromBasket(productId, quantity);
        }
    }
}