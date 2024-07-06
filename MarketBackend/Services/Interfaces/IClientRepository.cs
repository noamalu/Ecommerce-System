using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketBackend.Domain.Market_Client;

namespace MarketBackend.Services.Interfaces
{
    public interface IClientRepository : IRepository<Member>
    {
        Task<Member> GetByUserName(string userName);
        Task<bool> ContainsUserName(string userName);
    }
}