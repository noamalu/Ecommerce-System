using MarketBackend.Domain.Market_Client;
using System.Collections.Concurrent;
using MarketBackend.Services.Interfaces;

namespace MarketBackend.DAL
{
    public class RuleRepositoryRAM : IRuleRepository
    {
        private static ConcurrentDictionary<int, IRule> _ruleById;

        private static RuleRepositoryRAM _ruleRepo = null;

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
         public void Update(IRule rule)
        {
            _ruleById[rule.Id] = rule;
            rule.Update();
        }

        public void Add(IRule rule)
        {
            _ruleById.TryAdd(rule.Id, rule);
            
        }

        public bool ContainsID(int id)
        {
            if (_ruleById.ContainsKey(id))
                return true;
            else return false;
        }

        public bool ContainsValue(IRule rule)
        {
            if (_ruleById.Contains(new KeyValuePair<int, IRule>(rule.Id, rule)))
                return true;
            else return false;
        }

        public void Delete(int id)
        {
            if (_ruleById.ContainsKey(id))
            {
                _ruleById.TryRemove(id, out IRule removed);
                
            }
            else throw new Exception("Product Id does not exist."); ;
        }

        public void Delete(IRule rule)
        {
            if (_ruleById.ContainsKey(rule.Id))
            {
                _ruleById.TryRemove(rule.Id, out IRule removed);
                
            }
            else throw new Exception("Product Id does not exist."); ;
        }

        public IEnumerable<IRule> getAll()
        {
            return _ruleById.Values.ToList();
        }

        public IRule GetById(int id)
        {
            if (_ruleById.ContainsKey(id))
                return _ruleById[id];
            else
                throw new ArgumentException("Invalid Rule Id.");
        }

        public ConcurrentDictionary<int, IRule> GetShopRules(int storeId)
        {
            ConcurrentDictionary<int, IRule> storeRules = new ConcurrentDictionary<int, IRule>();
            foreach (IRule rule in _ruleById.Values)
            {
                if (rule.storeId == storeId) storeRules.TryAdd(rule.Id, rule);
            }
            return storeRules;
        }


    }
}