
using System.Collections.Concurrent;

namespace MarketBackend.Domain.Market_Client
{
    public class EventManager
    {
        private int _shopId;
        private ConcurrentDictionary<string, SynchronizedCollection<Member>> _listeners;

        public ConcurrentDictionary<string, SynchronizedCollection<Member>> Listeners { get => _listeners; set => _listeners = value; }

        public EventManager(int shopID){
            _shopId = shopID;
            _listeners = new ConcurrentDictionary<string, SynchronizedCollection<Member>>();
            _listeners.TryAdd("Product Sell Event", new SynchronizedCollection<Member>());
            _listeners.TryAdd("Remove Appointment Event", new SynchronizedCollection<Member>());
            _listeners.TryAdd("Add Appointment Event", new SynchronizedCollection<Member>());
            _listeners.TryAdd("Store Closed Event", new SynchronizedCollection<Member>());
            _listeners.TryAdd("Store Open Event", new SynchronizedCollection<Member>());
            _listeners.TryAdd("Message Event", new SynchronizedCollection<Member>());
            // UploadEventsFromContext();
        }

        public void UploadEventsFromContext()
        {

        }

        public void Subscribe(Member member, Event e)
        {
            if (!_listeners[e.Name].Contains(member))
            {
                _listeners[e.Name].Add(member);
            }
            else throw new Exception("User already sign to this event.");
        }

        public void Unsubscribe(Member member, Event e)
        {
            if (_listeners[e.Name].Contains(member))
            {
                _listeners[e.Name].Remove(member);
            }
            else throw new Exception("User already not sign to this event.");
        }

        public void SubscribeToAll(Member member)
        {
            List<string> events = _listeners.Keys.ToList<string>();
            foreach (string eventName in events)
            {
                _listeners[eventName].Add(member);
            }
        }

        public void UnsubscribeToAll(Member member)
        {
            foreach (string eventName in _listeners.Keys)
            {
                if (_listeners[eventName].Contains(member))
                {
                    _listeners[eventName].Remove(member);
                }
            }
        }

        public void NotifySubscribers(Event e)
        {
            foreach (Member mamber in _listeners[e.Name])
            {
                e.Update(mamber);
            }
        }

    }
}