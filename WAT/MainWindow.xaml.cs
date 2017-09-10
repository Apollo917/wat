using System.Windows;
using System.Windows.Input;
using WAT.Objects;
using System.ComponentModel;
using WAT.Wnds;

namespace WAT
{
    public partial class MainWindow : Window
    {
        Context context = new Context();

        public MainWindow()
        {
            InitializeComponent();

            this.Top = Properties.Settings.Default.top;
            this.Left = Properties.Settings.Default.left;

            DataContext = context;
        }


        private void OnStartPauseClick(object sender, MouseButtonEventArgs e)
        {
            context.StartPause();
        }

        private void OnStopClick(object sender, MouseButtonEventArgs e)
        {
            context.Stop();
        }

        private void OnStatisticsClick(object sender, MouseButtonEventArgs e)
        {
            var stats = new StatisticsWnd()
            {
                Owner = this
            };

            stats.Show();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            context.Stop();

            Properties.Settings.Default.top = this.Top;
            Properties.Settings.Default.left = this.Left;
            Properties.Settings.Default.Save();
        }
        

        private class Context : INotifyPropertyChanged
        {
            public string StButCont { get; private set; } = "START";

            public WorkSession Session { get; set; } = new WorkSession();

            public void StartPause()
            {
                switch (Session.Status)
                {
                    case EventStatus.JustCreated:
                        Session.Start();
                        StButCont = "PAUSE";
                        break;
                    case EventStatus.Active:
                        Session.Pause();
                        StButCont = "RESUME";
                        break;
                    case EventStatus.Suspended:
                        Session.Resume();
                        StButCont = "PAUSE";
                        break;
                    case EventStatus.Stopped:
                        Session = new WorkSession();
                        Session.Start();
                        StButCont = "PAUSE";
                        break;
                }

                OnPropertyChanged();
            }

            public void Stop()
            {
                Session.Stop();
                StButCont = "START";

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
