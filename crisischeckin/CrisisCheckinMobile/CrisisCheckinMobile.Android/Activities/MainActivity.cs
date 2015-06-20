using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Graphics.Drawables;

namespace CrisisCheckinMobile.Droid
{
    [Activity(
        Label = "", // "CrisisCheckinMobile",
        MainLauncher = true,
        LaunchMode = LaunchMode.SingleTop,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter( // custom URI format: crisischeckin://CrisisCheckinMobile.Android
        new[] { Intent.ActionMain },
        Categories = new[] { Intent.CategoryLauncher },
        DataScheme = "crisischeckin",
        DataHost = "CrisisCheckinMobile.Android")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

//            ActionBar.Title = "";
//            ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
//            ActionBar.SetHomeButtonEnabled(true);
//            //ActionBar.SetDisplayHomeAsUpEnabled(true);
//            var htboxRedColor = Resources.GetColor(Resource.Color.htBoxRed);
//            var colorDrawable = new ColorDrawable(htboxRedColor);
//            ActionBar.SetBackgroundDrawable(colorDrawable);

            if (false) // TODO: if already authenticated
            {
                var commitmentActivity = new Intent(this, typeof(CommitmentActivity));
                //commitmentActivity.PutExtra("disasterId", disasterId);
                StartActivity(commitmentActivity);
            }
            else // TODO: send to log on page
            {
                var disasterListActivity = new Intent(this, typeof(DisasterListActivity));
                StartActivity(disasterListActivity);
            }
  
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            var disasterId = intent.Data.GetQueryParameter("disaster");

            // TODO: do something with this disaster ID
        }
    }
}

