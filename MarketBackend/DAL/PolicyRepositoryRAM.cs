using MarketBackend.Domain.Market_Client;
using System.Collections.Concurrent;
using MarketBackend.Services.Interfaces;
using MarketBackend.DAL.DTO;


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
            DBcontext.GetInstance().Stores.Find(policy.StoreId).Policies.Add(policy.CloneDTO());
            DBcontext.GetInstance().SaveChanges();
        }

        public bool ContainsID(int id)
        {
            if (_policyById.ContainsKey(id))
                return true;
            else return DBcontext.GetInstance().Stores.Any(s => s.Rules.Any(r => r.Id.Equals(id)));
        }

        public bool ContainsValue(IPolicy policy)
        {
            if (_policyById.Contains(new KeyValuePair<int, IPolicy>(policy.Id, policy)))
                return true;
            else return DBcontext.GetInstance().Stores.Any(s => s.Policies.Any(r => r.Id.Equals(policy.Id)));
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
            else if (ContainsID(id))
            {
                DBcontext context = DBcontext.GetInstance();
                StoreDTO shopDto = context.Stores.Where(s => s.Policies.Any(p => p.Id == id)).FirstOrDefault();
                PolicyDTO policyDTO = shopDto.Policies.Find(p => p.Id == id);
                _policyById.TryAdd(id, makePolicy(policyDTO));
                return _policyById[id];
            }
            else
                throw new ArgumentException("Invalid Rule Id.");
        }

        public IEnumerable<IPolicy> getAll()
        {
            UploadRulesFromContext();
            return _policyById.Values.ToList();
        }

        
        public void Delete(int id)
        {
            if (_policyById.ContainsKey(id))
            {
                StoreDTO store = DBcontext.GetInstance().Stores.Find(_policyById[id].StoreId);
                _policyById.TryRemove(id, out IPolicy removed);
                PolicyDTO p = store.Policies.Find(p => p.Id == id);
                store.Policies.Remove(p);
                DBcontext.GetInstance().Policies.Remove(p);
                DBcontext.GetInstance().SaveChanges();
            }
            else throw new Exception("Product Id does not exist."); ;
        }

        public void Update(IPolicy policy)
        {
            
        }

        private void UploadRulesFromContext()
        {
            List<StoreDTO> stores = DBcontext.GetInstance().Stores.ToList();
            foreach (StoreDTO storeDTO in stores)
            {
                UploadShopPoliciesFromContext(storeDTO.Id);
            }
        }

        private void UploadShopPoliciesFromContext(int storeId)
        {
            StoreDTO storeDto = DBcontext.GetInstance().Stores.Find(storeId);
            if (storeDto != null)
            {
                if (storeDto.Rules != null)
                {
                    List<PolicyDTO> policies = storeDto.Policies.ToList();
                    foreach (PolicyDTO policyDTO in policies)
                    {
                        _policyById.TryAdd(policyDTO.Id, makePolicy(policyDTO));
                    }
                }
            }
        }

        public IPolicy makePolicy(PolicyDTO policyDTO)
        {
            Type policyType = policyDTO.GetType();
            if (policyType.Name.Equals("DiscountPolicyDTO"))
            {
                return new DiscountPolicy((DiscountPolicyDTO)policyDTO);
            }
            else if (policyType.Name.Equals("PurchasePolicyDTO"))
            {
                return new PurchasePolicy((PurchasePolicyDTO)policyDTO);
            }
            else if (policyType.Name.Equals("DiscountCompositePolicyDTO"))
            {
                List<IPolicy> policies = new List<IPolicy>();
                foreach (PolicyDTO p in ((DiscountCompositePolicyDTO)policyDTO).Policies)
                {
                    policies.Add(makePolicy(p));
                }
                return new DiscountCompositePolicy((DiscountCompositePolicyDTO)policyDTO, policies);
            }
            return null;
        }
        public void Clear()
        {
            _policyById.Clear();
        }
        public void ResetDomainData()
        {
            _policyById = new ConcurrentDictionary<int, IPolicy>();
        }

        public void Delete(IPolicy policy)
        {
            Delete(policy.Id);
        }

        public void Dispose()
        {
             _policyById = new ConcurrentDictionary<int, IPolicy>();
        }
    }
}