using System;
using System.Web;
using System.Web.Caching;

namespace ScheduleStuff.FrameworkAbstractions
{
    public class CacheAdapter : ICache
    {
        public void Add(string key, TimeSpan value, CacheDependency dependencies, DateTime absoluteExpiration,
                        TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            HttpRuntime.Cache.Add(key, value, dependencies, absoluteExpiration, slidingExpiration, priority,
                                  onRemoveCallback);
        }
    }
}