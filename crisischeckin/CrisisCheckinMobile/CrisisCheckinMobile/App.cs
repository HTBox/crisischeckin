using CrisisCheckinMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CrisisCheckinMobile
{
    public class App
    {
        public static string AuthToken { get; set; }
        public static Commitment Commitment { get; set; }
        public static string BackIcon = "";
        public static string ReportTroubleIcon = "";
        public static string ProfileIcon = "";
        public static readonly Color ContentBackgroundColor = Color.FromHex("4f2c1d");
        public static readonly Color MastHeadBackgroundColor = Color.FromHex("d22630");
        public static readonly Color LowLightColor = Color.FromHex("ceb888");
        public static readonly Color TextColor = Color.White;

        public static Page GetMainPage()
        {
            //NavigationPage mainPage;
            //if (string.IsNullOrWhiteSpace(AuthToken)) {
            //    mainPage = new NavigationPage(new LoginPage());
            //} else {
            //    mainPage = new NavigationPage(new DisasterListPage());
            //}

            //if (Commitment != null) {
            //    mainPage.Navigation.PushAsync(new CommitmentPage());
            //}

            //return mainPage;
            return null;
        }



    }

}
