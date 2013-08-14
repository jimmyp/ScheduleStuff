using System;

namespace ScheduleStuff
{
    interface ITaskBuilder
    {
        void Every(TimeSpan threeHours);
    }
}