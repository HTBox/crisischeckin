// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using UIKit;
using System;
using System.CodeDom.Compiler;

namespace CrisisCheckinMobile.iOS
{
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton LoginButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField PasswordTextField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton RegisterButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField UsernameTextField { get; set; }

		[Action ("LoginButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void LoginButton_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (LoginButton != null) {
				LoginButton.Dispose ();
				LoginButton = null;
			}
			if (PasswordTextField != null) {
				PasswordTextField.Dispose ();
				PasswordTextField = null;
			}
			if (RegisterButton != null) {
				RegisterButton.Dispose ();
				RegisterButton = null;
			}
			if (UsernameTextField != null) {
				UsernameTextField.Dispose ();
				UsernameTextField = null;
			}
		}
	}
}
