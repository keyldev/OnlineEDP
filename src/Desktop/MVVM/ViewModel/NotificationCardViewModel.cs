using OnlineEDP.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEDP.MVVM.ViewModel
{
    internal class NotificationCardViewModel : ObservableObject
    {
        private string _notifyText = "Your work 5..";

        public string NotifyText
        {
            get { return _notifyText; }
            set { _notifyText = value; NotifyPropertyChanged(); }
        }
        private string _notifyDate = DateTime.Now.Date.ToString();

        public string NotifyDate
        {
            get { return _notifyDate; }
            set { _notifyDate = value; NotifyPropertyChanged(); }
        }

    }
}
