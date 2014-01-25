using System;
using MonoTouch.Foundation;
using MonoTouch.Security;
using OpenTK;
using System.Drawing;
using MonoTouch.UIKit;
namespace CrisisCheckin.IOS
{
	public static class UIHelper
	{

		public static void Present(UIViewController controller)
		{
			UIApplication.SharedApplication.Windows [0].InvokeOnMainThread (delegate {
					UINavigationController navcontroller = (UINavigationController)UIApplication.SharedApplication.Windows[0].RootViewController;
				navcontroller.PresentViewController(controller,true,null);
				});
		}

	}
}

