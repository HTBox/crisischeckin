using System;
using Xamarin.Forms;

namespace CrisisCheckinMobile
{
    public partial class DisasterCheckInPage : ContentPage
    {
        public DisasterCheckInPage()
        {
            InitializeComponent();

            BackgroundColor = Constants.HtBoxDarkBrown;

            selectDisaster.TextColor = Constants.HtBoxLightBrown;
            activity.TextColor = Constants.HtBoxLightBrown;

            startDateLabel.TextColor = Constants.HtBoxLightBrown;
            startDate.MinimumDate = DateTime.Today;

            endDateLabel.TextColor = Constants.HtBoxLightBrown;
            endDate.MinimumDate = DateTime.Today;

            checkInToDisasterButton.TextColor = Constants.HtBoxTan;
        }
    }
}
