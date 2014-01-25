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
	[Register ("CrisisCheckin_IOSViewController")]
	partial class CrisisCheckin_IOSViewController
	{
		[Outlet]
		MonoTouch.UIKit.UIButton HistoryButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton LogoffButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton UpcomingButton { get; set; }

		[Outlet]
		MonoTouch.UIKit.UIButton VolunteerButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (VolunteerButton != null) {
				VolunteerButton.Dispose ();
				VolunteerButton = null;
			}

			if (UpcomingButton != null) {
				UpcomingButton.Dispose ();
				UpcomingButton = null;
			}

			if (HistoryButton != null) {
				HistoryButton.Dispose ();
				HistoryButton = null;
			}

			if (LogoffButton != null) {
				LogoffButton.Dispose ();
				LogoffButton = null;
			}
		}
	}
}
