using System;

namespace Simple.Triggers
{
    public interface ITrigger
    {
        ISchedule Action(Action task);
    }
}