// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoTouch.Foundation;
using System.CodeDom.Compiler;

namespace CrisisCheckin.IOS
{
	[Register ("LoginViewController")]
	partial class LoginViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton LoginButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField UserNameText { get; set; }

		[Outlet]
		MonoTouch.UIKit.UITextField UserPasswordText { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (UserNameText != null) {
				UserNameText.Dispose ();
				UserNameText = null;
			}

			if (UserPasswordText != null) {
				UserPasswordText.Dispose ();
				UserPasswordText = null;
			}

			if (LoginButton != null) {
				LoginButton.Dispose ();
				LoginButton = null;
			}
		}
	}
}
