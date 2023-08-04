using OnlineEDP.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEDP.MVVM.ViewModel
{
    internal class MainWindowViewModel : ObservableObject
    {
        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; NotifyPropertyChanged(); }
        }


        public static MainWindowViewModel Instance;
        public MainWindowViewModel()
        {
            Instance = this;
            CurrentView = new LoginViewModel();
        }
    }
}
