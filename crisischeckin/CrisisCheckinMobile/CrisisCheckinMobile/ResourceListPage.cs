using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrisisCheckinMobile.ApiClient;
using CrisisCheckinMobile.ViewModels;
using Xamarin.Forms;

namespace CrisisCheckinMobile
{
    //public class ResourceListPage : MasterDetailPage

    public class ResourceListPage : ContentPage
    {
        private ListView _resourceListView;
        private EventHandler<SelectedItemChangedEventArgs> _itemSelectedEventHandler;

        public IEnumerable<ResourceListItemViewModel> ResourceList
        {
            get;
            set;
        }

        public ResourceListPage()
        {
            var task = Init();
           
        }

        private async Task Init()
        {
            _itemSelectedEventHandler = (sender, args) =>
            {
                if (args.SelectedItem == null)
                    return;

                var selectedItem = args.SelectedItem as ResourceListItemViewModel;

                Navigation.PushAsync(new ResourceViewPage(selectedItem));

            };

            BackgroundColor = Constants.HtBoxDarkBrown;

            Padding = new Thickness(0, Device.OnPlatform(20,0,0), 0, 0);

            Title = "Resources for: Big Disaster"; //TODO - Add Disaster Name

            // TODO: Get data from the API
            //ICrisisCheckInApiClient apiClient = new CrisisCheckInApiClient();
            //var dtos = await apiClient.GetCommitmentsList(2); //TODO: wire up to Auth0 so we don't have to pass person ID
            //Data = dtos.Select(c => new DisasterListItemViewModel(c));

            ResourceList = new List<ResourceListItemViewModel>
            {
                new ResourceListItemViewModel
                {
                    Description = "Dump Truck",
                    Type = "Heavy Machines",
                    PersonFullName = "Bob Smith",
                    Location_State = "Maine",
                    Qty = 2
                },
                 new ResourceListItemViewModel
                {
                    Description = "Poland Spring Water",
                    Type = "Water",
                    PersonFullName = "Mike Smith",
                    Location_State = "Maine",
                    Qty = 500
                }
            };

            _resourceListView = new ListView
            {
                ItemsSource = ResourceList,
                BackgroundColor = Constants.HtBoxDarkBrown
            };
            _resourceListView.ItemSelected += _itemSelectedEventHandler;
            var cell = new DataTemplate(typeof(TextCell));
            cell.SetBinding(TextCell.TextProperty, new Binding("Type"));
            cell.SetBinding(TextCell.DetailProperty, new Binding("Description"));
            cell.SetValue(TextCell.TextColorProperty, Color.White);
            cell.SetValue(TextCell.DetailColorProperty, Constants.HtBoxLightBrown);
            _resourceListView.ItemTemplate = cell;

            Content = _resourceListView;
        }

        ~ResourceListPage() // TODO: put somewhere else?
        {
            if (_resourceListView != null)
            {
                _resourceListView.ItemSelected -= _itemSelectedEventHandler;
            }
        }
    }
}