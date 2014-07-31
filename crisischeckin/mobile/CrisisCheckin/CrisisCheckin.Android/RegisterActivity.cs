
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
	[Activity (Label = "RegisterActivity")]			
	public class RegisterActivity : Activity
	{
		IWebService webService;

		EditText editTextFirstName;
		EditText editTextLastName;
		EditText editTextEmail;
		EditText editTextPhone;
		EditText editTextUsername;
		EditText editTextPassword;
		EditText editTextPassword2;
		Spinner spinnerCluster;
		Button buttonCreate;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			SetContentView (Resource.Layout.Register);

			webService = WebServiceFactory.Create ();

			editTextFirstName = FindViewById<EditText> (Resource.Id.editTextFirstName);
			editTextLastName = FindViewById<EditText> (Resource.Id.editTextLastName);
			editTextEmail = FindViewById<EditText> (Resource.Id.editTextEmail);
			editTextPhone = FindViewById<EditText> (Resource.Id.editTextPhone);
			editTextUsername = FindViewById<EditText> (Resource.Id.editTextUsername);
			editTextPassword = FindViewById<EditText> (Resource.Id.editTextPassword);
			editTextPassword2 = FindViewById<EditText> (Resource.Id.editTextPassword2);
			spinnerCluster = FindViewById<Spinner> (Resource.Id.spinnerCluster);
			buttonCreate = FindViewById<Button> (Resource.Id.buttonCreate);

			//Let's disable the button until the clusters have loaded
			buttonCreate.Enabled = false;

			Refresh ();
		}

		async Task Refresh()
		{
			var r = await webService.GetClustersAsync(new ClustersRequest ());

			if (!r.Succeeded) {
				buttonCreate.Enabled = false;
				Toast.MakeText (this, "Failed to Fetch Clusters.  Please try again later.", ToastLength.Short).Show ();
				return;
			}

			if (r.Result.Any ()) {

				var items = (from c in r.Result select c.Name).ToArray();

				spinnerCluster.Adapter = new ArrayAdapter<string> (this, global::Android.Resource.Layout.SimpleSpinnerItem, items);
				buttonCreate.Enabled = true;
			}
		}
	}
}

