using CrisisCheckinMobile.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CrisisCheckinMobile
{
    public class ResourceViewPage : ContentPage
    {
        public ResourceViewPage(ResourceListItemViewModel resourceItem)
        {
            var task = Init(resourceItem);
        }

        private async Task Init(ResourceListItemViewModel resourceItem)
        {

            Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
            BackgroundColor = Constants.HtBoxDarkBrown;

            this.Title = resourceItem.Type;

            var descriptionLabel = new Label
            {
                Text = resourceItem.Description,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                TextColor = Constants.HtBoxLightBrown
            };

            var personLabel = new Label
            {
                Text = $"Contact Person: {resourceItem.PersonFullName}",
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.White
            };

            var stateLabel = new Label
            {
                Text = $"State: {resourceItem.Location_State}",
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.White
            };

            var quantityLabel = new Label
            {
                Text = $"Qty: {resourceItem.Qty.ToString()}",
                FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
                TextColor = Color.White
            };

            Content = new ScrollView
            {
                Content = new StackLayout
                {
                    Spacing = 10,
                    Children = { descriptionLabel, personLabel, stateLabel, quantityLabel }
                }
            };

        }
    }
}
