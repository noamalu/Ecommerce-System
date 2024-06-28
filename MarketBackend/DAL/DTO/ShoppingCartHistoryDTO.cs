using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.Domain.Models;
using MarketBackend.Domain.Market_Client;

namespace MarketBackend.DAL.DTO
{
    [Table("ShoppingCartHistory")]
    public class ShoppingCartHistoryDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int  ShoppingCartId { get; set; }
        public Dictionary<int, BasketDTO> _baskets = new();
        public Dictionary<int, ProductDTO> _products = new();        

        public ShoppingCartHistoryDTO(ShoppingCartHistory cart) {
            ShoppingCartId = cart._shoppingCartId;
            _baskets = new();
            foreach(var basket in cart._baskets)
                _baskets.Add(basket.Key, DBcontext.GetInstance().Baskets.Find(basket.Key));
            _products = new();
            foreach(var product in cart._products)
                _products.Add(product.Key, DBcontext.GetInstance().Products.Find(product.Key));
        }
    }
}