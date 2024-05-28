
namespace MarketBackend.Domain.Market_Client
{
    public class StoreOpenEvent : Event
    {
        private int _member;
        private Store _store;

        public StoreOpenEvent(Store store, int member) : base("Store Open Event")
        {
            _member = member;
            _store = store;
        }

        public override string GenerateMsg()
        {
            return $"{Name}: Member: {_member} Opened the store {_store._storeName}.";
        }
    }
}