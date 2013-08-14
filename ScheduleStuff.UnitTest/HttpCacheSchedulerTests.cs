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
        public class SomeTask : VolatileTask
        {
        }

        [Test]
        public void RunVolatileTaskEvery_X_Adds_Item_To_Cache_For_Correct_Time()
        {
            var substituteCache = Substitute.For<ICache>();

            var substituteForIClock = Substitute.For<IClock>();
            var now = DateTime.Now;
            substituteForIClock.GetNow().Returns(now);

            var threeHours = new TimeSpan(0, 3, 0, 0);

            var scheduler = new HttpCacheScheduler(substituteForIClock, substituteCache);


            scheduler.RunVolatileTask<SomeTask>().Every(threeHours);


            substituteCache.Received().Add(Arg.Any<string>(),
                                Arg.Is<TimeSpan>(x => x == threeHours),
                                Arg.Is<CacheDependency>(x => x == null), 
                                Arg.Is<DateTime>(x => x == now.Add(threeHours)),
                                Arg.Is<TimeSpan>(x => x == Cache.NoSlidingExpiration), 
                                Arg.Is<CacheItemPriority>(x => x == CacheItemPriority.NotRemovable),
                                Arg.Any<CacheItemRemovedCallback>());
        }
    }

}
