namespace MarketBackend.Domain.Market_Client
{
  public class PurchasePolicyManager : IPolicyManager<PurchasePolicy>
    {
        public PurchasePolicyManager(int storeId):base(storeId)
        {
        }
    }
}