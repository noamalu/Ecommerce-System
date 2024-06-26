using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MarketBackend.Domain.Market_Client;


namespace MarketBackend.DAL.DTO
{
    [Table("Baskets")]
    public class BasketDTO
    {
        [Key]
        public int _basketId { get; set; }
        [Required]
        [ForeignKey("ShopDTO")]
        public int _storeId { get; set; }
        [Required]
        [ForeignKey("CartDTO")]
        public int _cartId {get; set;}
        public List<BasketItemDTO> BasketItems { get; set; }
        public Dictionary<int, int> products { get; set; }


        public BasketDTO(int shopId, List<BasketItemDTO> basketItems)
        {
            _storeId = shopId;
            BasketItems = basketItems;
        }

        public double TotalPrice { get; set; }
        public BasketDTO() { }
        public BasketDTO(Basket basket) {
            _basketId = basket._basketId;
            _storeId = basket._storeId;
            _cartId = basket._cartId;
            BasketItems = new List<BasketItemDTO>();
            foreach (BasketItem item in basket.BasketItems)
                BasketItems.Add(new BasketItemDTO(item));
            products = basket.products.ToDictionary(entry => entry.Key,
                                               entry => entry.Value);

        }

    }
}