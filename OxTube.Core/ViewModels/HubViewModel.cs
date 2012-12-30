using System;
using System.ComponentModel;
using Newtonsoft.Json;
using OxTube.Core.Helpers;
#if WINDOWS_PHONE
using System.IO.IsolatedStorage;
using System.Collections.ObjectModel;
#endif

namespace OxTube.Core.ViewModels
{
#if WINDOWS_PHONE
    public class HubViewModel : INotifyPropertyChanged
    {
        private readonly IsolatedStorageSettings _isolatedApplicationSettings = IsolatedStorageSettings.ApplicationSettings;
        private ObservableCollection<DataModels.StopInfo> _toOxford = new ObservableCollection<DataModels.StopInfo>();
        private ObservableCollection<DataModels.StopInfo> _toLondon = new ObservableCollection<DataModels.StopInfo>();
        private ObservableCollection<DataModels.StopInfo> _favourites = new ObservableCollection<DataModels.StopInfo>();

        public ObservableCollection<DataModels.StopInfo> ToOxford
        {
            get { return _toOxford; }
            set { _toOxford = value; NotifyPropertChanged("ToOxford"); }
        }
        public ObservableCollection<DataModels.StopInfo> ToLondon
        {
            get { return _toLondon; }
            set { _toLondon = value; NotifyPropertChanged("ToLondon"); }
        }
        public ObservableCollection<DataModels.StopInfo> Favourites
        {
            get { return _favourites; }
            set { _favourites = value; NotifyPropertChanged("Favourites"); }
        }

        public enum Stops { ToOxford, ToLondon }
        public void LoadStopInfo(Stops stop)
        {
            string stopJsonData;

            switch (stop)
            {
                case Stops.ToLondon:
                    ToLondon.Clear();
#if WP7
                    stopJsonData = VariousFunctions.GetStringFromCoreResources("Resources/StopData/TowardsLondon.json");
#elif WP8
                    stopJsonData = VariousFunctions.GetStringFromResources("Resources/StopData/TowardsLondon.json");
#else
                    stopJsonData = "[]";
#endif
                    
                    ToLondon = JsonConvert.DeserializeObject<ObservableCollection<DataModels.StopInfo>>(stopJsonData);
                    foreach (var t in ToLondon)
                    {
                        t.StopDirection = "tl";
                        t.StopDirectionFrendly = "Towards: London";
                    }
                    
                    break;

                case Stops.ToOxford:
                    ToOxford.Clear();

#if WP7
                    stopJsonData = VariousFunctions.GetStringFromCoreResources("Resources/StopData/TowardsOxford.json");
#elif WP8
                    stopJsonData = VariousFunctions.GetStringFromResources("Resources/StopData/TowardsOxford.json");
#else
                    stopJsonData = "[]";
#endif

                    ToOxford = JsonConvert.DeserializeObject<ObservableCollection<DataModels.StopInfo>>(stopJsonData);
                    foreach (var t in ToOxford)
                    {
                        t.StopDirection = "to";
                        t.StopDirectionFrendly = "Towards: Oxford";
                    }
                    break;
            }
        }

        public void AddFavourite(DataModels.StopInfo stopInfo)
        {
            var contains = (_favourites.Contains(stopInfo));

            if (!contains)
                Favourites.Add(stopInfo);

            SaveFavourites();
        }
        public void RemoveFavourite(DataModels.StopInfo stopInfo)
        {
            Favourites.Remove(stopInfo);

            SaveFavourites();
        }
        public void LoadFavourites()
        {
            // Get Favourites from IsolatedStorage
            if (_isolatedApplicationSettings.Contains("favourites"))
                Favourites = (ObservableCollection<DataModels.StopInfo>)_isolatedApplicationSettings["favourites"];
            else
                Favourites = new ObservableCollection<DataModels.StopInfo>();
        }
        private void SaveFavourites()
        {
            if (_isolatedApplicationSettings.Contains("favourites"))
                _isolatedApplicationSettings["favourites"] = Favourites;
            else
                _isolatedApplicationSettings.Add("favourites", Favourites);

            // I always forget this shit, SAVE THE FUCKING SHIT MAN.
            _isolatedApplicationSettings.Save();
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
#endif
}