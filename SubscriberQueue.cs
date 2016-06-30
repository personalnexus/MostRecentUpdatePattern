using System.Collections.Concurrent;

namespace MostRecentUpdatePatternSample
{
    public class SubscriberQueue
    {
        private ConcurrentQueue<Subscription> _queue = new ConcurrentQueue<Subscription>();

        /// <summary>
        /// Either replaces a previous update for the subscription with the given one or
        /// queues the subscription (when it doesn't have a current update)
        /// </summary>
        public void Enqueue(Subscription subscription, Update update)
        {
            if (subscription.ReplaceUpdate(update) == null)
            {
                _queue.Enqueue(subscription);
            }
            //
            // If the previous value of subscription.Update was something other than
            // null, the subscription was still queued and we just replaced the update
            // with a newer one
            //
        }

        /// <summary>
        /// 
        /// </summary>
        public bool TryDequeue(out Update update)
        {
            Subscription subscription;
            update = null;
            //
            // The update on a subscripion may be null (if it has been voided)
            // in which case we move on to the next subscription
            //
            while (update == null)
            {
                if (!_queue.TryDequeue(out subscription))
                {
                    //
                    // Stop when there are no more subscriptions
                    //
                    return false;
                }
                update = subscription.ReplaceUpdate(update);
            }
            return true;
        }
    }
}
