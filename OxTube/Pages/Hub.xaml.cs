using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using OxTube.ItemViewModels;

namespace OxTube.Pages
{
    public partial class Hub : PhoneApplicationPage
    {
        public ItemViewModels.HubViewModel PageViewModel = new ItemViewModels.HubViewModel();

        public Hub()
        {
            InitializeComponent();

            this.DataContext = PageViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            PageViewModel.LoadStopInfo(ItemViewModels.HubViewModel.Stops.ToLondon);
            PageViewModel.LoadStopInfo(ItemViewModels.HubViewModel.Stops.ToOxford);
        }

        private void lbToLondon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Save Stop Info
            App.SelectedStopInfo = lbToLondon.SelectedItem as HubViewModel.StopInfo;

            // Navigate to Stop Page
            NavigationService.Navigate(new Uri("/Pages/Stop.xaml", UriKind.Relative));
        }

        private void lbToOxford_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Save Stop Info
            App.SelectedStopInfo = lbToOxford.SelectedItem as HubViewModel.StopInfo;

            // Navigate to Stop Page
            NavigationService.Navigate(new Uri("/Pages/Stop.xaml", UriKind.Relative));
        }
    }
}