using Android.App;
using Android.OS;

namespace CrisisCheckinMobile.Droid
{
    [Activity(Label = "DisasterListActivity")]            
    public class DisasterListActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ActionBar.Title = "";
            ActionBar.NavigationMode = Android.App.ActionBarNavigationMode.Standard;
            ActionBar.SetHomeButtonEnabled(true);


        }
    }
}

