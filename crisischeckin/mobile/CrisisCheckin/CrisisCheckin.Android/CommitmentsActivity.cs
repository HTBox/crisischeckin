
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using CrisisCheckin.Shared;
using System.Threading.Tasks;

namespace CrisisCheckin.Android
{
	[Activity (Label = "Commitments")]			
	public class CommitmentsActivity : Activity
	{
		IWebService webService;
		Button buttonVolunteer;
		ListView listActive;
		ListView listInactive;
		CommitmentsAdapter activeAdapter;
		CommitmentsAdapter inactiveAdapter;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Commitments);

			webService = WebServiceFactory.Create ();

			buttonVolunteer = FindViewById<Button> (Resource.Id.buttonVolunteer);
			listActive = FindViewById<ListView> (Resource.Id.listActive);
			listInactive = FindViewById<ListView> (Resource.Id.listInactive);

			activeAdapter = new CommitmentsAdapter (this);
			inactiveAdapter = new CommitmentsAdapter (this);

			listActive.Adapter = activeAdapter;
			listInactive.Adapter = inactiveAdapter;

			listActive.ItemClick += (sender, e) => {
				var c = activeAdapter[e.Position];
				var detailsIntent = new Intent(this, typeof(CommitmentDetailsActivity));
				detailsIntent.PutExtra("COMMITMENT", JsonSerializer.Serialize(c));
				StartActivity(detailsIntent);
			};
			listInactive.ItemClick += (sender, e) => {
				var c = inactiveAdapter[e.Position];
				var detailsIntent = new Intent(this, typeof(CommitmentDetailsActivity));
				detailsIntent.PutExtra("COMMITMENT", JsonSerializer.Serialize(c));
				StartActivity(detailsIntent);
			};
			buttonVolunteer.Click += delegate {
				StartActivity (typeof(DisastersActivity));
			};

			Refresh ();
		}


		async Task Refresh()
		{
			buttonVolunteer.Visibility = ViewStates.Gone;

			var r = await webService.GetCommitmentsAsync (new CommitmentsRequest {
				Username = App.Settings.SignedInUsername,
				Password = App.Settings.SignedInPassword
			});

			if (!r.Succeeded) {
				Toast.MakeText (this, "Failed to Fetch Commitments for " + App.Settings.SignedInUsername, ToastLength.Short).Show ();
				return;
			}

			if (r.Result.Any (c => c.IsActive)) {
				activeAdapter.Commitments = r.Result.Where (c => c.IsActive).ToList ();
				activeAdapter.NotifyDataSetInvalidated ();
				buttonVolunteer.Visibility = ViewStates.Gone;
			} else {
				buttonVolunteer.Visibility = ViewStates.Visible;
			}

			if (r.Result.Any (c => !c.IsActive)) {
				inactiveAdapter.Commitments = r.Result.Where (c => !c.IsActive).ToList ();
				inactiveAdapter.NotifyDataSetInvalidated ();
			}
		}
	}

	public class CommitmentsAdapter : BaseAdapter<Commitment> 
	{
		public CommitmentsAdapter(Activity parent) 
		{
			ParentActivity = parent;
			Commitments = new List<Commitment> ();
		}

		public Activity ParentActivity { get;set; }

		public List<Commitment> Commitments { get; set; }

		public override long GetItemId (int position) { return position; }
		public override int Count { get { return Commitments.Count; } }
		public override Commitment this [int index] { get { return Commitments [index]; } }

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			var view = convertView
				?? ParentActivity.LayoutInflater.Inflate (global::Android.Resource.Layout.SimpleListItem2, parent, false);

			var text1 = view.FindViewById<TextView> (global::Android.Resource.Id.Text1);
			var text2 = view.FindViewById<TextView> (global::Android.Resource.Id.Text2);

			var c = Commitments [position];
			text1.Text = c.DisasterId.ToString();
			text2.Text = string.Format ("{0} - {1}", c.StartDate.ToString ("MMM d yyyy"), c.EndDate.ToString ("MMM d yyyy"));

			return view;
		}
	}
}

