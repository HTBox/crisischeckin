using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

using Xamarin.Forms.Platform.Android;

namespace CrisisCheckinMobile.Droid
{
    [Activity(
        Label = "CrisisCheckinMobile",
        MainLauncher = true,
        LaunchMode = LaunchMode.SingleTop,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter( // custom URI format: crisischeckin://CrisisCheckinMobile.Android
        new[] { Intent.ActionMain },
        Categories = new[] { Intent.CategoryLauncher },
        DataScheme = "crisischeckin",
        DataHost = "CrisisCheckinMobile.Android")]
    public class MainActivity : AndroidActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            Xamarin.Forms.Forms.Init(this, bundle);

            SetPage(App.GetMainPage());
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            var disasterId = intent.Data.GetQueryParameter("disaster");
            // TODO - do something with this disaster ID
        }
    }
}

