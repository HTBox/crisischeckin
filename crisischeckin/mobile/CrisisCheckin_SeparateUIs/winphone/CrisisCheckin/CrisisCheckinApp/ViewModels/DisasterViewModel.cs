using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Collections.Generic;
using CrisisCheckinApp.ServiceClient;

namespace CrisisCheckinApp.ViewModels
{
    public class DisasterViewModel : INotifyPropertyChanged
    {
        private Disaster disaster;

        public Disaster Disaster
        {
            get
            {
                return this.disaster;
            }
            set
            {
                this.disaster = value;
               NotifyPropertyChanged("Disaster");
            }
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