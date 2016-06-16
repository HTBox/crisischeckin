using UIKit;
using System;
using CrisisCheckinMobile.ViewModels;
using CrisisCheckinMobile.Models;

namespace CrisisCheckinMobile.iOS
{
	public partial class CommitmentViewController : UIViewController
	{
        public CommitmentViewModel Data
        {
            get;
            set;
        }

	    private CommitmentStatus _selectedStatus;
        private EventHandler<PickerChangedEventArgs> _statusSelectedEventHandler;
	    private EventHandler _barButtonClickEventHandler;
	    private PickerModel _pickerModel;
	    private UIPickerView _statusPicker;
	    private UIBarButtonItem _doneButton;

        private CommitmentContainerViewController _container;
        private CommitmentContainerViewController Container
        {
            get
            {
                return _container ?? (_container = GetContainer());
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

	    public override void ViewWillAppear(bool animated)
	    {
	        base.ViewWillAppear(animated);

            var rawStatuses = Enum.GetValues(typeof(CommitmentStatus));
            var statuses = new CommitmentStatus[rawStatuses.Length];
            var i = 0;
            foreach (CommitmentStatus status in rawStatuses)
            {
                statuses[i] = status;
                i++;
            }
            _pickerModel = new PickerModel(statuses);
            _statusSelectedEventHandler = (sender, args) =>
            {
                _selectedStatus = args.SelectedValue;
            };
            _pickerModel.PickerChanged += _statusSelectedEventHandler;

            _statusPicker = new UIPickerView
            {
                ShowSelectionIndicator = true,
                Model = _pickerModel
            };

            // Setup the toolbar
            var toolbar = new UIToolbar
            {
                BarStyle = UIBarStyle.Black,
                Translucent = true
            };
            toolbar.SizeToFit();

            _barButtonClickEventHandler = (s, e) =>
            {
                DisasterStatusText.Text = Enum.GetName(typeof(CommitmentStatus), _selectedStatus);
                DisasterStatusText.ResignFirstResponder();
            };
            // Create a 'done' button for the toolbar and add it to the toolbar
            _doneButton = new UIBarButtonItem("Done", UIBarButtonItemStyle.Done, _barButtonClickEventHandler);
            toolbar.SetItems(new[] { _doneButton }, true);

            // Tell the textbox to use the picker for input
            DisasterStatusText.InputView = _statusPicker;

            // Display the toolbar over the pickers
            DisasterStatusText.InputAccessoryView = toolbar;
	    }

	    public override void ViewWillDisappear(bool animated)
	    {
	        base.ViewWillDisappear(animated);
	        _pickerModel.PickerChanged -= _statusSelectedEventHandler;
	        _doneButton.Clicked -= _barButtonClickEventHandler;
	    }

	    public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            DisasterStatusText.Layer.BorderColor = Constants.HtBoxTan.CGColor;
            DisasterStatusText.BorderStyle = UITextBorderStyle.Line;
            DisasterStatusText.Layer.BorderWidth = 1f;
            DisasterStatusText.ClipsToBounds = true; // might have to do this per James M. and Stack Overflow
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