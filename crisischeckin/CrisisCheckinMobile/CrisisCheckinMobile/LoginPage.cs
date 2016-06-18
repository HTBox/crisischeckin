using CrisisCheckinMobile.Views;
using System;
using Xamarin.Forms;

namespace CrisisCheckinMobile
{
    public class LoginPage : ContentPage
    {
        private readonly Button _loginButton;
        private readonly Button _registerButton;

        private async void OnLoginButtonClicked (object sender, EventArgs e)
        {
            //...
            var isValid = true; //AreCredentialsCorrect (user);
            if (isValid)
            {
                //App.IsUserLoggedIn = true;
                Navigation.InsertPageBefore(new ProfileView(), this);
                await Navigation.PopAsync();
            }
            else
            {
                // Login failed
            }
        }
         
        public LoginPage()
        {
            _loginButton = new Button
            {
                Text = "Login",
                TextColor = Color.White
            };
            _loginButton.Clicked += OnLoginButtonClicked;

            _registerButton = new Button
            {
                Text = "Register",
                TextColor = Color.White
            };
            // TODO: register button click

            BackgroundColor = Constants.HtBoxDarkBrown;
            Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.Center,
                WidthRequest = 280.0,
                Children =
                {
                    new Entry
                    {
                        Placeholder = "Username",
                        HorizontalTextAlignment = TextAlignment.Start,
                        WidthRequest = 280.0
                    },
                    new Entry
                    {
                        Placeholder = "Password",
                        HorizontalTextAlignment = TextAlignment.Start,
                        IsPassword = true,
                        WidthRequest = 280.0
                    },
                    _loginButton,
                    _registerButton
                }
            };
        }

        ~LoginPage()
        {
            if (_loginButton != null)
            {
                _loginButton.Clicked -= OnLoginButtonClicked;
            }
            if (_registerButton != null)
            {
                // TODO: remove handler here
            }
        }
    }
}