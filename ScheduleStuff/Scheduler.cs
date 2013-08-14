namespace ScheduleStuff
{
    public abstract class Scheduler
    {
        public abstract ITaskScheduler RunVolatileTask<T>() where T : VolatileTask, new();
    }
}