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
					new EntryElement ("First Name", "", ""),
					new EntryElement ("Last Name", "", ""),
					new EntryElement ("Phone", "123-123-1234", "") {
						KeyboardType = UIKeyboardType.PhonePad
					},
					new EntryElement ("Email", "your@email.com", "") {

					},
					new EntryElement ("Username", "username", "") {

					},
					new EntryElement ("Password", "password", "") {

					},
					new EntryElement ("Confirm", "password", "") {

					},
				}
			};
		}
	}
}

