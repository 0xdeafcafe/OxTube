using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using OxTube.Core.Helpers;
using System.Net;
#if WINDOWS_PHONE
using System.Windows.Media;
using System.Windows;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
#endif
#if WP7
using OxTube.UI.WinPhone7.Pages;
#endif
#if WP8
using OxTube.UI.WinPhone8.Pages;
#endif

namespace OxTube.Core.ViewModels
{
#if WINDOWS_PHONE
    public class StopViewModel : INotifyPropertyChanged
    {
#if WP7
        private readonly Stop _parent;

        public StopViewModel(DataModels.StopInfo stopHeader, Stop parent)
        {
            StopHeaderInfo = stopHeader;
            _parent = parent;
        }
#elif WP8
        private readonly Stop _parent;

        public StopViewModel(DataModels.StopInfo stopHeader, Stop parent)
        {
        StopHeaderInfo = stopHeader;
            _parent = parent;
        }
#endif

        private DataModels.StopInfo _stopHeaderInfo = new DataModels.StopInfo();
        private ObservableCollection<DataModels.TimeEntry> _stopTimeInfo = new ObservableCollection<DataModels.TimeEntry>();

        public DataModels.StopInfo StopHeaderInfo
        {
            get { return _stopHeaderInfo; }
            set { _stopHeaderInfo = value; NotifyPropertyChanged("StopHeaderInfo"); }
        }
        public ObservableCollection<DataModels.TimeEntry> StopTimeInfo
        {
            get { return _stopTimeInfo; }
            set { _stopTimeInfo = value; NotifyPropertyChanged("StopTimeInfo"); }
        }

        public void LoadTimeData()
        {
            // Create Webclient
            var wb = new WebClient();
            wb.DownloadStringCompleted += (o, args) =>
                                              {
                                                if (args.Error == null && !args.Cancelled)
                                                {
                                                    //Hold the JSON returned from the API call
                                                    var jsonData = args.Result;

                                                    try
                                                    {
                                                        // Parse JSON
                                                        StopTimeInfo = JsonConvert.DeserializeObject<ObservableCollection<DataModels.TimeEntry>>(jsonData);

                                                        // Make the Output friendly for the DataBinded values
                                                        for (var i = 0; i < StopTimeInfo.Count; i++)
                                                            StopTimeInfo[i] = CreateFriendlyTime(StopTimeInfo[i]);

    #if WP7
                                                        // Hide Pending UI
                                                        _parent.HideRefreshUI();
    #elif WP8
                                                        // Hide Pending UI
                                                        _parent.HideRefreshUI();
    #endif
                                                    }
                                                    catch
                                                    {
                                                        // Tell user there was an error
                                                        MessageBox.Show("The connection to the server was unsuccessful.", "Error", MessageBoxButton.OK);

    #if WP7
                                                        // Hide Pending UI
                                                        _parent.HideRefreshUI();
    #elif WP8
                                                        // Hide Pending UI
                                                        _parent.HideRefreshUI();
    #endif
                                                    }
                                                }
                                                else
                                                {
                                                    // Tell user there was an error
                                                    MessageBox.Show("Unable to connect to server. Please check your connection and tap the refresh button.", "Error", MessageBoxButton.OK);

    #if WP7
                                                    // Hide Pending UI
                                                    _parent.HideRefreshUI();
    #elif WP8
                                                    // Hide Pending UI
                                                    _parent.HideRefreshUI();
    #endif
                                                }
                                              };

            wb.DownloadStringAsync(VariousFunctions.CreateAPIUri(_stopHeaderInfo.StopCode.ToString(), _stopHeaderInfo.StopDirection));
        }

        private DataModels.TimeEntry CreateFriendlyTime(DataModels.TimeEntry timeEntry)
        {
            // Do Friendly Time
            if (timeEntry.ArrivalTime == 0)
                timeEntry.ArrivalTimeFriendly = "due now";
            else if (timeEntry.ArrivalTime == 1)
                timeEntry.ArrivalTimeFriendly = "1 minute";
            else
                timeEntry.ArrivalTimeFriendly = timeEntry.ArrivalTime + " minutes";

            // Do colour Schemes
            timeEntry.BackgroundColour = new SolidColorBrush(VariousFunctions.GetArgbFromTime(timeEntry.ArrivalTime));

            return timeEntry;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
#endif
}
