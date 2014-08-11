using CrisisCheckinMobile.Models;
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
        readonly Commitment _commitment;

        public CommitmentPage(Commitment commitment)
        {
            _commitment = commitment;
            BindingContext = commitment;

            var grid = new Grid() {
                RowDefinitions = new RowDefinitionCollection {
                    new RowDefinition(),
                    new RowDefinition(),
                },
                ColumnDefinitions = new ColumnDefinitionCollection {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                }
            };

            var plannedLabel = new Label {
                Text = "I am"
            };
            plannedLabel.SetValue(Grid.RowProperty, 0);

            var plannedTextCell = new TextCell {

            };
            plannedTextCell.SetBinding(TextCell.TextProperty, "Status");
            grid.Children.Add(plannedLabel);

            Content = grid;
        }

        void SaveChanges()
        {
            App.ClientModel.SaveChanges();
        }
    }
}
