using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using OxTube.Core.ViewModels;

namespace OxTube.UI.WinPhone8.Pages
{
    public partial class Hub : PhoneApplicationPage
    {
        public HubViewModel PageViewModel = new HubViewModel();

        public Hub()
        {
            InitializeComponent();

            DataContext = PageViewModel;
        }
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            PageViewModel.LoadStopInfo(HubViewModel.Stops.ToOxford);
            PageViewModel.LoadStopInfo(HubViewModel.Stops.ToLondon);
            PageViewModel.LoadFavourites();
        }

        private void lbToLondon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbToLondon.SelectedItem == null) return;

            // Save Stop Info
            App.SelectedStopInfo = lbToLondon.SelectedItem as DataModels.StopInfo;

            // Navigate to Stop Page
            NavigationService.Navigate(new Uri("/Pages/Stop.xaml", UriKind.Relative));
        }
        private void lbToOxford_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbToOxford.SelectedItem == null) return;

            // Save Stop Info
            App.SelectedStopInfo = lbToOxford.SelectedItem as DataModels.StopInfo;

            // Navigate to Stop Page
            NavigationService.Navigate(new Uri("/Pages/Stop.xaml", UriKind.Relative));
        }
        private void lbFavourites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbFavourites.SelectedItem == null) return;

            // Save Stop Info
            App.SelectedStopInfo = lbFavourites.SelectedItem as DataModels.StopInfo;

            // Navigate to Stop Page
            NavigationService.Navigate(new Uri("/Pages/Stop.xaml", UriKind.Relative));
        }


        private void btnFavouriteLondon_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null || (sender as MenuItem == null) || lbToLondon.ItemContainerGenerator == null) return;

            var stopInfo = ((ListBoxItem)lbToLondon.ItemContainerGenerator.ContainerFromItem((sender as MenuItem).DataContext)).Content as DataModels.StopInfo;
            PageViewModel.AddFavourite(stopInfo);
        }
        private void btnFavouriteOxford_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null || (sender as MenuItem == null) || lbToOxford.ItemContainerGenerator == null) return;

            var stopInfo = ((ListBoxItem)lbToOxford.ItemContainerGenerator.ContainerFromItem((sender as MenuItem).DataContext)).Content as DataModels.StopInfo;
            PageViewModel.AddFavourite(stopInfo);
        }
        private void btnUnFavourite_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null || (sender as MenuItem == null) || lbFavourites.ItemContainerGenerator == null) return;

            var stopInfo = ((ListBoxItem)lbFavourites.ItemContainerGenerator.ContainerFromItem((sender as MenuItem).DataContext)).Content as DataModels.StopInfo;
            PageViewModel.RemoveFavourite(stopInfo);
        }
    }
}