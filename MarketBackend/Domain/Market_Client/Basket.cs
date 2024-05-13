using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.Services.Interfaces;

namespace MarketBackend.Domain.Models
{
    public class Basket
    {
        private readonly int _basketId;
        private readonly IStore _store;
        private readonly Dictionary<int, int> products;
        public Basket(int basketId, IStore store){
            _basketId = basketId; 
            _store = store;
            products = new Dictionary<int, int>();
        }

        public double calculateTotalPrice(){
            return products.Select(pair => _store.getProductPrice(pair.Key) * pair.Value).Sum();

        }
        public bool purchaseCart(){
        return _store.purchaseCart(this);

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
                throw new ArgumentException($"Product id={productId} not in the basket!");
            }
        }

    }
}