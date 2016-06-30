using System.Threading;

namespace MostRecentUpdatePatternSample
{
    public class Subscription
    {
        private Update _mostRecentUpdate;

        public Update MostRecentUpdate { get { return _mostRecentUpdate; } }

        public Update ReplaceUpdate(Update newUpdate)
        {
            return Interlocked.Exchange(ref _mostRecentUpdate, newUpdate);
        }
    }
}
