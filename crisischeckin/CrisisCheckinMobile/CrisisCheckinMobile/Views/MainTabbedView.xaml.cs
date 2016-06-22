using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace CrisisCheckinMobile.Views
{
    public partial class MainTabbedView : TabbedPage
    {
        public MainTabbedView()
        {
            BackgroundColor = Color.White;

            var homePage = new NavigationPage(new ProfileView());
            homePage.Title = "Home";
            homePage.Icon = "iosMenuIcons/CCI-menuHomeHi.png";
            homePage.BarBackgroundColor = Constants.HtBoxRed;
            homePage.BarTextColor = Color.White;
            Children.Add(homePage);

            Children.Add(new ResourceListPage()
            {
                Title = "Resources",
                Icon = "iosMenuIcons/CCI-menuSettingHi.png"
            });

            Children.Add(new MyRequests()
            {
                Title = "Requests",
                Icon = "iosMenuIcons/CCI-menuListHi.png"
            });

            Children.Add(new TemporaryView()
            {
                Title = "Report",
                Icon = "iosMenuIcons/CCI-menuRptHi.png"
            });

        }
    }
}
