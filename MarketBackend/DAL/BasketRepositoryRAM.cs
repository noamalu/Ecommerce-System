using System;
using System.Collections.Generic;
using System.Linq;
using MarketBackend.Domain.Models;
using MarketBackend.Services.Interfaces;

namespace MarketBackend.DAL
{
    public class BasketRepositoryRAM : IBasketRepository
    {
        private readonly Dictionary<int, Basket> baskets;

        public BasketRepositoryRAM()
        {
            baskets = new Dictionary<int, Basket>();
        }

        public void Add(Basket entity)
        {
            if(baskets.ContainsKey(entity._basketId)){
                throw new ArgumentException($"Basket with ID {entity._basketId} already exists.");

            }
            baskets.Add(entity._basketId, entity);
        }

        public void Delete(Basket entity)
        {
            if (!baskets.ContainsKey(entity._basketId)){
                throw new KeyNotFoundException($"Basket with ID {entity._basketId} does not exist.");
            }

            baskets.Remove(entity._basketId);
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
