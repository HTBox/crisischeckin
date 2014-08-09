using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;
using CrisisCheckin.Shared;

namespace CrisisCheckin
{
	public class SignInViewController : DialogViewController
	{
		RegisterViewController registerViewController;
		CommitmentsViewController commitmentsViewController;
		IWebService webService;

		EntryElement username;
		EntryElement password;

		public SignInViewController () : 
			base (UITableViewStyle.Grouped, new RootElement ("Crisis Checkin"), false)
		{
			webService = WebServiceFactory.Create ();

			username = new EntryElement ("Email", "your@email.com", "") {
				KeyboardType = UIKeyboardType.EmailAddress,
				AutocorrectionType = UITextAutocorrectionType.No
			};
			password = new EntryElement ("Password", "password", "", true) { 
				AutocorrectionType = UITextAutocorrectionType.No
			};


			Root = new RootElement ("Crisis Checkin") {
				new Section ("Already have an account?") {
					username,
					password,
					new StyledStringElement ("Sign In", async () => {

						username.ResignFirstResponder (true);
						password.ResignFirstResponder (true);

						//TODO: Show progress HUD
						ProgressHud.Show ("Signing In");

						// You have to fetch values first from MonoTouch.Dialog elements
						username.FetchValue ();
						password.FetchValue ();

						// Actually sign in
							var r = await webService.SignInAsync(new SignInRequest { 
							Username = username.Value,
							Password = password.Value
						});

						if (!r.Succeeded) {
							// Show failure message
							Utility.ShowError ("Sign In Failed", "Invalid Username or Password");
							return;
						}

						// Store our credentials for future web service calls
						AppDelegate.Settings.SignedInUsername = username.Value;
						AppDelegate.Settings.SignedInPassword = password.Value;

						//TODO: Hide progress hud
						ProgressHud.Dismiss ();

						// Navigate to commitments after successfuly login
						commitmentsViewController = new CommitmentsViewController ();
						NavigationController.PushViewController (commitmentsViewController, true);

					}) {
						Alignment = UITextAlignment.Center
					}
				},
				new Section ("Don't have an account yet?") {
					new StyledStringElement ("Create an Account", () => {

						// Navigate to registration controller
						registerViewController = new RegisterViewController();
						NavigationController.PushViewController (registerViewController, true);


					}) {
						Alignment = UITextAlignment.Center
					}
				}
			};
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			// Reset when we come back to this
			username.Value = string.Empty;
			password.Value = string.Empty;

			username.BecomeFirstResponder (false);
		}
	}
}

