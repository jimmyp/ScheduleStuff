using ScheduleStuff.FrameworkAbstractions;
using ScheduleStuff.HttpCache;

namespace ScheduleStuff
{
    public abstract class Scheduler
    {
        public abstract ITaskScheduler RunVolatileTask<T>() where T : IVolatileTask, new();

        public static Scheduler Current = new HttpCacheScheduler(new ClockAdapter(), new CacheAdapter());
    }
}