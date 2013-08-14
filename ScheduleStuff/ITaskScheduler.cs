using System;

namespace ScheduleStuff
{
    public interface ITaskScheduler
    {
        void Every(TimeSpan timespan);
    }
}