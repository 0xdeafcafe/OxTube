using System;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using OxTube.Core.ViewModels;
using System.ComponentModel;

namespace OxTube.UI.WinPhone7.Pages
{
    public partial class Stop
    {
        public StopViewModel PageViewModel;

        public Stop()
        {
            InitializeComponent();

            // Create Page View Model
            PageViewModel = new StopViewModel(App.SelectedStopInfo, this);

            // Set Data Context
            DataContext = PageViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Refresh Times
            BtnRefreshClick(null, null);
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            if (gridMask.Visibility == Visibility.Visible)
                e.Cancel = true;
        }

        public bool IsRefreshing;
        private void BtnRefreshClick(object sender, EventArgs e)
        {
            // Show Pending UI
            try
            {
                // Show UI
                if (btnRefresh != null)
                    btnRefresh.IsEnabled = false;
                gridMask.Visibility = Visibility.Visible;

                // Progress Bar
                SystemTray.ProgressIndicator = new ProgressIndicator
                                                   {
                                                       IsIndeterminate = true,
                                                       IsVisible = true,
                                                       Text = "Refreshing Times..."
                                                   };
            }
            catch
            { }

            // Refresh Code
            PageViewModel.LoadTimeData();
        }
        public void HideRefreshUI()
        {
            Dispatcher.BeginInvoke(delegate
                                       {
                                           // Hide Pending UI
                                           try
                                           {
                                               // Hide UI
                                               if (btnRefresh != null)
                                                   btnRefresh.IsEnabled = true;
                                               gridMask.Visibility = Visibility.Collapsed;

                                               // Progress Bar
                                               SystemTray.ProgressIndicator = new ProgressIndicator {IsVisible = false};
                                           }
                                           catch (Exception)
                                           {

                                           }
                                       });
        }
    }
}