using System;

namespace WAT.Interfaces
{
    public interface IEvent
    {
        DateTime Beginning { get; }
        DateTime End { get; }

        TimeSpan Duration { get; }

        void Start();
        void Stop();
    }
}
