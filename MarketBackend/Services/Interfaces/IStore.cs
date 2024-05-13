using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.Domain.Models;

namespace MarketBackend.Services.Interfaces
{
    public interface IStore
    {
        public double getProductPrice(int productId);
        public bool purchaseCart(Basket basket);
    }
}