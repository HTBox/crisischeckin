
using System;
using UIKit;

// To keep the app from scaling on the iPhone 6
// http://conceptdev.blogspot.com/2014/09/iphone-6-and-6-plus-launch-images-for.html
// http://conceptdev.blogspot.com/2014/09/iphone-6-and-6-plus-launchscreenstorybo.html

namespace CrisisCheckinMobile.iOS
{
    public partial class LoginViewController : UIViewController
    {
        public LoginViewController(IntPtr handle)
            : base(handle)
        {
        }

        public event EventHandler OnLoginSuccess;

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
            
            // Release any cached data, images, etc that aren't in use.
        }

        partial void LoginButton_TouchUpInside(UIButton sender)
        {
            // Validate our Username & Password somehow.
            //if(IsUserNameValid() && IsPasswordValid())
            //{
                //We have successfully authenticated a the user,
                //Now fire our OnLoginSuccess Event.
                if(OnLoginSuccess != null)
                {
                    OnLoginSuccess(sender, new EventArgs());
                }
//            }
//            else
//            {
//                new UIAlertView("Login Error", "Bad user name or password", null, "OK", null).Show();
//            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            LoginButton.BackgroundColor = UIColor.Clear;
        }
    }
}