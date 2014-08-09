using System;
using MonoTouch.Dialog;
using MonoTouch.UIKit;

namespace CrisisCheckin
{
	public class RegisterViewController : DialogViewController
	{
		public RegisterViewController () : 
			base (UITableViewStyle.Grouped, new RootElement ("Register"), true)
		{
			Root = new RootElement ("Register") {
				new Section {
					new EntryElement ("First Name", "", "") {
						AutocorrectionType = UITextAutocorrectionType.No
					},
					new EntryElement ("Last Name", "", "") {
						AutocorrectionType = UITextAutocorrectionType.No
					},
					new EntryElement ("Phone", "123-123-1234", "") {
						KeyboardType = UIKeyboardType.PhonePad
					},
					new EntryElement ("Email", "your@email.com", "") {
						KeyboardType = UIKeyboardType.EmailAddress,
						AutocorrectionType = UITextAutocorrectionType.No
					},
					new EntryElement ("Username", "username", "") {
						AutocorrectionType = UITextAutocorrectionType.No
					},
					new EntryElement ("Password", "password", "", true) {
						AutocorrectionType = UITextAutocorrectionType.No
					},
					new EntryElement ("Confirm", "password", "", true) {
						AutocorrectionType = UITextAutocorrectionType.No
					},
					new StyledStringElement ("Register", () => {

						ProgressHud.Show ("Registering");

						//TODO: Actually register

						ProgressHud.Dismiss ();

					}) {
						Alignment = UITextAlignment.Center
					}
				}
			};
		}
	}
}

