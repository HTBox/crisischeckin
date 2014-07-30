using System;
using Android.App;
using Android.Runtime;
using CrisisCheckin.Shared;

namespace CrisisCheckin
{
	[Application(Theme="@style/CrisisCheckinTheme")]
	public class App : Application
	{
		public App(IntPtr javaReference, JniHandleOwnership transfer) 
			: base(javaReference, transfer)
		{
		}

		public override void OnCreate ()
		{
			base.OnCreate ();

			Settings = Settings.Load();
		}

		public static Settings Settings { get; private set; }
	}
}

