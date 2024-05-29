 namespace MarketBackend.Domain.Market_Client
{
 public abstract class IPolicy
    {
        private int _id;
        private DateTime _expirationDate;
        private int _storeId;

        public int Id { get => _id; set => _id = value; }
        public DateTime ExpirationDate { get => _expirationDate; set => _expirationDate = value; }
        public int StoreId { get => _storeId; set => _storeId = value; }
    }
}
