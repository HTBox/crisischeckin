using CrisisCheckinMobile.Annotations;
using CrisisCheckinMobile.ApiClient;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CrisisCheckinMobile.ViewModels
{
    public class RegistrationPageViewModel : ViewModelBase
    {

        private readonly ICrisisCheckInApiClient _client;
        private readonly Page _page;


        private string _firstName;
        private string _lastName;
        private string _phoneNumber;
        private string _email;
        private string _organizationType;
        private bool _isOrganization;
        private int _selectedOrg;
        private string _password;
        private string _passwordConfirm;

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; OnPropertyChanged(); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; OnPropertyChanged(); }
        }

        public string PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged(); }
        }

        public string PasswordConfirm
        {
            get { return _passwordConfirm; }
            set { _passwordConfirm = value; OnPropertyChanged(); }
        }

        public int SelectedOrgType
        {
            get { return _selectedOrg; }
            set
            {
                _selectedOrg = value;
                IsOrganization = (_selectedOrg == 1);
                OnPropertyChanged();
            }
        }

        public bool IsOrganization
        {
            get { return _isOrganization; }
            set { _isOrganization = value; OnPropertyChanged(); }
        }

        public string OrganizationType
        {
            get { return _organizationType; }
            set { _organizationType = value; OnPropertyChanged(); }
        }
        public Command AddUserCommand { get; set; }

        public RegistrationPageViewModel(Page page, ICrisisCheckInApiClient client)
        {
            _client = client;
            _page = page;
            SelectedOrgType = 0;


            AddUserCommand = new Command(OnAddUser);
        }

        private async void OnAddUser()
        {

            //Check if fields all have values


            //Do email validation service



            //Show confirm message
            await _page.DisplayAlert("Crisis Registraion", "Confirm registraion thru email", "Ok");

            //Return back to login screen
            _page.Navigation.PushAsync(new LoginPage());
        }
    }


    public abstract class ViewModelBase : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
