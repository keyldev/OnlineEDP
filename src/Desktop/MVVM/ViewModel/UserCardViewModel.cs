using OnlineEDP.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEDP.MVVM.ViewModel
{
    internal class UserCardViewModel : ObservableObject
    {

        // пкм -> кастомное меню с информацией, а именно:
        // почта, логаут, информация о группе etc
        // компетенции, максимум 3 (C++, Web, C#)
        private ObservableCollection<UserSkill> userSkills;

        public ObservableCollection<UserSkill> UserSkillsList
        {
            get { return userSkills; }
            set { userSkills = value; NotifyPropertyChanged(); }
        }
        private string _userName;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; NotifyPropertyChanged(); }
        }
        private string _userRole;

        public string UserRole
        {
            get { return _userRole; }
            set { _userRole = value; NotifyPropertyChanged(); }
        }


        public UserCardViewModel()
        {

        }
    }
    class UserSkill : ObservableObject
    {
        private string _userSkill;

        public string UserSkillName
        {
            get { return _userSkill; }
            set { _userSkill = value; NotifyPropertyChanged(); }
        }

    }
}
