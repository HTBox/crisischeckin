
using Foundation;
using UIKit;
using System;

namespace CrisisCheckinMobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        const string _crisisCheckInStoryboardName = "CrisisCheckIn";
        const string _loginViewControllerName = "LoginPageViewController";
        const string _tabBarControllerName = "MainTabBarController";
        const string _disasterListViewControllerName = "DisasterListViewController";

        // class-level declarations
        public override UIWindow Window
        {
            get;
            set;
        }

        UIStoryboard _mainStoryboard;

        // Public property to access our main storyboard file
        public UIStoryboard MainStoryboard
        {
            get { return _mainStoryboard ?? (_mainStoryboard = UIStoryboard.FromName(_crisisCheckInStoryboardName, NSBundle.MainBundle)); }
        }

        // Creates an instance of viewControllerName from the given storyboard
        public UIViewController GetViewController(UIStoryboard storyboard, string viewControllerName)
        {
            return storyboard.InstantiateViewController(viewControllerName);
        }

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            UITabBarItem.Appearance.SetTitleTextAttributes(new UITextAttributes
                {
                    TextColor = Constants.HtBoxDarkBrown
                }, UIControlState.Normal);
            UITabBar.Appearance.SelectedImageTintColor = Constants.HtBoxRed;

            UIApplication.SharedApplication.SetStatusBarStyle(UIStatusBarStyle.LightContent, false); // sets the battery icon etc. to white text

            // Init the login controller and set it as the root view
            var loginViewController = GetViewController(MainStoryboard, _loginViewControllerName) as LoginViewController;
            loginViewController.OnLoginSuccess += LoginViewController_OnLoginSuccess;

            var c = GetViewController(MainStoryboard, "CommitmentScreen") as CommitmentViewController;
            Window.RootViewController = c; //loginViewController;

            return true;
        }

        void LoginViewController_OnLoginSuccess (object sender, EventArgs e)
        {
            // Successful login, so set the root view controller to the tabbed view controller
            var tabBarController = GetViewController(MainStoryboard, _tabBarControllerName) as UITabBarController;

            // make sure the unselected tab bar images are brown (original color)
            var items = tabBarController.TabBar.Items;
            for (var i = 0; i < items.Length; i++)
            {
                var image = items[i].Image;
                items[i].Image = image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysOriginal);
            }

            Window.RootViewController = tabBarController;
        }
    }
}