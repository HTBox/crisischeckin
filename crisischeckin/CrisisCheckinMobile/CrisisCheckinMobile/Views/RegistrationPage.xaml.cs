

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
            viewModel = new RegistrationPageViewModel(this.Navigation);
            BindingContext = viewModel;


        }
    }
}
