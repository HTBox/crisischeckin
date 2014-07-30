
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
using System.Threading.Tasks;
using CrisisCheckin.Shared;

namespace CrisisCheckin.Android
{
	[Activity (Label = "Commitment Details")]			
	public class CommitmentDetailsActivity : Activity
	{
		IWebService webService;
		TextView textViewName;
		TextView textViewDescription;
		TextView textViewStartDate;
		TextView textViewEndDate;
		TextView textViewCluster;
		Button buttonCheckIn;

		Commitment commitment = null;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.CommitmentDetails);

			webService = WebServiceFactory.Create ();

			//Deserialize the commitment that was passed in
			commitment = JsonSerializer.Deserialize<Commitment> (this.Intent.GetStringExtra ("COMMITMENT"));

			textViewName = FindViewById<TextView> (Resource.Id.textViewName);
			textViewDescription = FindViewById<TextView> (Resource.Id.textViewDescription);
			textViewStartDate = FindViewById<TextView> (Resource.Id.textViewStartDate);
			textViewEndDate = FindViewById<TextView> (Resource.Id.textViewEndDate);
			textViewCluster = FindViewById<TextView> (Resource.Id.textViewCluster);
			buttonCheckIn = FindViewById<Button> (Resource.Id.buttonCheckIn);

			//TODO: Need to add meaningful name/descriptions
			textViewName.Text = "TODO: Put in Name";
			textViewDescription.Text = "TODO: Put in Desc";
			textViewStartDate.Text = commitment.StartDate.ToString ("ddd MMM d, yyyy");
			textViewEndDate.Text = commitment.EndDate.ToString ("ddd MMM d, yyyy");
			textViewCluster.Text = "TODO: Put in Cluster";

			buttonCheckIn.Click += async delegate {
			
				//TODO: Create confirmation dialog  (Are you sure: Yes/No)

				var confirm = true;

				if (confirm) {
					var checkedIn = commitment.IsActive;
				
					if (checkedIn) {
						var r = await webService.CheckOutAsync(new CheckOutRequest { Username = App.Settings.SignedInUsername });
						checkedIn = !(r.Succeeded && r.Result);
					} else {
						var r = await webService.CheckInAsync(new CheckInRequest { Username = App.Settings.SignedInUsername });
						checkedIn = r.Succeeded && r.Result;
					}

					buttonCheckIn.Text = checkedIn ? "Check Out" : "Check In";
					commitment.IsActive = checkedIn;
				}
			};

		}
	}
}

