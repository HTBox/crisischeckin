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

            Children.Add(new NavigationPage(new ProfileView()
            {
                Title = "Home"
            })
            {
                Title = "Home",
                Icon = "iosMenuIcons/CCI-menuHomeHi.png",
                BarBackgroundColor = Constants.HtBoxRed,
                BarTextColor = Color.White
            });

            Children.Add(new NavigationPage(new ResourceListPage())
            {
                Title = "Resources",
                Icon = "iosMenuIcons/CCI-menuSettingHi.png",
                BarBackgroundColor = Constants.HtBoxRed,
                BarTextColor = Color.White
            });

            Children.Add(new NavigationPage(new MyRequests()
            {
                Title = "Requests"
            })
            {
                Title = "Requests",
                Icon = "iosMenuIcons/CCI-menuListHi.png",
                BarBackgroundColor = Constants.HtBoxRed,
                BarTextColor = Color.White
            });

            Children.Add(new NavigationPage(new TemporaryView()
            {
                Title = "Report"
            })
            {
                Title = "Report",
                Icon = "iosMenuIcons/CCI-menuRptHi.png",
                BarBackgroundColor = Constants.HtBoxRed,
                BarTextColor = Color.White
            });

        }
    }
}
