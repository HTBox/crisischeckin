using CrisisCheckinMobile.Annotations;
using CrisisCheckinMobile.ApiClient;
using CrisisCheckinMobile.Models;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CrisisCheckinMobile.ViewModels
{
    public class RegistrationPageViewModel : ViewModelBase
    {

        private readonly ICrisisCheckInApiClient _client;
        private readonly Page _page;
        private string _firstName;
        private string _lastName;
        private string _userName;
        private string _phoneNumber;
        private string _email;
        private string _organizationType;
        private bool _isOrganization;
        private int _selectedOrg;
        private string _password;
        private string _passwordConfirm;
        private List<string> _organizations;
        //  private List<string> _organizations;
        private Orginazation _organization;

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

        public int ContributionLevel
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

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged(); }
        }

        public string OrganizationType
        {
            get { return _organizationType; }
            set { _organizationType = value; OnPropertyChanged(); }
        }

        public Orginazation SelectedOrg
        {
            get { return _organization; }
            set { _organization = value; OnPropertyChanged(); }
        }


        public List<string> OrganizationList
        {
            get { return _organizations; }
            set { _organizations = value; OnPropertyChanged(); }
        }

        public Command AddUserCommand { get; set; }

        public RegistrationPageViewModel(Page page, ICrisisCheckInApiClient client)
        {
            _client = client;
            _page = page;
            ContributionLevel = 0;

            var inti = Init();

        }

        private async Task Init()
        {
            AddUserCommand = new Command(OnAddUser);


            //TODO: Get Organization list
            //var orgs = await _client.GetActiveOrganizations();
            //OrganizationList = orgs;
        }

        private async void OnAddUser()
        {
            var person = new Person
            {
                FirstName = FirstName,
                LastName = LastName,
                OrganizationId = SelectedOrg.Id,
                PhoneNumber = PhoneNumber,
                Email = Email,
                UserName = UserName,
                Password = Password,
                ConfirmPassword = PasswordConfirm

            };

            //TODO: finish implementation for send info to client api
            _client.AddNewUser(person);

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
