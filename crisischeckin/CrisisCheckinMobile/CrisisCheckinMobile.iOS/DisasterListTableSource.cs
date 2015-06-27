using System;
using MonoTouch.UIKit;
using System.Collections.Generic;
using MonoTouch.Foundation;

namespace CrisisCheckinMobile.iOS
{
    public class DisasterListTableSource : UITableViewSource
    {
        const string CellIdentifier = "disasterListCell"; // set in the Storyboard

        public List<DisasterListViewModel> DisasterListItems
        {
            get;
            set;
        }

        // in the MyTableSource class
        public UITableViewController Parent
        {
            get;
            set;
        }
            
        public DisasterListTableSource(List<DisasterListViewModel> items, UITableViewController parent)
        {
            DisasterListItems = items;
            Parent = parent;
        }

        #region implemented abstract members of UITableViewSource

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            // see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events
            // NOTE: Don't call the base implementation on a Model class
            // in a Storyboard, Dequeue will ALWAYS return a cell, 
            var cell = tableView.DequeueReusableCell (CellIdentifier);
            var item = DisasterListItems[indexPath.Row];
            cell.TextLabel.Text = item.DisasterName;
            cell.DetailTextLabel.Text = item.DisasterStatusAndDate;

            return cell;
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            // see http://docs.xamarin.com/guides/ios/application_fundamentals/delegates,_protocols,_and_events
            // NOTE: Don't call the base implementation on a Model class
            return DisasterListItems.Count;
        }

//        public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
//        {
//            // TODO: Do something meaningful with the disaster selection
//            new UIAlertView("Row Selected", DisasterListItems[indexPath.Row], null, "OK", null).Show();
//            tableView.DeselectRow (indexPath, true); // iOS convention is to remove the highlight

//            // conditionally get and push the view controller based on the status
              // and pass the appropriate data
//            Parent.NavigationController.PushViewController(new UIViewController(), false);
//        }

        #endregion
    }
}