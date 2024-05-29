using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.Domain.Market_Client;

namespace MarketBackend.Services.Models
{
    public class ProductResultDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    
        public ProductResultDto(Product product){
            Id = product.ProductId;
            Price = product._price;
            Name = product.Name;
            Description = product.Description;            
        }        
    }
}