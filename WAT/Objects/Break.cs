using System;
using WAT.Interfaces;

namespace WAT.Objects
{
    public class Break : IEvent
    {
        public DateTime Beginning { get; private set; }
        public DateTime End { get; private set; }

        public TimeSpan Duration { get { return End - Beginning; } }


        public Break()
        {
            Start();
        }


        public void Start()
        {
            Beginning = DateTime.Now;
        }

        public void Stop()
        {
            End = DateTime.Now;
        }

    }
}
