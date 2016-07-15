using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrisisCheckinMobile.ApiClient;
using CrisisCheckinMobile.CustomRenderers;
using CrisisCheckinMobile.Models;
using Xamarin.Forms;

namespace CrisisCheckinMobile.Views
{
    class RequestDetailPage : ContentPage
    {

        public RequestDetailPage(RequestDto request)
        {
            var task = Init(request);
        }

        private async Task Init(RequestDto selectedRequest)
        {
            BackgroundColor = Constants.HtBoxDarkBrown;
            Title = selectedRequest.Description;

            var descriptionLabel = new Label
            {
                Text = selectedRequest.Description,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Constants.HtBoxLightBrown
            };

            var locationLabel = new Label
            {
                Text = $"Location: {selectedRequest.Location}",
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.White
            };

            var createdLabel = new Label
            {
                Text = $"Created On: {selectedRequest.CreatedDate.ToString("MMMM dd, yyyy H:mm")}",
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.White
            };

            var endLabel = new Label
            {
                Text = $"Ends On: {selectedRequest.EndDate.ToString("MMMM dd, yyyy H:mm")}",
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.White
            };

            var statusLabel = new Label
            {
                Text = $"Completed: {selectedRequest.Completed}",
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.White
            };



            if (selectedRequest.Completed == false)
            {
                var completeButton = BuildCompleteButton();
                await GetCompleteButtonHandler(selectedRequest, completeButton);

                Content = new ScrollView
                {
                    Content = new StackLayout
                    {
                        Spacing = 10,
                        Padding = 10,
                        Children = {descriptionLabel, locationLabel, createdLabel, endLabel, statusLabel, completeButton}
                    }
                };
            }
            else
            {
                Content = new ScrollView
                {
                    Content = new StackLayout
                    {
                        Spacing = 10,
                        Padding = 10,
                        Children = { descriptionLabel, locationLabel, createdLabel, endLabel, statusLabel }
                    }
                };
            }
        }

        private async Task GetCompleteButtonHandler(RequestDto selectedRequest, PaddedButton completeButton)
        {
            completeButton.Clicked += async (o, e) =>
            {
                ICrisisCheckInApiClient apiClient = new CrisisCheckInApiClient();
                await apiClient.CompleteRequest(selectedRequest.RequestId);
                await Navigation.PopAsync();
            };
        }

        private PaddedButton BuildCompleteButton()
        {
            var completeButton = new PaddedButton
            {
                Text = "Complete",
                TextColor = Color.White,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BorderWidth = 1.0,
                BorderColor = Color.White,
                BackgroundColor = Constants.HtBoxRed,
                Margin = new Thickness(0, 30, 0, 0)
            };
            return completeButton;
        }
    }
}
