using System;

namespace Simple.Triggers
{
    public interface ISchedule
    {
        ITrigger Every(TimeSpan timeSpan);
    }
}