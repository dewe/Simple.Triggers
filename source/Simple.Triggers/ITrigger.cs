using System;

namespace Simple.Triggers
{
    public interface ITrigger
    {
        void Action(Action task);
    }
}