using System;

namespace ScheduleStuff
{
    public abstract class TaskScheduler : ITaskScheduler
    {
        protected VolatileTask _task;
        private bool _repeatTask;
        protected TimeSpan _timeSpan;

        public void Every(TimeSpan timespan)
        {
            _timeSpan = timespan;
            _repeatTask = true;

            ScheduleNextRun();
        }

        protected abstract void ScheduleNextRun();

        public void TimeToRunTaskOccured()
        {
            _task.Run();
            if (_repeatTask) ScheduleNextRun();
        }
    }
}