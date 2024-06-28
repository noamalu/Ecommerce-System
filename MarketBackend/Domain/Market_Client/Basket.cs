using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.DAL;
using MarketBackend.DAL.DTO;
using MarketBackend.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace MarketBackend.Domain.Market_Client
{
    public class Basket
    {
        public int _basketId{get; set;}
        public int _storeId{get; set;}
        public int _cartId{get; set;}
        public Dictionary<int, int> products{get;} //product id to quantity
        private SynchronizedCollection<BasketItem> _basketItems;
        public SynchronizedCollection<BasketItem> BasketItems { get => _basketItems; set => _basketItems = value; }

        public Basket(int basketId, int storeId){
            _basketId = basketId; 
            _storeId = storeId;
            products = new Dictionary<int, int>();
            _basketItems = new SynchronizedCollection<BasketItem>();
        }

        public Basket(BasketDTO other)
        {
            _basketId = other._basketId;
            _storeId = other._storeId;
            products = other.products;
            _basketItems = new(other.BasketItems);            
        }

        public void addToBasket(int productId, int quantity){
            if (products.ContainsKey(productId)){
                products[productId] += quantity;
                FindBasketItem(productId).Quantity += quantity;
            }
            else{
                products[productId] = quantity;
                _basketItems.Add(new BasketItem(ProductRepositoryRAM.GetInstance().GetById(productId), quantity)) ;
            }
        }

        public void RemoveFromBasket(int productId, int quantity){
            if (products.ContainsKey(productId)){
                products[productId] = Math.Max(products[productId]-quantity,0);
                _basketItems.Remove(FindBasketItem(productId));
                products.Remove(productId);
            }
            else{
                throw new ArgumentException($"Product id={productId} not in the {_basketId}!");
            }
        }

        public BasketItem GetBasketItem(Product product)
        {
            foreach(BasketItem basketItem in _basketItems)
            {
                if (basketItem.Product == product)
                {
                    return basketItem;
                }
            }
            return null;
        }

        public BasketItem FindBasketItem(int productId)
        {
            foreach (BasketItem basketItem in _basketItems)
            {
                if (basketItem.Product.ProductId == productId) return basketItem;
            }
            return null;
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

        //TODO:
        public string GetInfo()
        {
            throw new NotImplementedException();
        }

        public bool IsEmpty()
        {
            return products.IsNullOrEmpty();
        }
        public bool HasProduct(Product p)
        {
            foreach(BasketItem basketItem in _basketItems)
            {
                if(basketItem.Product == p) return true;
            }
            return false;
        }   
        public double GetBasketPriceBeforeDiscounts()
        {
            double price = 0;
            foreach (BasketItem basketItem in _basketItems)
                price += basketItem.Product.Price * basketItem.Quantity;
            return price;
        }
        public void resetDiscount()
        {
            foreach (BasketItem basketItem in _basketItems)
            {
                if(basketItem.Product._sellMethod is RegularSell)
                    basketItem.PriceAfterDiscount = basketItem.Product.Price;
            }
        }
    }
}