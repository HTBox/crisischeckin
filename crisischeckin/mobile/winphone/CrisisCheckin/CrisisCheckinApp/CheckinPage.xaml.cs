using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace CrisisCheckinApp
{
    public partial class CheckinPage : PhoneApplicationPage
    {
        public CheckinPage()
        {
            InitializeComponent();
            
        }

        private void CheckinPage_Loaded(object sender, RoutedEventArgs e)
        {
            this.disasterName.Text = MainPage.CurrDisaster.Name;

        }

        private void checkinOk_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));

        }
    }
}