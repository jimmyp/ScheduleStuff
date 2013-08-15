using System;

namespace ScheduleStuff
{
    public abstract class TaskScheduler : ITaskScheduler
    {
        protected IVolatileTask Task;
        private bool _repeatTask;
        protected TimeSpan TimeSpan;

        protected TaskScheduler(IVolatileTask task)
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