using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CrisisCheckin.IOS
{
	public partial class LoginViewController : UIViewController
	{
		static bool UserInterfaceIdiomIsPhone {
			get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
		}
		UIView dismiss;

		public LoginViewController ()
			: base (UserInterfaceIdiomIsPhone ? "LoginViewController_iPhone" : "LoginViewController_iPad", null)
		{

		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();
			
			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			this.LoginButton.TouchUpInside += delegate {
				// Temp until we have a way to login.

				Locker.Locked = false;
				this.DismissViewController(true,null);

			};

			// Perform any additional setup after loading the view, typically from a nib.
		}
		public override UIView InputAccessoryView 
		{
			get 
			{
				if (dismiss == null)
				{
					dismiss = new UIView(new RectangleF(0,0,320,27));
					dismiss.BackgroundColor = UIColor.FromPatternImage(new UIImage("Images/accessoryBG.png"));          
					UIButton dismissBtn = new UIButton(new RectangleF(View.Bounds.Width - 70,2, 60, 30));
					dismissBtn.SetBackgroundImage(new UIImage("keyboard@2x.png"), UIControlState.Normal);        
					dismissBtn.TouchDown += delegate {
						this.View.EndEditing(true);
					};
					dismiss.AddSubview(dismissBtn);
				}
				return dismiss;
			}
		}
	}
}

