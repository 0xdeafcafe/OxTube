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
            public string StopDirection { get; set; }

            public string StopDirectionFrendly { get; set; }
        }

        private ObservableCollection<StopInfo> _toOxford = new ObservableCollection<StopInfo>();
        private ObservableCollection<StopInfo> _toLondon = new ObservableCollection<StopInfo>();

        public ObservableCollection<StopInfo> ToOxford
        {
            get { return _toOxford; }
            set { _toOxford = value; NotifyPropertChanged("ToOxford"); }
        }
        public ObservableCollection<StopInfo> ToLondon
        {
            get { return _toLondon; }
            set { _toLondon = value; NotifyPropertChanged("ToLondon"); }
        }
        public enum Stops { ToOxford, ToLondon }
        public async Task LoadStopInfo(Stops stop)
        {
            string stopJSONdata = string.Empty;

            switch (stop)
            {
                case Stops.ToLondon:
                    ToLondon.Clear();
                    stopJSONdata = VariousFunctions.GetStringFromResources("Resources/StopData/TowardsLondon.json");
                    ToLondon = JsonConvert.DeserializeObject<ObservableCollection<StopInfo>>(stopJSONdata);
                    for (int i = 0; i < ToLondon.Count; i++)
                    {
                        ToLondon[i].StopDirection = "tl";
                        ToLondon[i].StopDirectionFrendly = "Towards: London";
                    }
                    
                    break;

                case Stops.ToOxford:
                    ToOxford.Clear();
                    stopJSONdata = VariousFunctions.GetStringFromResources("Resources/StopData/TowardsOxford.json");
                    ToOxford = JsonConvert.DeserializeObject<ObservableCollection<StopInfo>>(stopJSONdata);
                    for (int i = 0; i < ToOxford.Count; i++)
                    {
                        ToOxford[i].StopDirection = "to";
                        ToOxford[i].StopDirectionFrendly = "Towards: Oxford";
                    }
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