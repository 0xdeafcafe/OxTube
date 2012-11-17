using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OxTube.ItemViewModels
{
    public class StopViewModel : INotifyPropertyChanged
    {
        public StopViewModel(HubViewModel.StopInfo stopHeader)
        {
            StopHeaderInfo = stopHeader;
        }

        public class TimeInfo
        {
            public string Destination { get; set; }
            public string ArrivalTime { get; set; }
        }

        private HubViewModel.StopInfo _stopHeaderInfo = new HubViewModel.StopInfo();
        private IList<TimeInfo> _stopTimeInfo = new List<TimeInfo>();

        public HubViewModel.StopInfo StopHeaderInfo
        {
            get { return _stopHeaderInfo; }
            set { _stopHeaderInfo = value; NotifyPropertyChanged("StopHeaderInfo"); }
        }
        public IList<TimeInfo> StopTimeInfo
        {
            get { return _stopTimeInfo; }
            set { _stopTimeInfo = value; NotifyPropertyChanged("StopTimeInfo"); }
        }

        public async Task LoadTimeData()
        {

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
}
