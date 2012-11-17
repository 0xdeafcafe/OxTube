using Newtonsoft.Json;
using OxTube.Helpers;
using OxTube.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace OxTube.ItemViewModels
{
    public class StopViewModel : INotifyPropertyChanged
    {
        public StopViewModel(HubViewModel.StopInfo stopHeader, Stop parent)
        {
            StopHeaderInfo = stopHeader;
            _parent = parent;
        }
        public class TimeEntry
        {
            public string ServiceName { get; set; }
            public string Destination { get; set; }
            public int ArrivalTime { get; set; }

            public string ArrivalTimeFriendly { get; set; }
            public SolidColorBrush BackgroundColour { get; set; }
        }

        private Stop _parent;
        private int CONNECTION_TIMEOUT_MILLISECONDS = 10000; // 10 seconds
        private ManualResetEvent _pausingThread = new ManualResetEvent(false);
        private HubViewModel.StopInfo _stopHeaderInfo = new HubViewModel.StopInfo();
        private IList<TimeEntry> _stopTimeInfo = new List<TimeEntry>();

        public HubViewModel.StopInfo StopHeaderInfo
        {
            get { return _stopHeaderInfo; }
            set { _stopHeaderInfo = value; NotifyPropertyChanged("StopHeaderInfo"); }
        }
        public IList<TimeEntry> StopTimeInfo
        {
            get { return _stopTimeInfo; }
            set { _stopTimeInfo = value; NotifyPropertyChanged("StopTimeInfo"); }
        }

        public async Task LoadTimeData()
        {
            // Create Webclient
            WebClient wb = new WebClient();
            wb.DownloadStringCompleted += wb_DownloadStringCompleted;
            wb.DownloadStringAsync(VariousFunctions.CreateAPIUri(_stopHeaderInfo.StopCode.ToString(), _stopHeaderInfo.StopDirection));
        }

        async void wb_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error == null && !e.Cancelled)
            {
                //Hold the JSON returned from the API call
                string jsonData = e.Result;

                // Parse JSON
                StopTimeInfo = JsonConvert.DeserializeObject<IList<TimeEntry>>(jsonData);

                // Make the Output friendly for the DataBinded values
                for (int i = 0; i < StopTimeInfo.Count; i++)
                    StopTimeInfo[i] = await CreateFriendlyTime(StopTimeInfo[i]);

                // Hide Pending UI
                _parent.HideRefreshUI();
            }
            else
            {
                // Tell user there was an error
                MessageBox.Show("Unable to connect to server. Please check your connection and tap the refresh button.", "error", MessageBoxButton.OK);

                // Hide Pending UI
                _parent.HideRefreshUI();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private async Task<TimeEntry> CreateFriendlyTime(TimeEntry timeEntry)
        {
            // Do Friendly Time
            if (timeEntry.ArrivalTime == 0)
                timeEntry.ArrivalTimeFriendly = "due now";
            else if (timeEntry.ArrivalTime == 1)
                timeEntry.ArrivalTimeFriendly = "1 minute";
            else
                timeEntry.ArrivalTimeFriendly = timeEntry.ArrivalTime.ToString() + " minutes";

            // Do colour Schemes
            timeEntry.BackgroundColour = new SolidColorBrush(VariousFunctions.GetARGBFromTime(timeEntry.ArrivalTime));

            return timeEntry;
        }
    }
}
