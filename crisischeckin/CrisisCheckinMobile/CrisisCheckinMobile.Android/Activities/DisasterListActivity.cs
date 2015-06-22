using Android.App;
using Android.OS;
using Android.Widget;
using System.Collections.Generic;

namespace CrisisCheckinMobile.Droid
{
    [Activity(Label = "DisasterListActivity")]            
    public class DisasterListActivity : Activity
    {
        ListView _listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ActionBar.Title = "";
            ActionBar.NavigationMode = ActionBarNavigationMode.Standard;
            ActionBar.SetHomeButtonEnabled(true);

            SetContentView(Resource.Layout.disasterlist);

            _listView = FindViewById<ListView>(Resource.Id.disasterListView);

            // TODO: Get data via the API
            var data = new List<DisasterListDto>();
            data.Add(new DisasterListDto("Terrible Disaster", "Working - until August 12, 2015"));
            data.Add(new DisasterListDto("Disaster Name 2", ""));
            data.Add(new DisasterListDto("Disaster Name 3", "Planned - September 5 - 21, 2015"));

            var adapter = new DisasterListViewAdapter(this, data);
            _listView.Adapter = adapter;
        }
    }
}