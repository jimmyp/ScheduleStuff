using System;
using NSubstitute;
using NUnit.Framework;

namespace ScheduleStuff.UnitTest
{
    [TestFixture]
    public class SchedulerTests
    {
        public abstract class StubSchedule
        {
            public void AddNextRun()
            {
            }
        }

        private class StubTaskScheduler : TaskScheduler
        {
            private readonly StubSchedule _stubSchedule;

            public StubTaskScheduler(StubSchedule stubSchedule, IVolatileTask task) : base(task)
            {
                _stubSchedule = stubSchedule;
            }

            protected override void ScheduleNextRun()
            {
                _stubSchedule.AddNextRun();
            }
        }

        [Test]
        public void When_TimeToRunTaskOccurs_Task_Is_Run()
        {
            var substituteForTask = Substitute.For<IVolatileTask>();
            var substituteForSchedule = Substitute.For<StubSchedule>();
            var taskbuilder = new StubTaskScheduler(substituteForSchedule, substituteForTask);
            var oneDay = new TimeSpan(1);
            taskbuilder.Every(oneDay);

            taskbuilder.TimeToRunTaskOccured();


            substituteForTask.Received().Run();
        }

        [Test]
        public void Given_Task_Is_Scheduled_As_Repeating_When_Cache_Times_Out_Task_Is_Rescheduled()
        {
            var substituteForTask = Substitute.For<IVolatileTask>();
            var substituteForSchedule = Substitute.For<StubSchedule>();
            var taskbuilder = new StubTaskScheduler(substituteForSchedule, substituteForTask);
            var oneDay = new TimeSpan(1);
            taskbuilder.Every(oneDay);

            taskbuilder.TimeToRunTaskOccured();

            substituteForSchedule.Received().AddNextRun();
        }
    }
}