using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketBackend.Domain.Market_Client
{
    public abstract class IPolicyManager<T> where T : IPolicy
    {
        protected int _storeId;
        private ConcurrentDictionary<int, IPolicy> _policies;

        protected IPolicyManager(int storeId)
        {
            StoreId = storeId;
            _policies = new ConcurrentDictionary<int, IPolicy>();
        }

        public int StoreId { get => _storeId; set => _storeId = value; }
    }
}