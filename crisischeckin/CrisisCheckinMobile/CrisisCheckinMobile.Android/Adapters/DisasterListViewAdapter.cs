using Android.Widget;
using System.Collections.Generic;

namespace CrisisCheckinMobile.Droid
{
    public class DisasterListViewAdapter : BaseAdapter<string>
    {
        List<string> _items;

        public override long GetItemId(int position)
        {
            throw new System.NotImplementedException();
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            throw new System.NotImplementedException();
        }

        public override int Count
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public override string this[int index]
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }
    }
}

