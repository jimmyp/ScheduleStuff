using System;
using System.Web.Caching;
using ScheduleStuff.FrameworkAbstractions;

namespace ScheduleStuff.HttpCache
{
    public class HttpCacheTaskScheduler : TaskScheduler
    {
        private readonly ICache _cache;
        private readonly IClock _clock;

        public HttpCacheTaskScheduler(ICache cache, IClock clock, IVolatileTask task) : base(task)
        {
            _cache = cache;
            _clock = clock;
        }

        protected override void ScheduleNextRun()
        {
            _cache.Add(new Guid().ToString(),
                       TimeSpan,
                       null,
                       _clock.GetNow().Add(TimeSpan),
                       Cache.NoSlidingExpiration,
                       CacheItemPriority.NotRemovable,
                       CacheItemRemoved);
        }

        private void CacheItemRemoved(string key, object value, CacheItemRemovedReason reason)
        {
            TimeToRunTaskOccured();
        }
    }
}