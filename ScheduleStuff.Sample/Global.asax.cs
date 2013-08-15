using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ScheduleStuff.FrameworkAbstractions;
using ScheduleStuff.HttpCache;

namespace ScheduleStuff.Sample
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            var scheduler = new HttpCacheScheduler(new Clock(), new CacheAdapter());
            scheduler.RunVolatileTask<IncrementCount>().Every(new TimeSpan(0, 0, 2, 0));
        }
    }

    public class IncrementCount : IVolatileTask
    {
        public void Run()
        {
            Counter.Value++;
        }
    }

    public class Counter
    {
        public static int Value { get; set; }
    }

    public class CacheAdapter : ICache
    {
        public void Add(string key, TimeSpan value, CacheDependency dependencies, DateTime absoluteExpiration,
                        TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            HttpRuntime.Cache.Add(key, value, dependencies, absoluteExpiration, slidingExpiration, priority,
                                  onRemoveCallback);
        }
    }

    public class Clock : IClock
    {
        public DateTime GetNow()
        {
            return DateTime.Now;
        }
    }
}