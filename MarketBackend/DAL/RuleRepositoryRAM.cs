using MarketBackend.Domain.Market_Client;
using System.Collections.Concurrent;
using MarketBackend.Services.Interfaces;
using MarketBackend.DAL.DTO;

namespace MarketBackend.DAL
{
    public class RuleRepositoryRAM : IRuleRepository
    {
        private static ConcurrentDictionary<int, IRule> _ruleById;

        private static RuleRepositoryRAM _ruleRepo = null;
        private DBcontext dBcontext;

        private RuleRepositoryRAM()
        {
            _ruleById = new ConcurrentDictionary<int, IRule>();
        }
        public static RuleRepositoryRAM GetInstance()
        {
            if (_ruleRepo == null)
                _ruleRepo = new RuleRepositoryRAM();
            return _ruleRepo;
        }
        public async Task Update(IRule rule)
        {
            _ruleById[rule.Id] = rule;
            rule.Update();
        }

         public async Task Update(SimpleRule rule)
        {
            dBcontext = DBcontext.GetInstance();
            await dBcontext.PerformTransactionalOperationAsync(async () =>
            {
                StoreDTO shopDto = dBcontext.Stores.Where(s => s.Rules.Any(r => r.Id == rule.Id)).FirstOrDefault();
                SimpleRuleDTO ruleDTO = (SimpleRuleDTO)shopDto.Rules.Find(r => r.Id == rule.Id);
            });
        }
        public async Task Update(QuantityRule rule)
        {
            
        }
        public async Task Update(CompositeRule rule)
        {
            
        }
        public async Task Update(TotalPriceRule rule)
        {

        }

        public async Task Add(IRule rule)
        {
            dBcontext = DBcontext.GetInstance();
            await dBcontext.PerformTransactionalOperationAsync(async () =>
            {
                RuleDTO ruleDTO = rule.CloneDTO();
                dBcontext.Stores.Find(rule.storeId).Rules.Add(ruleDTO);
            });
            _ruleById.TryAdd(rule.Id, rule);
            // RuleDTO ruleDTO = rule.CloneDTO();
            // DBcontext.GetInstance().Stores.Find(rule.storeId).Rules.Add(ruleDTO);
            // DBcontext.GetInstance().SaveChanges();
            
        }

        public async Task<bool> ContainsID(int id)
        {
            if (_ruleById.ContainsKey(id))
                return true;
            else return false;
        }

        public async Task<bool> ContainsValue(IRule rule)
        {
            if (_ruleById.Contains(new KeyValuePair<int, IRule>(rule.Id, rule)))
                return true;
            else return false;
        }

        public async Task Delete(int id)
        {
            if (_ruleById.ContainsKey(id))
            {
                
                dBcontext = DBcontext.GetInstance();
                await dBcontext.PerformTransactionalOperationAsync(async () =>
                {
                    StoreDTO store =  dBcontext.Stores.Find(_ruleById[id].storeId);
                    store.Rules.Remove(store.Rules.Find(r=>r.Id==id));
                });
                _ruleById.TryRemove(id, out IRule removed);
                
            }
            else throw new Exception("Product Id does not exist."); ;
        }

        public async Task Delete(IRule rule)
        {
            if (_ruleById.ContainsKey(rule.Id))
            {
                _ruleById.TryRemove(rule.Id, out IRule removed);
                
            }
            else throw new Exception("Product Id does not exist."); ;
        }

        public async Task<IEnumerable<IRule>> getAll()
        {
            await UploadRulesFromContext();
            return _ruleById.Values.ToList();
        }

        private async Task UploadRulesFromContext()
        {
            dBcontext = DBcontext.GetInstance();
            await dBcontext.PerformTransactionalOperationAsync(async () =>
            {
                List<StoreDTO> shops = dBcontext.Stores.ToList();
                foreach(StoreDTO storeDTO in shops)
                {
                    UploadShopRulesFromContext(storeDTO.Id);
                }
            });
            
        }

        private async Task UploadShopRulesFromContext(int shopId)
        {
            dBcontext = DBcontext.GetInstance();
            await dBcontext.PerformTransactionalOperationAsync(async () =>
            {
                StoreDTO shopDto = dBcontext.Stores.Find(shopId);
                if (shopDto != null)
                {
                    if (shopDto.Rules != null)
                    {
                        List<RuleDTO> rules = shopDto.Rules.ToList();
                        foreach (RuleDTO ruleDTO in rules)
                        {
                            _ruleById.TryAdd(ruleDTO.Id, await makeRule(ruleDTO));
                        }
                    }
                }
            });
        }

        public async Task<IRule> makeRule(RuleDTO ruleDTO)
        {
            Type ruleType = ruleDTO.GetType();
            if (ruleType.Name.Equals("SimpleRuleDTO"))
            {
                return new SimpleRule((SimpleRuleDTO)ruleDTO);
            }
            else if (ruleType.Name.Equals("QuantityRuleDTO"))
            {
                return new QuantityRule((QuantityRuleDTO)ruleDTO);
            }
            else if (ruleType.Name.Equals("TotalPriceRuleDTO"))
            {
                return new TotalPriceRule((TotalPriceRuleDTO)ruleDTO);
            }
            else if (ruleType.Name.Equals("CompositeRuleDTO"))
            {
                List<IRule> rules = new List<IRule>();
                foreach (RuleDTO r in ((CompositeRuleDTO)ruleDTO).Rules)
                {
                    rules.Add(await makeRule(r));
                }
                return new CompositeRule((CompositeRuleDTO)ruleDTO, rules);
            }
            return null;
        }


        public async Task<IRule> GetById(int id)
        {
            if (_ruleById.ContainsKey(id))
                return _ruleById[id];
            else if (await ContainsID(id))
            {
                DBcontext context = DBcontext.GetInstance();
                StoreDTO storeDto = context.Stores.Where(s => s.Rules.Any(r => r.Id == id)).FirstOrDefault();
                RuleDTO ruleDTO = storeDto.Rules.Find(r=>r.Id==id);
                _ruleById.TryAdd(id, await makeRule(ruleDTO));
                return _ruleById[id];
            }
            else
                throw new ArgumentException("Invalid Rule Id.");
        }

        public async Task<ConcurrentDictionary<int, IRule>> GetStoreRules(int storeId)
        {
            UploadShopRulesFromContext(storeId);
            ConcurrentDictionary<int, IRule> shopRules = new ConcurrentDictionary<int, IRule>();
            foreach (IRule rule in _ruleById.Values)
            {
                if (rule.Id == storeId) shopRules.TryAdd(rule.Id, rule);
            }
            return shopRules;
        }

        public static void Dispose()
        {
            _ruleById = new ConcurrentDictionary<int, IRule>();
        }
    }
}