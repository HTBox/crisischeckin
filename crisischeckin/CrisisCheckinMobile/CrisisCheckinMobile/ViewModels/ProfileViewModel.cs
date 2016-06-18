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
                    await navigation.PushAsync(new TemporaryView());
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
                    await navigation.PushAsync(new TemporaryView());
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
                    this.IsCheckedInVisible = true;
                    this.IsCheckedOutVisisble = false;
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
                    this.IsCheckedInVisible = false;
                    this.IsCheckedOutVisisble = true;
                });
            }
        }

        private bool isCheckedIn = true;
        public bool IsCheckedInVisible
        {
            get
            {
                return isCheckedIn;
            }
            set
            {
                if (isCheckedIn != value)
                {
                    isCheckedIn = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCheckedInVisible)));
                }
            }
        }

        private bool isCheckedout;
        public bool IsCheckedOutVisisble
        {
            get
            {
                return isCheckedout;
            }
            set
            {
                if (isCheckedout != value)
                {
                    isCheckedout = value;

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsCheckedOutVisisble)));
                }
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        
    }
}
