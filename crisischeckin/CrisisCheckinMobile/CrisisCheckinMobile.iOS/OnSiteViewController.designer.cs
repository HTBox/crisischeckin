// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace CrisisCheckinMobile.iOS
{
	[Register ("OnSiteViewController")]
	partial class OnSiteViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel AsLongAsIAmNeededLabel2 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel IWillWorkUntilLabel2 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel NotWorkingLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel WorkingLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISwitch WorkingSwitch { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (AsLongAsIAmNeededLabel2 != null) {
				AsLongAsIAmNeededLabel2.Dispose ();
				AsLongAsIAmNeededLabel2 = null;
			}
			if (IWillWorkUntilLabel2 != null) {
				IWillWorkUntilLabel2.Dispose ();
				IWillWorkUntilLabel2 = null;
			}
			if (NotWorkingLabel != null) {
				NotWorkingLabel.Dispose ();
				NotWorkingLabel = null;
			}
			if (WorkingLabel != null) {
				WorkingLabel.Dispose ();
				WorkingLabel = null;
			}
			if (WorkingSwitch != null) {
				WorkingSwitch.Dispose ();
				WorkingSwitch = null;
			}
		}
	}
}
