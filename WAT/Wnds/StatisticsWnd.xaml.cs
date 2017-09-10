using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using WAT.Modules;
using WAT.Objects;

namespace WAT.Wnds
{
    public partial class StatisticsWnd : Window
    {
        Context context = new Context();

        public StatisticsWnd()
        {
            InitializeComponent();

            DataContext = context;
        }

        private class Context : INotifyPropertyChanged
        {
            public ObservableCollection<WorkSession> Sessions { get; set; } = new ObservableCollection<WorkSession>();


            public Context()
            {
                Refresh();
            }

            public void Refresh()
            {
                Sessions.Clear();

                try
                {
                    if (FileRW.TryRead("sessions", out string jSes))
                    {
                        var ss = jSes.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                        foreach (string st in jSes.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
                        {
                            if(!string.IsNullOrWhiteSpace(st))
                            {
                                Sessions.Add(JsonConvert.DeserializeObject<WorkSession>(st));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ex.Handle();
                }

                OnPropertyChanged();
            }


            public event PropertyChangedEventHandler PropertyChanged = delegate { };
            protected virtual void OnPropertyChanged(string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
