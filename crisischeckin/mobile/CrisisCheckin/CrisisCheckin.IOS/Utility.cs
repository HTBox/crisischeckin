using System;
using MonoTouch.UIKit;

namespace CrisisCheckin
{
	public static class Utility
	{
		public static void ShowError (string title, string message, string buttonText = "OK") 
		{
			var avErr = new UIAlertView ("Sign In Failed", "Invalid Username or Password", null, buttonText);
			avErr.Show ();
		}
	}
}

