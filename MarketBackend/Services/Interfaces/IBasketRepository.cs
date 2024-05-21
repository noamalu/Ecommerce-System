using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.Domain.Models;

namespace MarketBackend.Services.Interfaces
{
    public interface IBasketRepository : IRepository<Basket>
    {
        public IEnumerable<Basket> getBasketsByCartId(int cartId);
        public Basket CreateBasket(int storeId, int cartId);
        public Basket? TryGetById(int id);
    }
}