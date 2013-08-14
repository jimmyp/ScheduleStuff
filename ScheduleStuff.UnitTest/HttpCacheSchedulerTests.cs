using System;
using System.Web.Caching;
using NSubstitute;
using NUnit.Framework;
using ScheduleStuff.FrameworkAbstractions;
using ScheduleStuff.HttpCache;

namespace ScheduleStuff.UnitTest
{
    [TestFixture]
    public class HttpCacheSchedulerTests
    {
        private ICache _substituteICache;
        private IClock _substituteForIClock;
        private DateTime _now;

        public class SomeTask : VolatileTask
        {
        }

        [SetUp]
        public void SetUp()
        {
            _substituteICache = Substitute.For<ICache>();
            _substituteForIClock = Substitute.For<IClock>();
            _now = DateTime.Now;
            _substituteForIClock.GetNow().Returns(_now);
            
        }

        [Test]
        public void RunVolatileTaskEvery_X_Adds_Item_To_Cache_For_Correct_Time()
        {
            var threeHours = new TimeSpan(0, 3, 0, 0);
            var scheduler = new HttpCacheScheduler(_substituteForIClock, _substituteICache);

            scheduler.RunVolatileTask<SomeTask>().Every(threeHours);
            
            _substituteICache.Received().Add(Arg.Any<string>(),
                                Arg.Is<TimeSpan>(x => x == threeHours),
                                Arg.Is<CacheDependency>(x => x == null), 
                                Arg.Is<DateTime>(x => x == _now.Add(threeHours)),
                                Arg.Is<TimeSpan>(x => x == Cache.NoSlidingExpiration), 
                                Arg.Is<CacheItemPriority>(x => x == CacheItemPriority.NotRemovable),
                                Arg.Any<CacheItemRemovedCallback>());
        }

        [Test]
        public void When_Cache_Times_Out_Task_Is_Run()
        {
            var substituteForTask = Substitute.For<VolatileTask>();
            var taskbuilder = new HttpCacheTaskScheduler(_substituteICache, _substituteForIClock, substituteForTask);


            taskbuilder.TimeToRunTaskOccured();


            substituteForTask.Received().Run();
        }

        [Test]
        public void When_Cache_Times_Out_Task_Is_Rescheduled()
        {
            var substituteForTask = Substitute.For<VolatileTask>();
            var taskbuilder = new HttpCacheTaskScheduler(_substituteICache, _substituteForIClock, substituteForTask);
            var oneDay = new TimeSpan(1);
            taskbuilder.Every(oneDay);
            
            taskbuilder.TimeToRunTaskOccured();


            _substituteICache.Received(2).Add(Arg.Any<string>(),
                                Arg.Is<TimeSpan>(x => x == oneDay),
                                Arg.Is<CacheDependency>(x => x == null),
                                Arg.Is<DateTime>(x => x == _now.Add(oneDay)),
                                Arg.Is<TimeSpan>(x => x == Cache.NoSlidingExpiration),
                                Arg.Is<CacheItemPriority>(x => x == CacheItemPriority.NotRemovable),
                                Arg.Any<CacheItemRemovedCallback>());
        }
    }

}
