// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System;
using System.CodeDom.Compiler;

namespace CrisisCheckinMobile.iOS
{
	[Register ("CommitmentViewController")]
	partial class CommitmentViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel IAmLabel { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (IAmLabel != null) {
				IAmLabel.Dispose ();
				IAmLabel = null;
			}
		}
	}
}
