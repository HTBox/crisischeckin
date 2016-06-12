using UIKit;
using System;
using System.Collections.Generic;
using CrisisCheckinMobile.ViewModels;

namespace CrisisCheckinMobile.iOS
{
	partial class CrisisCheckInTableViewController : UITableViewController
	{
        List<DisasterListItemViewModel> Data
        {
            get;
            set;
        }

		public CrisisCheckInTableViewController (IntPtr handle) : base (handle)
		{
            // TODO: Replace sample data creation with API call
            //Data = new List<DisasterListViewModel>();
            //Data.Add(new DisasterListViewModel(TODO, "Terrible Disaster", "Working - until August 12, 2015"));
            //Data.Add(new DisasterListViewModel(TODO, "Disaster Name 2", ""));
            //Data.Add(new DisasterListViewModel(TODO, "Disaster Name 3", "Planned - September 5 - 21, 2015"));
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Sets the background of the table so empty cells don't have a white background
            // The table view itself has its background color set to clear in the storyboard
            var background = new UIView(TableView.Bounds);
            background.BackgroundColor = Constants.HtBoxDarkBrown;
            TableView.BackgroundView = background;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            TableView.Source = new DisasterListTableSource(Data, this);
        }
	}
}
