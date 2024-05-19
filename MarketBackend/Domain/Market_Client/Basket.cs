using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.Services.Interfaces;

namespace MarketBackend.Domain.Models
{
    public class Basket
    {
        public int _basketId{get; set;}
        public int _storeId{get; set;}
        public int _cartId{get; set;}
        public Dictionary<int, int> products{get;} //product id to quantity
        public Basket(int basketId, int storeId){
            _basketId = basketId; 
            _storeId = storeId;
            products = new Dictionary<int, int>();
        }

        public void addToBasket(int productId, int quantity){
            if (products.ContainsKey(productId)){
                products[productId] += quantity;
            }
            else{
                products[productId] = quantity;
            }
        }

        public void RemoveFromBasket(int productId, int quantity){
            if (products.ContainsKey(productId)){
                products[productId] = Math.Max(products[productId]-quantity,0);
            }
            else{
                throw new ArgumentException($"Product id={productId} not in the {_basketId}!");
            }
        }

        public static Basket Clone(Basket basketToClone)
        {
            var newBasket = new Basket(basketToClone._basketId, basketToClone._storeId)
            {
                _cartId = basketToClone._cartId
            };            

            foreach (var product in basketToClone.products)
            {
                newBasket.products[product.Key] = product.Value;
            }

            return newBasket;
        }
    }
}