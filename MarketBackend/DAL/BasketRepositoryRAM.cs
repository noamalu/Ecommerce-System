using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MarketBackend.Domain.Models;
using MarketBackend.Services.Interfaces;

namespace MarketBackend.DAL
{
    public class BasketRepositoryRAM : IBasketRepository
    {
        private readonly ConcurrentDictionary<int, Basket> baskets;

        private static BasketRepositoryRAM _basketRepository = null;
        private object Lock;

        private BasketRepositoryRAM()
        {
            baskets = new ConcurrentDictionary<int, Basket>();
            Lock = new object();
        }
        public static BasketRepositoryRAM GetInstance()
        {
            _basketRepository ??= new BasketRepositoryRAM();
            return _basketRepository;
        }

        public void Add(Basket entity)
        {
            if(baskets.ContainsKey(entity._basketId)){
                throw new ArgumentException($"Basket with ID {entity._basketId} already exists.");

            }
            baskets.TryAdd(entity._basketId, entity);
        }

        public void Delete(Basket entity)
        {
            if (!baskets.ContainsKey(entity._basketId)){
                throw new KeyNotFoundException($"Basket with ID {entity._basketId} does not exist.");
            }

            baskets.TryRemove(new KeyValuePair<int, Basket>(entity._basketId, entity));
        }

        public IEnumerable<Basket> getAll()
        {
            return baskets.Values.ToList();
        }

        public IEnumerable<Basket> getBasketsByCartId(int cartId)
        {
            return baskets.Values.Where(basket => basket._cartId == cartId).ToList();
        }

        public Basket GetById(int id){
            if (!baskets.ContainsKey(id)){
                throw new KeyNotFoundException($"Basket with ID {id} does not exist.");
            }
            return baskets[id];
    }

        public void Update(Basket entity)
        {
            if (baskets.ContainsKey(entity._basketId))
            {
                baskets[entity._basketId] = entity;
            }
            else
            {
                throw new KeyNotFoundException($"Basket with ID {entity._basketId} not found.");
            }
        }
    }
}
