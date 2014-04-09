using System;
using System.Diagnostics;
using System.Runtime.Caching;

namespace Simple.Triggers
{
    /// <summary>
    ///     Uses cache expiration as internal timer, executing at most
    ///     once every 20 second. Original idea from @_marcan.
    /// </summary>
    public class LowResSchedule : ITrigger, ISchedule
    {
        private readonly ObjectCache _cache = MemoryCache.Default;
        private Action _task;
        private TimeSpan _timeSpan;

        public LowResSchedule() : this(MemoryCache.Default) {}

        public LowResSchedule(ObjectCache internalCache)
        {
            _cache = internalCache;
        }

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
            if (_task == null)
            {
                return;
            }

            try
            {
                _task();                
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.ToString());
            }
        }
    }
}