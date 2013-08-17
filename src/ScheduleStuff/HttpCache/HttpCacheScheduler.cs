using ScheduleStuff.FrameworkAbstractions;

namespace ScheduleStuff.HttpCache
{
    public class HttpCacheScheduler : Scheduler
    {
        private readonly IClock _clock;
        private readonly ICache _cache;

        public HttpCacheScheduler(IClock clock, ICache cache)
        {
            _clock = clock;
            _cache = cache;
        }

        public override ITaskScheduler RunVolatileTask<T>()
        {
            return new HttpCacheTaskScheduler(_cache, _clock, new T());
        }
    }
}