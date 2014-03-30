using System;
using System.Runtime.Caching;

namespace Simple.Triggers
{
    /// <summary>
    /// Uses cache expiration as internal timer. 
    /// Original idea from @_marcan.
    /// </summary>
    public class CacheTimer : ITrigger, ISchedule
    {
        private readonly ObjectCache _cache;

        private Action _task;
        private TimeSpan _timeSpan;

        public CacheTimer()
            : this(MemoryCache.Default) { }

        public CacheTimer(ObjectCache internalCache)
        {
            _cache = internalCache;
        }

        public void Action(Action task)
        {
            _task = task;
        }

        public ITrigger Every(TimeSpan timeSpan)
        {
            _timeSpan = timeSpan;
            SetTimer();
            return this;
        }

        private void SetTimer()
        {
            var uuid = Guid.NewGuid().ToString();
            _cache.Add(uuid, 0, CreatePolicy());
        }

        private CacheItemPolicy CreatePolicy()
        {
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now.Add(_timeSpan);
            policy.Priority = CacheItemPriority.NotRemovable;
            policy.RemovedCallback += args => WhenExpire();
            return policy;
        }

        private void WhenExpire()
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