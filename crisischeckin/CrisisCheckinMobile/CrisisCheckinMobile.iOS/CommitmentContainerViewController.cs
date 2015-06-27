using MonoTouch.UIKit;
using System;
using System.Drawing;

namespace CrisisCheckinMobile.iOS
{
	public partial class CommitmentContainerViewController : UIViewController
	{
        private const string FirstSegueName = "FirstSegue";
        private const string SecondSegueName = "SecondSegue";
        private string CurrentSegueIdentifier
        {
            get;
            set;
        }

        public CommitmentViewModel ViewModel
        {
            get;
            set;
        }

		public CommitmentContainerViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            CurrentSegueIdentifier = FirstSegueName;
            if (ViewModel == null) // or something else
            {
                CurrentSegueIdentifier = FirstSegueName;
            }
            else
            {
                CurrentSegueIdentifier = SecondSegueName;
            }
            PerformSegue(CurrentSegueIdentifier, null);
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, MonoTouch.Foundation.NSObject sender)
        {
            base.PrepareForSegue(segue, sender);

            if (segue.Identifier == FirstSegueName)
            {
                var destinationViewController = segue.DestinationViewController;
                if (ChildViewControllers.Length > 0)
                {                   
                    SwapViewControllers(ChildViewControllers[0], destinationViewController);
                }
                else
                {
                    AddChildViewController(destinationViewController);
                    UIView destView = destinationViewController.View;
                    destView.AutoresizingMask = UIViewAutoresizing.FlexibleWidth | UIViewAutoresizing.FlexibleHeight;
                    destView.Frame = new RectangleF(0, 0, this.View.Frame.Size.Width, this.View.Frame.Size.Height);
                    this.View.AddSubview(destView);
                    destinationViewController.DidMoveToParentViewController(this);
                }
                CurrentSegueIdentifier = FirstSegueName;
            }
            else if (segue.Identifier == SecondSegueName)
            {
                SwapViewControllers(ChildViewControllers[0], segue.DestinationViewController);
                CurrentSegueIdentifier = SecondSegueName;
            }
            else
            {
                throw new ArgumentException("Unknown segue.");
            }
        }

        private void SwapViewControllers(UIViewController fromViewController, UIViewController toViewController)
        {
            toViewController.View.Frame = new RectangleF(0, 0, this.View.Frame.Size.Width, this.View.Frame.Size.Height);
            fromViewController.WillMoveToParentViewController(null);
            AddChildViewController(toViewController);
            Transition(fromViewController, toViewController, 0.0, UIViewAnimationOptions.TransitionNone, (() => {}), (finished =>
                {
                    fromViewController.RemoveFromParentViewController();
                    toViewController.DidMoveToParentViewController(this);
                }));
        }
	}
}