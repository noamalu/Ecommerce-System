using MarketBackend.Domain.Market_Client;
using System.Collections.Concurrent;
using MarketBackend.Services.Interfaces;


namespace MarketBackend.DAL
{
    public class PolicyRepositoryRAM : IPolicyRepository {
         private static ConcurrentDictionary<int, IPolicy> _policyById;

        private static PolicyRepositoryRAM _policyRepo = null;

        private PolicyRepositoryRAM()
        {
            _policyById = new ConcurrentDictionary<int, IPolicy>();
        }
        public static PolicyRepositoryRAM GetInstance()
        {
            if (_policyRepo == null)
                _policyRepo = new PolicyRepositoryRAM();
            return _policyRepo;
        }

        public void Add(IPolicy policy)
        {
            _policyById.TryAdd(policy.Id, policy);
        
        }

        public bool ContainsID(int id)
        {
            if (_policyById.ContainsKey(id))
                return true;
            else return false;
        }

        public bool ContainsValue(IPolicy policy)
        {
            if (_policyById.Contains(new KeyValuePair<int, IPolicy>(policy.Id, policy)))
                return true;
            else return false;
        }

        public ConcurrentDictionary<int, IPolicy> GetShopRules(int storeId)
        {
            ConcurrentDictionary<int, IPolicy> storePolicies = new ConcurrentDictionary<int, IPolicy>();
            foreach (IPolicy policy in _policyById.Values)
            {
                if (policy.StoreId == storeId) storePolicies.TryAdd(policy.Id, policy);
            }
            return storePolicies;
        }

         public IPolicy GetById(int id)
        {
            if (_policyById.ContainsKey(id))
                return _policyById[id];
            else
                throw new ArgumentException("Invalid Rule Id.");
        }

        public IEnumerable<IPolicy> getAll()
        {
            return _policyById.Values.ToList();
        }

        
        public void Delete(IPolicy Policy)
        {
            if (_policyById.ContainsKey(Policy.Id))
            {
                _policyById.TryRemove(Policy.Id, out IPolicy removed);
            }
            else throw new Exception("Product Id does not exist."); ;
        }

        public void Delete(int Id)
        {
            if (_policyById.ContainsKey(Id))
            {
                _policyById.TryRemove(Id, out IPolicy removed);
            }
            else throw new Exception("Product Id does not exist."); ;
        }

         public void Update(IPolicy policy)
        {
            _policyById[policy.Id] = policy;
        }

        public void Dispose()
        {
           _policyRepo = null;
        }
    }
}