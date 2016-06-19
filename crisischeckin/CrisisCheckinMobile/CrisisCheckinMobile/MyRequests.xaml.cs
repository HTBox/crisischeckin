using CrisisCheckinMobile.ApiClient;
using CrisisCheckinMobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CrisisCheckinMobile
{
    public partial class MyRequests : ContentPage/*, INotifyPropertyChanged*/
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        //private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}


        string _foo = "foo";
        public string foo
        {
            get { return _foo; }
            set
            {
                if (_foo != value)
                {
                    _foo = value;
                    OnPropertyChanged("foo");
                }
            }
        }


        ObservableCollection<RequestDto> _listRequests = new ObservableCollection<RequestDto>();
        public ObservableCollection<RequestDto> listRequests
        {
            get { return _listRequests; }
            set
            {
                if (_listRequests != value)
                {
                    _listRequests = value;
                    OnPropertyChanged("listRequests");
                }
            }
        }

        public MyRequests()
        {
            InitializeComponent();
            BindingContext = this;
            var task = Init();
        }
        private async Task Init()
        {
            ICrisisCheckInApiClient apiClient = new CrisisCheckInApiClient();
            var dtos = await apiClient.GetRequests(4);  //TODO: wire up to Auth0 so we don't have to pass person ID
            listRequests = new ObservableCollection<RequestDto>(dtos);
            listViewRequests.ItemsSource = listRequests;
            Debug.WriteLine(dtos.ToString());
        }
    }
}

