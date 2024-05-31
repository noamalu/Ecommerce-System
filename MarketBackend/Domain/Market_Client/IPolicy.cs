 namespace MarketBackend.Domain.Market_Client
{
 public abstract class IPolicy
    {
        private int _id;
        private DateTime _expirationDate;
        private int _storeId;
        private IRule _rule;
        private RuleSubject _subject;

        public int Id { get => _id; set => _id = value; }
        public DateTime ExpirationDate { get => _expirationDate; set => _expirationDate = value; }
        public int StoreId { get => _storeId; set => _storeId = value; }
        public IRule Rule { get => _rule; set => _rule = value; }
        public RuleSubject Subject { get => _subject; set => _subject = value; }

        public IPolicy(int id, int storeId, DateTime expirationDate, RuleSubject subject, IRule rule)
        {
            _id = id;
            _rule = rule;
            _expirationDate = expirationDate;
            _subject = subject;
            _storeId = storeId;
        }

        public abstract void Apply(Basket basket);
        public abstract string GetInfo();
        public abstract bool IsValidForBasket(Basket basket);
        public bool IsExpired()
        {
            return _expirationDate < DateTime.Now;
        }


    }
}
