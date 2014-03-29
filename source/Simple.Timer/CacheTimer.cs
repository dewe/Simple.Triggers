using System;
using System.Runtime.Caching;
using TUI.Finder.Web.Infrastructure;

namespace Simple.Timer
{
    /// <summary>
    /// Uses cache expiration as timer trigger. 
    /// Original idea from @_marcan.
    /// </summary>
    public class CacheDrivenTimer : ITimer
    {
        private readonly ObjectCache _cache;
        private readonly string _timerName;
        private int _minutes;

        public CacheDrivenTimer()
            : this(MemoryCache.Default) { }

        public CacheDrivenTimer(ObjectCache internalCache)
        {
            _timerName = Guid.NewGuid().ToString();
            _cache = internalCache;
        }

        public event Action Elapsed;

        public void Start(int minutes)
        {
            _minutes = minutes;
            SetTimer();
        }

        private void SetTimer()
        {
            _cache.Add(_timerName, 0, CreatePolicy());
        }

        private CacheItemPolicy CreatePolicy()
        {
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now.AddMinutes(_minutes);
            policy.Priority = CacheItemPriority.Default;
            policy.RemovedCallback += args => WhenElapsed();
            return policy;
        }

        private void WhenElapsed()
        {
            Elapsed();
            SetTimer();
        }
    }
}