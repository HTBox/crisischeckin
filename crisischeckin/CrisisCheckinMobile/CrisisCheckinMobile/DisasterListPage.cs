using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrisisCheckinMobile.ApiClient;
using CrisisCheckinMobile.ViewModels;
using Xamarin.Forms;

namespace CrisisCheckinMobile
{
    public class DisasterListPage : ContentPage
    {
        private ListView _disasterListView;
        private EventHandler<SelectedItemChangedEventArgs> _itemSelectedEventHandler;

        public IEnumerable<DisasterViewModel> Data
        {
            get;
            set;
        }

        public DisasterListPage()
        {
            var task = Init();

            // TODO: Replace sample data creation with API call
            //Data = new List<DisasterViewModel>
            //{
            //    new DisasterViewModel(1, "Terrible Disaster", "Working - until August 12, 2015",
            //        new CommitmentViewModel
            //        {
            //            Id = 1,
            //            DisasterId = 1,
            //            Status = CommitmentStatus.Here,
            //            PersonId = 2,
            //            PersonIsCheckedIn = true,
            //            EndDate = new DateTime(2015, 8, 12)
            //        }),
            //    new DisasterViewModel(2, "Disaster Name 2", "", new CommitmentViewModel()),
            //    new DisasterViewModel(3, "Disaster Name 3", "Planned - September 5 - 21, 2015",
            //        new CommitmentViewModel
            //        {
            //            Id = 2,
            //            DisasterId = 3,
            //            PersonId = 3,
            //            Status = CommitmentStatus.Planned,
            //            StartDate = new DateTime(2015, 9, 5),
            //            EndDate = new DateTime(2015, 9, 21)
            //        })
            //};

            
        }

        private async Task Init()
        {
            _itemSelectedEventHandler = (sender, args) =>
            {
                if (args.SelectedItem == null)
                    return;

                var listView = sender as ListView;
                var selectedItem = args.SelectedItem as DisasterViewModel;
                if (selectedItem != null)
                {
                    Navigation.PushAsync(new CommitmentPage(selectedItem.CommitmentData));
                }
                if (listView != null)
                {
                    listView.SelectedItem = null;
                }
            };

            BackgroundColor = Constants.HtBoxDarkBrown;

            // TODO: Implement a progress indicator
            ICrisisCheckInApiClient apiClient = new CrisisCheckInApiClient();
            var dtos = await apiClient.GetCommitmentsList(2); //TODO: wire up to Auth0 so we don't have to pass person ID
           // Data = dtos.Select(c => new DisasterViewModel(c));

            _disasterListView = new ListView
            {
                ItemsSource = Data,
                BackgroundColor = Constants.HtBoxDarkBrown
            };
            _disasterListView.ItemSelected += _itemSelectedEventHandler;
            var cell = new DataTemplate(typeof(TextCell));
            cell.SetBinding(TextCell.TextProperty, new Binding("DisasterName"));
            cell.SetBinding(TextCell.DetailProperty, new Binding("DisasterStatusAndDate"));
            cell.SetValue(TextCell.TextColorProperty, Color.White);
            cell.SetValue(TextCell.DetailColorProperty, Constants.HtBoxLightBrown);
            _disasterListView.ItemTemplate = cell;

            Content = _disasterListView;
        }

        ~DisasterListPage() // TODO: put somewhere else?
        {
            if (_disasterListView != null)
            {
                _disasterListView.ItemSelected -= _itemSelectedEventHandler;
            }
        }
    }
}