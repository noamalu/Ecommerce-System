
namespace MarketBackend.Domain.Market_Client
{
    public class DiscountPolicyManager : IPolicyManager<DiscountPolicy>
    {
        public DiscountPolicyManager(int storeId):base(storeId)
        {
        }
    }
}