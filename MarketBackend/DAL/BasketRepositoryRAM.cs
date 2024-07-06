using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using MarketBackend.DAL.DTO;
using MarketBackend.Domain.Market_Client;
using MarketBackend.Domain.Models;
using MarketBackend.Services.Interfaces;

namespace MarketBackend.DAL
{
    public class BasketRepositoryRAM : IBasketRepository
    {
        private readonly ConcurrentDictionary<int, Basket> baskets;
        private int BasketCounter{get; set;}

        private static BasketRepositoryRAM _basketRepository = null;
        private object Lock;
        private BasketDTO bDto;

        private BasketRepositoryRAM()
        {
            baskets = new ConcurrentDictionary<int, Basket>();
            BasketCounter = 1;
            Lock = new object();
        }
        public static BasketRepositoryRAM GetInstance()
        {
            _basketRepository ??= new BasketRepositoryRAM();
            return _basketRepository;
        }

        public static void Dispose(){
            _basketRepository = new BasketRepositoryRAM();
        }
        public async Task<Basket> CreateBasket(int storeId, int cartId)
        {
            var newBasket = new Basket(BasketCounter, storeId)
            {
                _cartId = cartId
            };
            
            DBcontext dbContext = DBcontext.GetInstance();
            await dbContext.PerformTransactionalOperationAsync(async () =>
            {
                dbContext.Baskets.Add(new BasketDTO(newBasket));
            });
            baskets.TryAdd(newBasket._basketId, newBasket);
            BasketCounter ++;
            return newBasket;
        }

        public async Task Add(Basket entity)
        {
            if(baskets.ContainsKey(entity._basketId)){
                throw new ArgumentException($"Basket with ID {entity._basketId} already exists.");
            }
            DBcontext dbContext = DBcontext.GetInstance();
            await dbContext.PerformTransactionalOperationAsync(async () =>
            {
                dbContext.Baskets.Add(new BasketDTO(entity));
            });
            baskets.TryAdd(entity._basketId, entity);
        }

        public async Task Delete(Basket entity)
        {
            DBcontext dbContext = DBcontext.GetInstance();
            await dbContext.PerformTransactionalOperationAsync(async () =>
            {
                var dbBasket = dbContext.Baskets.Find(entity._basketId);
                if (dbBasket is not null){
                    dbContext.Baskets.Remove(dbBasket);
                    if (baskets.ContainsKey(entity._basketId)){
                        baskets.TryRemove(new KeyValuePair<int, Basket>(entity._basketId, entity));
                    }
                    else{
                        throw new KeyNotFoundException($"Basket with ID {entity._basketId} does not exist.");
            
                    }
                }
            });
        }

        public async Task<IEnumerable<Basket>> getAll()
        {
            return baskets.Values.ToList();
        }

        public async Task<IEnumerable<Basket>> getBasketsByCartId(int cartId)
        {
            return baskets.Values.Where(basket => basket._cartId == cartId).ToList();
        }

        public async Task<Basket> GetById(int id){
            if (baskets.ContainsKey(id)){
                return baskets[id];
            }
            else
            {
                DBcontext dbContext = DBcontext.GetInstance();
                await dbContext.PerformTransactionalOperationAsync(async () =>
                {
                    bDto = dbContext.Baskets.Find(id);
                });
                if (bDto != null)
                    {
                        LoadBasket(bDto);
                        return baskets[id];
                    }
                    throw new ArgumentException("Invalid user ID.");
            }
        }

        public Basket? TryGetById(int id){

            baskets.TryGetValue(id, out Basket? value);
            return value;
        }

        public async Task Update(Basket entity)
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

        private async Task Load()
        {
            var dbContext = DBcontext.GetInstance();
            await dbContext.PerformTransactionalOperationAsync(async () =>
            {
                List<BasketDTO> Lbaskets = dbContext.Baskets.ToList();
                foreach (BasketDTO basket in Lbaskets)
                {
                    baskets.TryAdd(basket.BasketId, new Basket(basket));
                }
            });
            
        }

        private void LoadBasket(BasketDTO basketDTO)
        {
            Basket basket = new Basket(basketDTO);
            baskets[basket._basketId] = basket;
        }

        public async Task Add_cartHistory(ShoppingCartHistory shoppingCartHistory, string memberUserName)
        {
            var dbContext = DBcontext.GetInstance();
            await dbContext.PerformTransactionalOperationAsync(async () =>
            {
                ShoppingCartHistoryDTO shoppingCartHistoryDTO = new ShoppingCartHistoryDTO(shoppingCartHistory);
                MemberDTO memberDTO = dbContext.Members.Where(member => member.UserName == memberUserName).ToList()[0];
                memberDTO.OrderHistory.Add(shoppingCartHistoryDTO);
            });
        }
    }
}
