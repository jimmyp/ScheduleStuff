using System;
using System.Web.Caching;
using ScheduleStuff.FrameworkAbstractions;

namespace ScheduleStuff.HttpCache
{
    public class HttpCacheTaskBuilder
    {
        private readonly ICache _cache;
        private readonly IClock _clock;
        private readonly VolatileTask _task;

        public HttpCacheTaskBuilder(ICache cache, IClock clock, VolatileTask task)
        {
            _cache = cache;
            _clock = clock;
            _task = task;
        }

        public void Every(TimeSpan threeHours)
        {
            var guid = new Guid();
            _cache.Add(guid.ToString(),
                       threeHours,
                       null,
                       _clock.GetNow().Add(threeHours),
                       Cache.NoSlidingExpiration,
                       CacheItemPriority.NotRemovable,
                       CacheItemRemoved);
        }

        private void CacheItemRemoved(string key, object value, CacheItemRemovedReason reason)
        {
            //_task.Run();
            Every((TimeSpan)value);
        }
    }
}