
namespace MarketBackend.Domain.Market_Client
{
    public class AddAppointmentEvent : Event
    {
        private int _member;
        private int _memberToAdd;
        private Store _store;
        private Role _role;
        public AddAppointmentEvent(Store store, int member, int memberToAdd, Role role) : base("Add Appointment Event")
        {
            _store = store;
            _member = member;
            _memberToAdd = memberToAdd;
            _role = role;
        }

        public override string GenerateMsg()
        {
            return $"{Name}: Member: \'{_member}\' added \'{_memberToAdd}\' " +
                $"role: {_role.getRoleName()} " +
                $"to store: {_store._storeName}";
        }
    }
}
