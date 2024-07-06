using MarketBackend.Domain.Market_Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBackend.Services.Interfaces
{
    public interface IRoleRepository : IRepository<Role>
    {
        public Task<Role> GetById(int storeId, string memberId);
    }
}
