using System;

namespace TUI.Finder.Web.Infrastructure
{
    public interface ITimer
    {
        event Action Elapsed;
        void Start(int minutes);
    }
}