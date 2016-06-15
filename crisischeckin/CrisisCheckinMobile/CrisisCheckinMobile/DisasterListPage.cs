using System;
using System.Collections.Generic;
using CrisisCheckinMobile.ViewModels;
using Xamarin.Forms;

namespace CrisisCheckinMobile
{
    public class DisasterListPage : ContentPage
    {
        private readonly ListView _disasterListView;
        private readonly EventHandler<SelectedItemChangedEventArgs> _itemSelectedEventHandler;

        public List<DisasterListItemViewModel> Data
        {
            get;
            set;
        }

        public DisasterListPage()
        {
            // TODO: Replace sample data creation with API call
            Data = new List<DisasterListItemViewModel>
            {
                new DisasterListItemViewModel(1, "Terrible Disaster", "Working - until August 12, 2015",
                    new CommitmentViewModel
                    {
                        Id = 1,
                        DisasterId = 1,
                        Status = CommitmentStatus.Here,
                        PersonId = 2,
                        PersonIsCheckedIn = true,
                        EndDate = new DateTime(2015, 8, 12)
                    }),
                new DisasterListItemViewModel(2, "Disaster Name 2", "", new CommitmentViewModel()),
                new DisasterListItemViewModel(3, "Disaster Name 3", "Planned - September 5 - 21, 2015",
                    new CommitmentViewModel
                    {
                        Id = 2,
                        DisasterId = 3,
                        PersonId = 3,
                        Status = CommitmentStatus.Planned,
                        StartDate = new DateTime(2015, 9, 5),
                        EndDate = new DateTime(2015, 9, 21)
                    })
            };
            _itemSelectedEventHandler = (sender, args) =>
            {
                if (args.SelectedItem == null)
                    return;

                var listView = sender as ListView;
                var selectedItem = args.SelectedItem as DisasterListItemViewModel;
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