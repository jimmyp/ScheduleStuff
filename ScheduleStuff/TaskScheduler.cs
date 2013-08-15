using System;

namespace ScheduleStuff
{
    public abstract class TaskScheduler : ITaskScheduler
    {
        protected VolatileTask Task;
        private bool _repeatTask;
        protected TimeSpan TimeSpan;

        protected TaskScheduler(VolatileTask task)
        {
            Task = task;
        }

        public void Every(TimeSpan timespan)
        {
            TimeSpan = timespan;
            _repeatTask = true;

            ScheduleNextRun();
        }

        protected abstract void ScheduleNextRun();

        public void TimeToRunTaskOccured()
        {
            Task.Run();
            if (_repeatTask) ScheduleNextRun();
        }
    }
}