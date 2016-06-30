using CrisisCheckinMobile.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace CrisisCheckinMobile.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private INavigation navigation;

        public ProfileViewModel(INavigation navigation)
        {
            this.navigation = navigation;
        }


        public ICommand GoToMyContacts
        {
            get
            {
                return new Command(async () =>
                {
                    //TODO Change this to Contacts Page when ready.
                    await navigation.PushAsync(new TemporaryView());
                });
            }
        }

        public ICommand GoToMyRequests
        {
            get
            {
                return new Command(async () =>
                {
                    //TODO 
                    await navigation.PushAsync(new RequestListPage());
                });
            }
        }

        public ICommand GoToMyInfo
        {
            get
            {
                return new Command(async () =>
                {
                    //TODO 
                    await navigation.PushAsync(new TemporaryView());
                });
            }
        }

        public ICommand GoToViewDisasterData
        {
            get
            {
                return new Command(async () =>
                {
                    //TODO 
                    await navigation.PushAsync(new TemporaryView());
                });
            }
        }

        public ICommand GoToViewDisasterResources
        {
            get
            {
                return new Command(async () =>
                {
                    //TODO 
                    await navigation.PushAsync(new ResourceListPage());
                });
            }
        }

        public ICommand AddResources
        {
            get
            {
                return new Command(async () =>
                {
                    //TODO 
                    await navigation.PushAsync(new TemporaryView());
                });
            }
        }

        public ICommand AddContacts
        {
            get
            {
                return new Command(async () =>
                {
                    //TODO 
                    await navigation.PushAsync(new TemporaryView());
                });
            }
        }

        public ICommand ReportData
        {
            get
            {
                return new Command(async () =>
                {
                    //TODO
                    await navigation.PushAsync(new TemporaryView());
                });
            }
        }

        public ICommand CheckOutOfDisaster
        {
            get
            {
                return new Command(() =>
                {
                    this.IsCheckedIn = false;
                });
            }
        }

        public ICommand CheckInToDisaster
        {
            get
            {
                return new Command(() =>
                {
                    // TODO call to check in page
                    //await navigation.PushAsync(new TemporaryView());
                    this.IsCheckedIn = true;
                });
            }
        }

        private bool isCheckedIn = false;
        private bool IsCheckedIn
        {
            get
            {
                return isCheckedIn;
            }
            set
            {
                isCheckedIn = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCheckedInVisible)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCheckedOutVisisble)));
            }
        }

        
        public bool IsCheckedInVisible
        {
            get
            {
                return !isCheckedIn;
            }
        }

        public bool IsCheckedOutVisisble
        {
            get
            {
                return isCheckedIn;
            }
         
        }

        public ICommand Logout
        {
            get
            {
                return new Command(async () =>
                {
                    App.IsUserLoggedIn = false;
                    await navigation.PushModalAsync(new LoginPage(), true);
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        
    }
}
