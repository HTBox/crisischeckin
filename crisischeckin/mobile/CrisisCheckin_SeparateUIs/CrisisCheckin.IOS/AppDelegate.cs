using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using CrisisCheckin.Shared;

namespace CrisisCheckin
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		UIWindow window;
		UINavigationController navigationController;
		SignInViewController signInViewController;

		public static Settings Settings { get; private set; }

		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			Settings = Settings.Load();

			signInViewController = new SignInViewController ();
			navigationController = new UINavigationController (signInViewController);

			window = new UIWindow (UIScreen.MainScreen.Bounds);
			window.RootViewController = navigationController;
			window.MakeKeyAndVisible ();

			return true;
		}
	}
}

