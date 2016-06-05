using Android.App;
using Android.Content;
using Android.OS;

namespace CrisisCheckinMobile.Droid.Activities
{
    [Activity(Label = "CommitmentActivity")]            
    public class CommitmentActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            ActionBar.Title = "";
            ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
            ActionBar.SetHomeButtonEnabled(true);
            //ActionBar.SetDisplayHomeAsUpEnabled(true);
//            var htboxRedColor = Resources.GetColor(Resource.Color.htBoxRed); // see values/styles.xml for styling
//            var colorDrawable = new ColorDrawable(htboxRedColor);
//            ActionBar.SetBackgroundDrawable(colorDrawable);

            //var layout = Resources.GetLayout(Resource.Layout.commitment);
            SetContentView(Resource.Layout.commitment);
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);

            // string text = intent.GetStringExtra ("MyData") ?? "Data not available";
        }
    }
}

