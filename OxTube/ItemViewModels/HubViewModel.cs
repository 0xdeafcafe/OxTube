using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using OxTube.Helpers;
using Newtonsoft.Json;

namespace OxTube.ItemViewModels
{
    public class HubViewModel : INotifyPropertyChanged
    {
        public class StopInfo
        {
            public string StopName { get; set; }
            public uint StopCode { get; set; }
            public string StopURL { get; set; }
        }

        private ObservableCollection<StopInfo> _toOxford = new ObservableCollection<StopInfo>();
        private ObservableCollection<StopInfo> _toLondon = new ObservableCollection<StopInfo>();

        public ObservableCollection<StopInfo> ToOxford
        {
            get { return _toOxford; }
        }
        public ObservableCollection<StopInfo> ToLondon
        {
            get { return _toLondon; }
        }
        public enum Stops { ToOxford, ToLondon }
        public async Task LoadStopInfo(Stops stop)
        {
            string stopJSONdata = string.Empty;

            switch (stop)
            {
                case Stops.ToLondon:
                    _toLondon.Clear();
                    stopJSONdata = VariousFunctions.GetStringFromResources("Resources/StopData/TowardsLondon.json");
                    _toLondon = JsonConvert.DeserializeObject<ObservableCollection<StopInfo>>(stopJSONdata);
                    NotifyPropertChanged("ToLondon");
                    break;

                case Stops.ToOxford:
                    _toOxford.Clear();
                    stopJSONdata = VariousFunctions.GetStringFromResources("Resources/StopData/TowardsOxford.json");
                    _toOxford = JsonConvert.DeserializeObject<ObservableCollection<StopInfo>>(stopJSONdata);
                    NotifyPropertChanged("ToOxford");
                    break;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}