using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrisisCheckinMobile.ApiClient;
using CrisisCheckinMobile.Models;
using Xamarin.Forms;

namespace CrisisCheckinMobile.Views
{
    public class RequestListPage : ContentPage
    {
        private ListView _requestListView;
        public IEnumerable<ItemsView> ListViewRequests { get; set; }
        private EventHandler<SelectedItemChangedEventArgs> _itemSelectedEventHandler;

        public RequestListPage()
        {
            var task = Init();
        }

        private async Task Init()
        {
            _itemSelectedEventHandler = (sender, args) =>
            {
                if (args.SelectedItem == null)
                    return;

                var selectedItem = args.SelectedItem as RequestDto;

                Navigation.PushAsync(new RequestDetailPage(selectedItem));
            };

            var cell = CreateRequestCell();

            Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
            Title = "Requests";
            BackgroundColor = Constants.HtBoxDarkBrown;

            _requestListView = new ListView()
            {
                ItemTemplate = cell,
                BackgroundColor = Constants.HtBoxDarkBrown
            };
            _requestListView.ItemSelected += _itemSelectedEventHandler;
            Content = _requestListView;
        }

        protected override void OnAppearing()
        {
            var t = GetRequests();
        }

        private async Task GetRequests()
        {
            ICrisisCheckInApiClient apiClient = new CrisisCheckInApiClient();
            var requests = await apiClient.GetRequests(2);

            _requestListView.ItemsSource = requests;
        }


        private DataTemplate CreateRequestCell()
        {
            var cell = new DataTemplate(typeof (TextCell));
            cell.SetBinding(TextCell.TextProperty, new Binding("Description"));
            cell.SetBinding(TextCell.DetailProperty, new Binding("Location"));
            cell.SetValue(TextCell.TextColorProperty, Color.White);
            cell.SetValue(TextCell.DetailColorProperty, Constants.HtBoxLightBrown);
            return cell;
        }
    }
}
