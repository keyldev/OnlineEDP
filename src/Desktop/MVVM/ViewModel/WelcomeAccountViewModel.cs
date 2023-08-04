using OnlineEDP.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEDP.MVVM.ViewModel
{
    internal class WelcomeAccountViewModel : ObservableObject
    {
        private string _userWelcomeName;

        public string UserWelcomeName
        {
            get { return _userWelcomeName; }
            set { _userWelcomeName = value; NotifyPropertyChanged(); }
        }



        public WelcomeAccountViewModel()
        {

        }
    }
}
