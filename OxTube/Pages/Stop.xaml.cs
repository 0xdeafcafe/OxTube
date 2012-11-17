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
    public partial class Stop : PhoneApplicationPage
    {
        public StopViewModel PageViewModel = new StopViewModel(App.SelectedStopInfo);

        public Stop()
        {
            InitializeComponent();

            this.DataContext = PageViewModel;
        }

        async protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            PageViewModel.LoadTimeData();
        }
    }
}