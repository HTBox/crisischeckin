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
	[Register ("CommitmentViewController")]
	partial class CommitmentViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView CommitmentContainerView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField DisasterStatusText { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel IAmLabel { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton View1Button { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton View2Button { get; set; }

		[Action ("View1Button_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void View1Button_TouchUpInside (UIButton sender);

		[Action ("View2Button_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void View2Button_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (CommitmentContainerView != null) {
				CommitmentContainerView.Dispose ();
				CommitmentContainerView = null;
			}
			if (DisasterStatusText != null) {
				DisasterStatusText.Dispose ();
				DisasterStatusText = null;
			}
			if (IAmLabel != null) {
				IAmLabel.Dispose ();
				IAmLabel = null;
			}
			if (View1Button != null) {
				View1Button.Dispose ();
				View1Button = null;
			}
			if (View2Button != null) {
				View2Button.Dispose ();
				View2Button = null;
			}
		}
	}
}
