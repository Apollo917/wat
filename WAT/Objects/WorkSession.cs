using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Threading;
using WAT.Interfaces;
using WAT.Modules;

namespace WAT.Objects
{
    public class WorkSession : IEvent, INotifyPropertyChanged
    {
        #region Fields & properties

        [JsonProperty]
        public DateTime Beginning { get; private set; }
        [JsonProperty]
        public DateTime End { get; private set; }

        [JsonProperty]
        List<Break> breaks = new List<Break>();

        [JsonProperty]
        int activeTime = 0;
        [JsonProperty]
        int breaktime = 0;

        [JsonIgnore]
        string sessionsPath = "sessions";

        [JsonIgnore]
        DispatcherTimer updateTimer = new DispatcherTimer();

        [JsonIgnore]
        public EventStatus Status { get; private set; } = EventStatus.JustCreated;

        [JsonIgnore]
        public TimeSpan Duration { get { return End - Beginning; } }
        [JsonIgnore]
        public TimeSpan SessionTime
        {
            get
            {
                return TimeSpan.FromSeconds(activeTime + breaktime);
            }
        }
        [JsonIgnore]
        public TimeSpan BreakTime
        {
            get
            {
                return TimeSpan.FromSeconds(breaktime);
            }
        }
        [JsonIgnore]
        public TimeSpan WorkTime
        {
            get
            {
                return TimeSpan.FromSeconds(activeTime);
            }
        }
        [JsonIgnore]
        public Double ActivityToAll
        {
            get
            {
                    return (activeTime != 0 && breaktime != 0) ? (activeTime * 100 / (activeTime + breaktime)) : 100;
            }
        }
        [JsonIgnore]
        public int BreaksCount
        {
            get
            {
                return breaks.Count;
            }
        }
        #endregion


        public WorkSession()
        {
            updateTimer.Interval = TimeSpan.FromSeconds(1);
            updateTimer.Tick += UpdateTimerTick;
        }

        

        public void Start()
        {
            if(Status == EventStatus.JustCreated)
            {
                Beginning = DateTime.Now;
                Status = EventStatus.Active;

                updateTimer.Start();
            }
        }
        public void Stop()
        {
            if(Status == EventStatus.Active || Status == EventStatus.Suspended)
            {
                updateTimer.Stop();

                if (Status == EventStatus.Suspended)
                {
                    breaks[breaks.Count - 1].Stop();
                }

                End = DateTime.Now;
                Status = EventStatus.Stopped;

                FileRW.Write(sessionsPath, JsonConvert.SerializeObject(this));
            }
        }


        public void Pause()
        {
            if(Status == EventStatus.Active)
            {
                breaks.Add(new Break());
                Status = EventStatus.Suspended;
            }
        }
        public void Resume()
        {
            if(Status == EventStatus.Suspended)
            {
                breaks[breaks.Count - 1].Stop();
                Status = EventStatus.Active;
            }
        }



        private void UpdateTimerTick(object sender, EventArgs e)
        {
            OnPropertyChanged();

            switch (Status)
            {
                case EventStatus.Active:
                    activeTime++;
                    break;
                case EventStatus.Suspended:
                    breaktime++;
                    break;
            }
        }


        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
