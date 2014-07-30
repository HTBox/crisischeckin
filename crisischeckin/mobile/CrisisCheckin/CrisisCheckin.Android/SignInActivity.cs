
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CrisisCheckin.Shared;

namespace CrisisCheckin.Android
{
	[Activity (Label = "Crisis Checkin")]			
	public class SignInActivity : Activity
	{
		EditText editTextUsername;
		EditText editTextPassword;
		Button buttonSignIn;
		Button buttonRegister;

		IWebService webService;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.SignIn);

			webService = WebServiceFactory.Create ();

			editTextUsername = FindViewById<EditText> (Resource.Id.editTextUsername);
			editTextPassword = FindViewById<EditText> (Resource.Id.editTextPassword);
			buttonSignIn = FindViewById<Button> (Resource.Id.buttonSignIn);
			buttonRegister = FindViewById<Button> (Resource.Id.buttonRegister);

			buttonSignIn.Click += async delegate {
			
				var r = await webService.SignInAsync(new SignInRequest { 
					Username = editTextUsername.Text,
					Password = editTextPassword.Text
				});

				if (!r.Succeeded) {
					Toast.MakeText(this, "SignIn Failed: " + r.Exception, ToastLength.Short).Show();
					return;
				}

				App.Settings.SignedInUsername = editTextUsername.Text;
				StartActivity(typeof(CommitmentsActivity));
			};

			buttonRegister.Click += delegate {
				StartActivity(typeof(RegisterActivity));
			};
		}
	}
}

