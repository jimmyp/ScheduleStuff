using System;
using System.Web.Caching;
using ScheduleStuff.FrameworkAbstractions;

namespace ScheduleStuff.HttpCache
{
    public class HttpCacheTaskScheduler : TaskScheduler
    {
        private readonly ICache _cache;
        private readonly IClock _clock;

        public HttpCacheTaskScheduler(ICache cache, IClock clock, VolatileTask task)
        {
            _cache = cache;
            _clock = clock;
            _task = task;
        }

        protected override void ScheduleNextRun()
        {
            _cache.Add(new Guid().ToString(),
                       _timeSpan,
                       null,
                       _clock.GetNow().Add(_timeSpan),
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