using System;
using System.Collections.Generic;

namespace MostRecentUpdatePatternSample
{
    public class DataObject
    {
        private Dictionary<SubscriberQueue, Subscription> _subscriptions = 
            new Dictionary<SubscriberQueue, Subscription>();

        private double _value;
        public double Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                //
                // Post the same update object (because its data readonly) with the new value to 
                // each subcriber
                //
                PostUpdateToSubscribers(new Update(value));
            }
        }

        public void VoidValue()
        {
            PostUpdateToSubscribers(null);
        }

        private void PostUpdateToSubscribers(Update update)
        {
            foreach (KeyValuePair<SubscriberQueue, Subscription> kvp in _subscriptions)
            {
                //
                // Enqueue the subscription (kvp.Value) with the update in the subscriber queue (kvp.Key)
                //
                kvp.Key.Enqueue(kvp.Value, update);
            }
        }

        /// <summary>
        /// Adds a subscriber to this object (call on producer thread only)
        /// </summary>
        public void Subscribe(SubscriberQueue subscriber)
        {
            Subscription subscription;
            if (_subscriptions.TryGetValue(subscriber, out subscription))
            {
                throw new ArgumentException("Cannot add same subscriber twice", "subscriber");
            }
            subscription = new Subscription();
            _subscriptions.Add(subscriber, subscription);
        }

        /// <summary>
        /// Adds a subscriber from this object (call on producer thread only)
        /// </summary>
        public void Unsubscribe(SubscriberQueue subscriber)
        {
            _subscriptions.Remove(subscriber);
        }
    }
}
