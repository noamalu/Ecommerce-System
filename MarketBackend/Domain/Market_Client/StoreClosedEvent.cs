
namespace MarketBackend.Domain.Market_Client
{
    public class StoreClosedEvent : Event
    {
        private int _member;
        private Store _store;

        public StoreClosedEvent(Store store, int member) : base("Store Closed Event")
        {
            _member = member;
            _store = store;
        }

        public override string GenerateMsg()
        {
            return $"{Name}: Member: {_member} Closed the store {_store._storeName}.";
        }
    }
}