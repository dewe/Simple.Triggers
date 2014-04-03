using System;
using System.Collections.Generic;
using System.Timers;

namespace Simple.Triggers
{
    public class MultiSchedule : IDisposable
    {
        private readonly List<Timer> _timers = new List<Timer>(); 

        public void Every(TimeSpan timeSpan, Action action)
        {
            var timer = new Timer();
            timer.Interval = timeSpan.TotalMilliseconds;
            timer.Elapsed += (source, eventargs) => action();
            timer.Start();
            _timers.Add(timer);
        }

        public void Dispose()
        {
            foreach (var timer in _timers)
            {
                timer.Dispose();
            }
        }
    }
}