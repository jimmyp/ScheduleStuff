using System;
using System.Web.Caching;

namespace ScheduleStuff.FrameworkAbstractions
{
    public interface ICache
    {
        void Add(string key,
                 TimeSpan value,
                 CacheDependency dependencies,
                 DateTime absoluteExpiration,
                 TimeSpan slidingExpiration,
                 CacheItemPriority priority,
                 CacheItemRemovedCallback onRemoveCallback);
    }
}