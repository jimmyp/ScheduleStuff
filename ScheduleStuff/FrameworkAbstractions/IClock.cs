using System;

namespace ScheduleStuff.FrameworkAbstractions
{
    public interface IClock
    {
        DateTime GetNow();
    }
}