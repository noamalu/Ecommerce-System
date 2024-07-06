using MarketBackend.DAL;

namespace MarketBackend.Domain.Market_Client
{
  public class PurchasePolicyManager : IPolicyManager<PurchasePolicy>
    {
        public PurchasePolicyManager(int storeId):base(storeId)
        {
        }

        public int AddPolicy(int id, DateTime expirationDate,RuleSubject subject, IRule rule)
        {
            int unicId = int.Parse($"{_storeId}{id}");
            PurchasePolicy policy = new PurchasePolicy(unicId, StoreId, expirationDate, subject, rule);
            Policies.TryAdd(policy.Id, policy);
            PolicyRepositoryRAM.GetInstance().Add(policy);
            return policy.Id;
        }
        public override PurchasePolicy GetPolicy(int policyId)
        {
            if (!Policies.TryGetValue(policyId, out IPolicy policy))
            {
                // This part assumes that ContainsID and GetById are asynchronous methods.
                var containsIdTask = PolicyRepositoryRAM.GetInstance().ContainsID(policyId);
                containsIdTask.Wait(); // Wait for the task to complete

                if (containsIdTask.Result) // Access the result of the task
                {
                    var getByIdTask = PolicyRepositoryRAM.GetInstance().GetById(policyId);
                    getByIdTask.Wait(); // Wait for the task to complete

                    if (getByIdTask.Result is PurchasePolicy purchasePolicy)
                    {
                        return purchasePolicy;
                    }
                    else
                    {
                        throw new Exception("Policy was found but it is not a PurchasePolicy");
                    }
                }
                throw new Exception("Policy was not found");
            }
            return (PurchasePolicy)policy;
        }
    }
}