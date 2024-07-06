using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.Domain.Models;
using MarketBackend.Domain.Market_Client;

namespace MarketBackend.Services.Interfaces
{
    public interface IBasketRepository : IRepository<Basket>
    {
        public Task<IEnumerable<Basket>> getBasketsByCartId(int cartId);
        public Task<Basket> CreateBasket(int storeId, int cartId);
        public Basket? TryGetById(int id);
    }
}