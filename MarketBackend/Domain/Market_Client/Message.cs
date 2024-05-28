
namespace MarketBackend.Domain.Market_Client
{
    public class Message
    {
        private string _comment;
        private bool _seen;

        public Message(string comment)
        {
            _comment = comment;
            _seen = false;
        }

        public Message(string comment,bool seen)
        {
            _comment = comment;
            _seen = seen;
        }

        public string Comment { get => _comment; set => _comment = value; }
        public bool Seen { get => _seen; set => _seen = value; }
    }
}