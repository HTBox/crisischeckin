using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace CrisisCheckin.IOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to
	// application events from iOS.


	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		// class-level declarations
		UIWindow window;
		CrisisCheckin_IOSViewController controller;
		//
		UINavigationController navcontroller;
		// This method is invoked when the application has loaded and is ready to run. In this
		// method you should instantiate the window, load the UI into it and then make the window
		// visible.
		//
		// You have 17 seconds to return from this method, or iOS will terminate your application.
		//
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			window = new UIWindow (UIScreen.MainScreen.Bounds);
			controller = new CrisisCheckin_IOSViewController ();
			var initialControllers = new List<UIViewController>();
			initialControllers.Add (controller);

			navcontroller = new UINavigationController ();
			navcontroller.ViewControllers = initialControllers.ToArray ();
			window.RootViewController = navcontroller;
			window.MakeKeyAndVisible ();
			UIHelper.Present (new LoginViewController ());
			return true;
		}
		public override void WillEnterForeground (UIApplication application)
		{
			if (Locker.Locked) {
				UIHelper.Present (new LoginViewController ());
			}
		}
		public override void DidEnterBackground (UIApplication application)
		{
			Locker.Locked = true;
		}
	}
}

