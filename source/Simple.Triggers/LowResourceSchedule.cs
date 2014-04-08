using System;
using System.Runtime.Caching;

namespace Simple.Triggers
{
    /// <summary>
    ///     Uses cache expiration as internal timer, executing at most
    ///     once every 20 second. Original idea from @_marcan.
    /// </summary>
    public class LowResourceSchedule : ITrigger, ISchedule
    {
        private readonly ObjectCache _cache = MemoryCache.Default;
        private Action _task;
        private TimeSpan _timeSpan;

        public ITrigger Every(TimeSpan timeSpan)
        {
            _timeSpan = timeSpan;
            SetTimer();
            return this;
        }

        public void Action(Action task)
        {
            _task = task;
        }

        private void SetTimer()
        {
            string uuid = Guid.NewGuid().ToString();
            CacheItemPolicy policy = CreatePolicy();
            _cache.Add(uuid, "item", policy);
        }

        private CacheItemPolicy CreatePolicy()
        {
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now.Add(_timeSpan);
            policy.Priority = CacheItemPriority.Default;
            policy.RemovedCallback = WhenExpire;
            return policy;
        }

        private void WhenExpire(CacheEntryRemovedArguments arguments)
        {
            ExecuteTask();
            SetTimer();
        }

        private void ExecuteTask()
        {
            _task();
        }
    }
}