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
	[Register ("PendingViewController")]
	partial class PendingViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField ArrivalDateText { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel AsLongAsIAmNeededLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel IWillArriveOnLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel IWillWorkUntilLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField WorkUntilDateText { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (ArrivalDateText != null) {
				ArrivalDateText.Dispose ();
				ArrivalDateText = null;
			}
			if (AsLongAsIAmNeededLabel != null) {
				AsLongAsIAmNeededLabel.Dispose ();
				AsLongAsIAmNeededLabel = null;
			}
			if (IWillArriveOnLabel != null) {
				IWillArriveOnLabel.Dispose ();
				IWillArriveOnLabel = null;
			}
			if (IWillWorkUntilLabel != null) {
				IWillWorkUntilLabel.Dispose ();
				IWillWorkUntilLabel = null;
			}
			if (WorkUntilDateText != null) {
				WorkUntilDateText.Dispose ();
				WorkUntilDateText = null;
			}
		}
	}
}
