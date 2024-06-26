using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MarketBackend.Domain.Market_Client;
namespace Market.DataLayer.DTOs{
    [Table("Products")]
    public class ProductDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }
        [Required]
        [ForeignKey("StoreDTO")]
        public int StoreId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string SellMethod { get; set; }
        public double ProductRating { get; set; }

        public ProductDTO(int id,int storeId, string name, double price, int quantity, string category, string description, string keywords, string sellMethod, double productRating)
        {
            ProductId = id;
            StoreId = storeId;
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
            Description = description;
            Keywords = keywords;
            SellMethod = sellMethod;
            ProductRating = productRating;
        }
        public ProductDTO(int id, int storeId, string name, double price, int quantity, string category, string description, string keywords)
        {
            ProductId = id;
            StoreId = storeId;
            Name = name;
            Price = price;
            Quantity = quantity;
            Category = category;
            Description = description;
            Keywords = keywords;
            SellMethod = "RegularSell";
            ProductRating = 0;
        }

        public ProductDTO(Product product) {
            ProductId = product.ProductId;
            StoreId = product.StoreId;
            Name = product.Name;
            Price = product.Price;
            Quantity = product.Quantity;
            Category = product.Category.ToString();
            Description = product.Description;
            string k = string.Join(", ", product.Keywords.ToArray<string>());
            Keywords = k;
            SellMethod = product.SellMethod.GetType().Name;
            ProductRating = product.ProductRating;

        }
    }

}