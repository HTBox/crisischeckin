using System;
using UIKit;

namespace CrisisCheckinMobile.iOS
{
	partial class PendingViewController : UIViewController
	{
		public PendingViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ArrivalDateText.Layer.BorderColor = Constants.HtBoxTan.CGColor;
            ArrivalDateText.BorderStyle = UITextBorderStyle.Line;
            ArrivalDateText.Layer.BorderWidth = 1f;

            WorkUntilDateText.Layer.BorderColor = Constants.HtBoxTan.CGColor;
            WorkUntilDateText.BorderStyle = UITextBorderStyle.Line;
            WorkUntilDateText.Layer.BorderWidth = 1f;
        }
	}
}
