using CrisisCheckinMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CrisisCheckinMobile.Views
{
    public partial class ProfileView : ContentPage
    {
        public ProfileView()
        {
            InitializeComponent();
            BindingContext = new ProfileViewModel(this.Navigation);

        }

        protected override async void OnAppearing()
        {
            if (!App.IsUserLoggedIn)
            {
                await Navigation.PushModalAsync(new LoginPage(), true);
            }
        }
    }
}
