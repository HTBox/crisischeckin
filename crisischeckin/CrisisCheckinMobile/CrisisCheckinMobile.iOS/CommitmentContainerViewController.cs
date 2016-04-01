using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace CrisisCheckinMobile.iOS
{
	public partial class CommitmentContainerViewController : UIViewController
	{
        const string _crisisCheckInStoryboardName = "CrisisCheckIn";
        UIStoryboard _mainStoryboard;

        private UIViewController _firstViewController;
        private UIViewController _secondViewController;

        private UIViewController CurrentViewController
        {
            get;
            set;
        }

        private string CurrentViewControllerName
        {
            get;
            set;
        }

        public CommitmentViewModel ViewModel
        {
            get;
            set;
        }
            
        public UIStoryboard MainStoryboard
        {
            get { return _mainStoryboard ?? (_mainStoryboard = UIStoryboard.FromName(_crisisCheckInStoryboardName, 
                NSBundle.MainBundle)); }
        }

        // Creates an instance of viewControllerName from the given storyboard
        public UIViewController GetViewController(UIStoryboard storyboard, string viewControllerName)
        {
            return storyboard.InstantiateViewController(viewControllerName);
        }

		public CommitmentContainerViewController (IntPtr handle) : base (handle)
		{
            
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            _firstViewController = GetViewController(MainStoryboard, Constants.FirstControllerName);
            _secondViewController = GetViewController(MainStoryboard, Constants.SecondControllerName);

            if (ViewModel == null) // or something else
            {
                CurrentViewControllerName = Constants.FirstControllerName;
                DisplayContentController(_firstViewController);
            }
            else
            {
                // do this based on condition of state of ViewModel since it's not null
                CurrentViewControllerName = Constants.SecondControllerName;
                DisplayContentController(_secondViewController);
            }
        }
            
        public void ChangeControllers(string controllerName)
        {
            if (controllerName == Constants.FirstControllerName)
            {
                HideContentController(_secondViewController);
                DisplayContentController(_firstViewController);
            }
            else if (controllerName == Constants.SecondControllerName)
            {
                HideContentController(_firstViewController);
                DisplayContentController(_secondViewController);
            }
            else
            {
                if (controllerName == null)
                {
                    throw new ArgumentException("Controller name is null.", "controllerName");
                }
                throw new ArgumentException("Unknown controller name.", "controllerName");
            }
        } 

        public void StatusChanged(string controllerName)
        {
            if (controllerName != CurrentViewControllerName)
            {
                ChangeControllers(controllerName);
                CurrentViewControllerName = controllerName;
            }
        }
            
        private void DisplayContentController(UIViewController content)
        {
            AddChildViewController(content);
            content.View.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
            content.View.Frame = new CGRect(0, 0, View.Frame.Size.Width, View.Frame.Size.Height);
            View.AddSubview(content.View);
            content.DidMoveToParentViewController(this);
        }

        private void HideContentController(UIViewController content)
        {
            content.WillMoveToParentViewController(null);
            content.View.RemoveFromSuperview();
            content.RemoveFromParentViewController();
        }
	}
}