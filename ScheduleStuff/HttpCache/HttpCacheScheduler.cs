using ScheduleStuff.FrameworkAbstractions;

namespace ScheduleStuff.HttpCache
{
    public class HttpCacheScheduler : IScheduler
    {
        private readonly IClock _clock;
        private readonly ICache _cache;

        public HttpCacheScheduler(IClock clock, ICache cache)
        {
            _clock = clock;
            _cache = cache;
        }

        public HttpCacheTaskBuilder RunVolatileTask<T>() where T : VolatileTask, new()
        {
            return new HttpCacheTaskBuilder(_cache, _clock, new T());
        }
    }
}