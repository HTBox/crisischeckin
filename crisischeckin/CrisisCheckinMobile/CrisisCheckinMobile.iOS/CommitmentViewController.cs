using UIKit;
using System;

namespace CrisisCheckinMobile.iOS
{
	public partial class CommitmentViewController : UIViewController
	{
        public CommitmentViewModel Data
        {
            get;
            set;
        }

        private CommitmentContainerViewController _container;
        private CommitmentContainerViewController Container
        {
            get
            {
                return (_container ?? (_container = GetContainer()));
            }
        }

        private CommitmentContainerViewController GetContainer()
        {
            foreach (var c in ChildViewControllers)
            {
                var container = c as CommitmentContainerViewController;
                if (container != null)
                {
                    return container;
                }
            }
            return null;
        }

		public CommitmentViewController (IntPtr handle) : base (handle)
		{
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            DisasterStatusText.Layer.BorderColor = Constants.HtBoxTan.CGColor; // TODO: use whatever is in the mockup; may have to change
                                                                               // after migration to unified
            DisasterStatusText.BorderStyle = UITextBorderStyle.Line;
            DisasterStatusText.Layer.BorderWidth = 1f;

            //DisasterStatusText.ClipsToBounds = true; // might have to do this per James M. and Stack Overflow

            //DisasterStatusText.Layer.BorderColor = Constants.HtBoxTan;

            //UIPickerView statusPicker = new UIPickerView();
            //statusPicker.ShowSelectionIndicator = true;

            /*
             // Setup the toolbar
            UIToolbar toolbar = new UIToolbar();
            toolbar.BarStyle = UIBarStyle.Black;
            toolbar.Translucent = true;
            toolbar.SizeToFit();

            // Create a 'done' button for the toolbar and add it to the toolbar
            UIBarButtonItem doneButton = new UIBarButtonItem("Done", UIBarButtonItemStyle.Done,
                (s, e) => {
                this.ColorTextField.Text = selectedColor;
                this.ColorTextField.ResignFirstResponder();
            });
            toolbar.SetItems(new UIBarButtonItem[]{doneButton}, true);

            // Tell the textbox to use the picker for input
            this.ColorTextField.InputView = picker;

            // Display the toolbar over the pickers
            this.ColorTextField.InputAccessoryView = toolbar;
            */
        }

        private void TriggerStatusChange(string segueName)
        {
            Container.StatusChanged(segueName);
        }

        partial void View1Button_TouchUpInside(UIButton sender)
        {
            TriggerStatusChange(Constants.FirstControllerName);
        }

        partial void View2Button_TouchUpInside(UIButton sender)
        {
            TriggerStatusChange(Constants.SecondControllerName);
        }
	}

    public class CommitmentViewEventArgs : EventArgs
    {
        public CommitmentViewEventArgs(string segueName, CommitmentViewModel viewModel)
        {
            SegueName = segueName;
            ViewModel = viewModel;
        }

        public string SegueName { get; set; }

        public CommitmentViewModel ViewModel { get; set; }
    }
}