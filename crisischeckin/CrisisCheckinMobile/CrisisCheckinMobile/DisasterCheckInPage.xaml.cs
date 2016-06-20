using System;
using System.Collections.Generic;
using CrisisCheckinMobile.ViewModels;
using CrisisCheckinMobile.Models;
using Xamarin.Forms;
using System.Linq;

namespace CrisisCheckinMobile
{
    public partial class DisasterCheckInPage : ContentPage
    {
        private List<DisasterViewModel> Disasters { get; set; }
        private EventHandler _pickerSelectedItemChangedHandler;
        private DisasterViewModel SelectedDisaster { get; set; }

        public DisasterCheckInPage()
        {
            InitializeComponent();
            
            // TODO: wire up to API and remove this fake data
            var disasters = new List<DisasterViewModel>
            {
                new DisasterViewModel(new DisasterDto
                {
                    Id = 1,
                    Name = "Hurricane"
                }),
                new DisasterViewModel(new DisasterDto
                {
                    Id = 2,
                    Name = "Terrible Disaster"
                })
            };

            var disasterNames = disasters.Select(d => d.Name).ToList();
            foreach (var name in disasterNames)
            {
                disasterNamePicker.Items.Add(name);
            }
            Disasters = disasters;
            _pickerSelectedItemChangedHandler = (sender, args) =>
            {
                if (Disasters == null)
                {
                    return;
                }

                Picker picker = (Picker)sender;
                int selectedIndex = picker.SelectedIndex;
                if (selectedIndex == -1)
                    return;
                string selectedItem = picker.Items[selectedIndex];
                SelectedDisaster = Disasters.First(d => d.Name == selectedItem);
            };
            disasterNamePicker.SelectedIndexChanged += _pickerSelectedItemChangedHandler;

            BackgroundColor = Constants.HtBoxDarkBrown;
            var textColor = Constants.HtBoxTan;

            selectDisaster.TextColor = textColor;
            activity.TextColor = textColor;

            startDateLabel.TextColor = textColor;
            startDate.MinimumDate = DateTime.Today;

            endDateLabel.TextColor = textColor;
            endDate.MinimumDate = DateTime.Today;

            locationLabel.TextColor = textColor;

            checkInToDisasterButton.TextColor = textColor;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (_pickerSelectedItemChangedHandler != null)
            {
                disasterNamePicker.SelectedIndexChanged -= _pickerSelectedItemChangedHandler;
            }
        }
    }
}
