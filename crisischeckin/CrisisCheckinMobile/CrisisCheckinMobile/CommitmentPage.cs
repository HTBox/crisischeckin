using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CrisisCheckinMobile
{
    public class CommitmentPage : ContentPage
    {
        public CommitmentPage()
        {
            var grid = new Grid() {
                RowDefinitions = new RowDefinitionCollection {
                    new RowDefinition { },
                    new RowDefinition { },
                }
            };

            var plannedLabel = new Label {
                Text = "I am"
            };
            plannedLabel.SetValue(Grid.RowProperty, 0);

            grid.Children.Add(plannedLabel);
        }
    }
}
