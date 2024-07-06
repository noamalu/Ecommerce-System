using MarketBackend.Domain.Market_Client;
using System.Collections.Concurrent;
using MarketBackend.Services.Interfaces;
using MarketBackend.DAL.DTO;

namespace MarketBackend.DAL
{
    public class PolicyRepositoryRAM : IPolicyRepository {
        private static ConcurrentDictionary<int, IPolicy> _policyById;

        private static PolicyRepositoryRAM _policyRepo = null;
        private DBcontext dbcontext;
        private PolicyDTO policyDTO;

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

        public async Task Add(IPolicy policy)
        {
            dbcontext = DBcontext.GetInstance();
            await dbcontext.PerformTransactionalOperationAsync(async () =>
            {
                PolicyDTO policyDTO = policy.CloneDTO();
                dbcontext.Stores.Find(policy.StoreId).Policies.Add(policyDTO);
            });
            _policyById.TryAdd(policy.Id, policy);
        }

        public async Task<bool> ContainsID(int id)
        {
            if (_policyById.ContainsKey(id))
                return true;
            else return DBcontext.GetInstance().Stores.Any(s => s.Rules.Any(r => r.Id.Equals(id)));
        }

        public async Task<bool> ContainsValue(IPolicy policy)
        {
            if (_policyById.Contains(new KeyValuePair<int, IPolicy>(policy.Id, policy)))
                return true;
            else return DBcontext.GetInstance().Stores.Any(s => s.Policies.Any(r => r.Id.Equals(policy.Id)));
        }

        public async Task<ConcurrentDictionary<int, IPolicy>> GetShopRules(int storeId)
        {
            ConcurrentDictionary<int, IPolicy> storePolicies = new ConcurrentDictionary<int, IPolicy>();
            foreach (IPolicy policy in _policyById.Values)
            {
                if (policy.StoreId == storeId) storePolicies.TryAdd(policy.Id, policy);
            }
            return storePolicies;
        }

         public async Task<IPolicy> GetById(int id)
        {
            if (_policyById.ContainsKey(id))
                return _policyById[id];
            else if (await ContainsID(id))
            {
                DBcontext context = DBcontext.GetInstance();
                await dbcontext.PerformTransactionalOperationAsync(async () =>
                {
                    StoreDTO shopDto = context.Stores.Where(s => s.Policies.Any(p => p.Id == id)).FirstOrDefault();
                    policyDTO = shopDto.Policies.Find(p => p.Id == id);
                });
                _policyById.TryAdd(id, await makePolicy(policyDTO));
                return _policyById[id];
            }
            else
                throw new ArgumentException("Invalid Rule Id.");
        }

        public async Task<IEnumerable<IPolicy>> getAll()
        {
            UploadRulesFromContext();
            return _policyById.Values.ToList();
        }

        
        public async Task Delete(int id)
        {
            if (_policyById.ContainsKey(id))
            {
                DBcontext dbcontext = DBcontext.GetInstance();
                await dbcontext.PerformTransactionalOperationAsync(async () =>
                {
                    StoreDTO store = dbcontext.Stores.Find(_policyById[id].StoreId);
                    PolicyDTO p = store.Policies.Find(p => p.Id == id);
                    store.Policies.Remove(p);
                    DBcontext.GetInstance().Policies.Remove(p);
                    _policyById.TryRemove(id, out IPolicy removed);
                });
            }
            else throw new Exception("Product Id does not exist."); ;
        }

        public async Task Update(IPolicy policy)
        {
            
        }

        private async Task UploadRulesFromContext()
        {
            DBcontext dbcontext = DBcontext.GetInstance();
            await dbcontext.PerformTransactionalOperationAsync(async () =>
            {
                List<StoreDTO> stores = DBcontext.GetInstance().Stores.ToList();
                foreach (StoreDTO storeDTO in stores)
                {
                    await UploadShopPoliciesFromContext(storeDTO.Id);
                }
            });
        }

        private async Task UploadShopPoliciesFromContext(int storeId)
        {
            DBcontext dbcontext = DBcontext.GetInstance();
            await dbcontext.PerformTransactionalOperationAsync(async () =>
            {
                StoreDTO storeDto = dbcontext.Stores.Find(storeId);
                if (storeDto != null)
                {
                    if (storeDto.Rules != null)
                    {
                        List<PolicyDTO> policies = storeDto.Policies.ToList();
                        foreach (PolicyDTO policyDTO in policies)
                        {
                            _policyById.TryAdd(policyDTO.Id, await makePolicy(policyDTO));
                        }
                    }
                }
            });
        }

        public async Task<IPolicy> makePolicy(PolicyDTO policyDTO)
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
                    policies.Add(await makePolicy(p));
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

        public async Task Delete(IPolicy policy)
        {
            await Delete(policy.Id);
        }

        public static void Dispose()
        {
             _policyById = new ConcurrentDictionary<int, IPolicy>();
        }
    }
}