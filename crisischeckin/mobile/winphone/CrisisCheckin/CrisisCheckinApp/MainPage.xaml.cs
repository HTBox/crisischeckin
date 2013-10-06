using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CrisisCheckinApp.ServiceClient;
using CrisisCheckinApp.ViewModels;

namespace CrisisCheckinApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        public static Disaster CurrDisaster;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void TextBlock_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

        }

        private void Disaster_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // open new checkin page
            NavigationService.Navigate(new Uri("/CheckinPage.xaml", UriKind.Relative));


        }

        private void LongListSelector_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void OngoingDisasters_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // open new checkin page
            
            


        }

        private void OngoingDisaster_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            CurrDisaster = ((sender as TextBlock).DataContext as DisasterViewModel).Disaster;
            NavigationService.Navigate(new Uri("/CheckinPage.xaml", UriKind.Relative));

        }
    }
}