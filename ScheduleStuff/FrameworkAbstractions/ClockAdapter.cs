using System;

namespace ScheduleStuff.FrameworkAbstractions
{
    public class ClockAdapter : IClock
    {
        public DateTime GetNow()
        {
            return DateTime.Now;
        }
    }
}