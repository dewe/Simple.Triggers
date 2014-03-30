using System;
using System.Timers;

namespace Simple.Triggers
{
    public class Schedule : ITrigger, ISchedule, IDisposable
    {
        private Action _task;
        private TimeSpan _timeSpan;
        private readonly Timer _timer = new Timer();

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
            _timer.Interval = _timeSpan.TotalMilliseconds;
            _timer.Elapsed += (s, a) => ExecuteTask();
            _timer.Start();
        }

        private void ExecuteTask()
        {
            _task();
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}