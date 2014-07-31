using System;
using System.Drawing;
using MonoTouch.UIKit;

namespace CrisisCheckin
{
	public static class ProgressHud
	{
		static UIView hud;
		static UILabel label;
		static UIActivityIndicatorView activity;

		const float HUD_SIZE = 64f;
		const float LABEL_HEIGHT = 28f;

		static void BuildHud ()
		{
			if (hud == null) {
				hud = new UIView (UIApplication.SharedApplication.KeyWindow.RootViewController.View.Frame) {
					BackgroundColor = UIColor.Black,
					Alpha = 0.8f,
					AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight
				};

				activity = new UIActivityIndicatorView (UIActivityIndicatorViewStyle.WhiteLarge) {
					Frame = new RectangleF (hud.Frame.Width / 2 - HUD_SIZE / 2, hud.Frame.Height / 2 - HUD_SIZE / 2 - LABEL_HEIGHT / 2 - 5f, HUD_SIZE, HUD_SIZE),
					AutoresizingMask = UIViewAutoresizing.FlexibleMargins
				};

				label = new UILabel {
					Frame = new RectangleF (0f, activity.Frame.Bottom + 10f, hud.Frame.Width, LABEL_HEIGHT),
					TextAlignment = UITextAlignment.Center,
					Font = UIFont.BoldSystemFontOfSize (18.0f),
					AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleBottomMargin,
					TextColor = UIColor.White

				};

				hud.AddSubview (activity);
				hud.AddSubview (label);

				//label = new UILabel (new RectangleF (0,  hud.Frame.Width,
			}


		}

		static object lockObj = new object ();

		public static void Show (string message)
		{
			UIApplication.SharedApplication.InvokeOnMainThread (() => {
				BuildHud ();

				if (hud.Superview != null)
					Dismiss ();

				lock (lockObj) {
				
					label.Text = message;

					var windows = UIApplication.SharedApplication.Windows;
					Array.Reverse(windows);
					foreach (UIWindow window in windows)
					{
						if (window.WindowLevel == UIWindow.LevelNormal && !window.Hidden)
						{
							window.AddSubview(hud);
							break;
						}
					}
					activity.StartAnimating ();
				}
			});
		}

		public static void Dismiss ()
		{
			UIApplication.SharedApplication.InvokeOnMainThread (() => {
				lock (lockObj) {
					if (activity.IsAnimating) {
						activity.StopAnimating ();
					}

					if (hud.Superview != null)
						hud.RemoveFromSuperview ();
				}
			});
		}
	}
}

