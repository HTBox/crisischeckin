

using CrisisCheckinMobile.ApiClient;
using CrisisCheckinMobile.ViewModels;
using Xamarin.Forms;

namespace CrisisCheckinMobile.Views
{
    public partial class RegistrationPage : ContentPage
    {
        private RegistrationPageViewModel viewModel;

        public RegistrationPage()
        {



            InitializeComponent();
            var client = new CrisisCheckInApiClient();
            viewModel = new RegistrationPageViewModel(this, client);
            BindingContext = viewModel;




        }
    }
}
