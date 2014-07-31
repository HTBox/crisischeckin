using System;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using System.Threading.Tasks;
using CrisisCheckin.Shared;
using System.Linq;

namespace CrisisCheckin
{
	public class CommitmentsViewController : DialogViewController
	{
		IWebService webService;

		UIBarButtonItem logout;
		UIBarButtonItem volunteer;

		public CommitmentsViewController () :
			base (MonoTouch.UIKit.UITableViewStyle.Grouped, new RootElement ("Commitments"), true)
		{
			webService = WebServiceFactory.Create ();

			Root = new RootElement ("Commitments") {
				new Section ("Active Commitments") {

				},
				new Section ("Inactive / Past Commitments") {

				}
			};
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			logout = new UIBarButtonItem (UIImage.FromFile ("Images/logout.png"), UIBarButtonItemStyle.Plain, delegate {
			
				var avConfirm = new UIAlertView ("Sign Out?", "Are you sure you want to Sign Out?", null, "No", "Yes");
				avConfirm.Clicked += (sender, e) => {

					if (e.ButtonIndex == 1)
						NavigationController.PopViewControllerAnimated (true);
				};
				avConfirm.Show ();
			
			});

			volunteer = new UIBarButtonItem (UIImage.FromFile ("Images/volunteer.png"), UIBarButtonItemStyle.Plain, delegate {

				//TODO: Go to disasters view controller

			});

			NavigationItem.LeftBarButtonItem = logout;
			NavigationItem.RightBarButtonItem = volunteer;

		}

		public override async void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			await Refresh ();
		}

		async Task Refresh ()
		{
			ProgressHud.Show ("Refreshing");

			var sectionActive = Root [0];
			var sectionInactive = Root [1];

			// Clear existing items
			sectionActive.Clear ();
			sectionInactive.Clear ();

			volunteer.Enabled = false;

			// Call the web service
			var r = await webService.GetCommitmentsAsync (new CommitmentsRequest {
				Username = AppDelegate.Settings.SignedInUsername,
				Password = AppDelegate.Settings.SignedInPassword
			});

			if (!r.Succeeded) {
				Utility.ShowError ("Request Failed", "Failed to Fetch Commitments for " + AppDelegate.Settings.SignedInUsername);

				ProgressHud.Dismiss ();
				return;
			}

			if (r.Result.Any (c => c.IsActive)) {

				foreach (var ac in r.Result.Where (c => c.IsActive)) {
					var text = ac.DisasterId + " - " + ac.StartDate.ToString ("yyyy-MM-dd") + " to " + ac.EndDate.ToString ("yyyy-MM-dd");

					sectionActive.Add (new StyledStringElement (text, () => {

						//TODO: Go to details

					}) { Accessory = UITableViewCellAccessory.DisclosureIndicator });
				}
				volunteer.Enabled = false;
			} else {
				volunteer.Enabled = true;
			}

			// Add the inactive / past commitments
			if (r.Result.Any (c => !c.IsActive)) {
				foreach (var pc in r.Result.Where (c => !c.IsActive)) {
					var text = pc.DisasterId + " - " + pc.StartDate.ToString ("yyyy-MM-dd") + " to " + pc.EndDate.ToString ("yyyy-MM-dd");

					sectionInactive.Add (new StyledStringElement (text, () => {

						//TODO: Go to details

					}) { Accessory = UITableViewCellAccessory.DisclosureIndicator });
				}
			}

			ProgressHud.Dismiss ();
		}
	}
}

