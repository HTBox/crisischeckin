using System;
using UIKit;

namespace CrisisCheckinMobile.iOS
{
    public class PickerModel : UIPickerViewModel
    {
        private readonly CommitmentStatus[] _values;

        public event EventHandler<PickerChangedEventArgs> PickerChanged;

        public PickerModel(CommitmentStatus[] values)
        {
            _values = values;
        }

        public override nint GetRowsInComponent(UIPickerView pickerView, nint component)
        {
            return _values.Length;
        }

        public override string GetTitle(UIPickerView pickerView, nint row, nint component)
        {
            var status = _values[row];
            var name = Enum.GetName(typeof(CommitmentStatus), status);
            return name;
        }

        public override nint GetComponentCount(UIPickerView pickerView)
        {
            return 1;
        }

        public override void Selected(UIPickerView pickerView, nint row, nint component)
        {
            if (PickerChanged == null) 
                return;

            PickerChanged(this, new PickerChangedEventArgs
            {
                SelectedValue = _values[row]
            });
        }
    }

    public class PickerChangedEventArgs : EventArgs
    {
        public CommitmentStatus SelectedValue { get; set; }
    }
}