using CrisisCheckinMobile.Models;
using Xamarin.Forms;

namespace CrisisCheckinMobile
{
    public class App : Application
    {
        public static string AuthToken { get; set; }
        //public static Commitment Commitment { get; set; }
        public static string BackIcon = "";
        public static string ReportTroubleIcon = "";
        public static string ProfileIcon = "";

        public App()
        {
            MainPage = GetMainPage();
        }
        public static Page GetMainPage()
        {
            NavigationPage mainPage;
            if (string.IsNullOrWhiteSpace(AuthToken))
            {
                mainPage = new NavigationPage(new LoginPage())
                {
                    BarBackgroundColor = Constants.HtBoxRed,
                    BarTextColor = Color.White
                };
            }
            else
            {
                mainPage = new NavigationPage(new DisasterListPage())
                {
                    BarBackgroundColor = Constants.HtBoxRed,
                    BarTextColor = Color.White
                };
            }

            //if (Commitment != null)
            //{
            //    mainPage.Navigation.PushAsync(new CommitmentPage());
            //}

            return mainPage;
        }
    }
}
