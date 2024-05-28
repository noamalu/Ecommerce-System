
namespace MarketBackend.Domain.Market_Client
{
    public class RemoveAppointmentEvent : Event
    {
        private int _member;
        private int _removedMember;
        private Store _store;

        public RemoveAppointmentEvent(Store store, int member, int removedMember) : base("Remove Appointment Event")
        {
            _store = store;
            _member = member;
            _removedMember = removedMember;
        }

        public override string GenerateMsg()
        {
            return $"{Name}: Member: \'{_member}\' removed \'{_removedMember}\' appointment " +
                $"from store {_store._storeName}";
        }
    }
}