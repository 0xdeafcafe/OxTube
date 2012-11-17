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
using System.Threading.Tasks;
using System.ComponentModel;

namespace OxTube.Pages
{
    public partial class Stop : PhoneApplicationPage
    {
        public StopViewModel PageViewModel;

        public Stop()
        {
            InitializeComponent();

            // Create Page View Model
            PageViewModel = new StopViewModel(App.SelectedStopInfo, this);

            // Set Data Context
            this.DataContext = PageViewModel;
        }

        async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Refresh Times
            btnRefresh_Click(null, null);
        }

        async protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            if (gridMask.Visibility == System.Windows.Visibility.Visible)
                e.Cancel = true;
        }

        public bool IsRefreshing;
        async private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Show Pending UI
            try
            {
                // Show UI
                if (btnRefresh != null)
                    btnRefresh.IsEnabled = false;
                gridMask.Visibility = System.Windows.Visibility.Visible;

                // Progress Bar
                SystemTray.ProgressIndicator = new ProgressIndicator();
                SystemTray.ProgressIndicator.IsIndeterminate = true;
                SystemTray.ProgressIndicator.IsVisible = true;
                SystemTray.ProgressIndicator.Text = "Refreshing Times...";
            }
            catch { }

            // Refresh Code
            if (sender != null && e != null)
                await PageViewModel.LoadTimeData();
            else
                PageViewModel.LoadTimeData();
        }
        public void HideRefreshUI()
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                // Hide Pending UI
                try
                {
                    // Hide UI
                    if (btnRefresh != null)
                        btnRefresh.IsEnabled = true;
                    gridMask.Visibility = System.Windows.Visibility.Collapsed;

                    // Progress Bar
                    SystemTray.ProgressIndicator = new ProgressIndicator();
                    SystemTray.ProgressIndicator.IsVisible = false;
                }
                catch (Exception ex)
                {

                }
            }));
        }
    }
}