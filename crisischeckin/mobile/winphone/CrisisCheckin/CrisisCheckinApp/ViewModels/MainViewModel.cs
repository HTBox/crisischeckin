using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using CrisisCheckinApp.Resources;
using CrisisCheckinApp.ServiceClient;

namespace CrisisCheckinApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private DisasterServiceClient client;

        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
            this.OngoingDisasters = new ObservableCollection<DisasterViewModel>();
            this.client = new DisasterServiceClient();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<DisasterViewModel> OngoingDisasters { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        /// <summary>
        /// Sample property that returns a localized string
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public void LoadData()
        {
            // Sample data; replace with real data
            var ongoingDisasters = this.client.GetDisasters(new GetDisastersRequest()).Disasters;
            foreach (var od in ongoingDisasters)
            {
                this.OngoingDisasters.Add(new DisasterViewModel { Disaster = od });
            }
          
            this.IsDataLoaded = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}