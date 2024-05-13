using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.Services.Interfaces;

namespace MarketBackend.Domain.Models
{
    public class ShoppingCart
    {
        private readonly int _shoppingCartId;
        private readonly IBasketRepository _basketRepository;
        public ShoppingCart(int shoppingCartId,IBasketRepository basketRepository){
            _shoppingCartId = shoppingCartId;
            _basketRepository = basketRepository;
        }

        public double calculateTotalPrice(){
            return _basketRepository.getBasketsByCartId(_shoppingCartId).Select(basket => basket.calculateTotalPrice()).Sum();
        }

        public bool purchaseCart(){
            return _basketRepository.getBasketsByCartId(_shoppingCartId).All(basket => basket.purchaseCart());
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