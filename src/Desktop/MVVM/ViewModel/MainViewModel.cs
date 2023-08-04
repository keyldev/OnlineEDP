using OnlineEDP.Core;
using OnlineEDP.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineEDP.MVVM.ViewModel
{
    internal class MainViewModel : ObservableObject
    {

        private ObservableCollection<NotificationCardViewModel> _userNotifications;

        public ObservableCollection<NotificationCardViewModel> UserNotifications
        {
            get { return _userNotifications; }
            set { _userNotifications = value; NotifyPropertyChanged(); }
        }
        private object _menuItemView;

        public object MenuItemView
        {
            get { return _menuItemView; }
            set { _menuItemView = value; NotifyPropertyChanged(); }
        }
        private string _notificationCountText = "4";

        public string NotificationCountText
        {
            get { return _notificationCountText; }
            set { _notificationCountText = value; NotifyPropertyChanged(); }
        }
        private string _userName = "Default Name";

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; NotifyPropertyChanged(); }
        }
        private string _userRole = "Default Role";

        public string UserRole
        {
            get { return _userRole; }
            set { _userRole = value; NotifyPropertyChanged(); }
        }
        private Visibility _adminButtonVisibility = Visibility.Collapsed;

        public Visibility AdminButtonVisibility
        {
            get { return _adminButtonVisibility; }
            set { _adminButtonVisibility = value; NotifyPropertyChanged(); }
        }


        public static MainViewModel Instance;
        public RelayCommand AllCoursesButton { get; set; }
        public RelayCommand MyCoursesButton { get; set; }
        public RelayCommand TeacherButton { get; set; }

        public MainViewModel()
        {
            Instance = this;
            // это всё дебаг, нужно переделать на запросы.. пипяу
            //
            UserNotifications = new ObservableCollection<NotificationCardViewModel>();
            MenuItemView = new WelcomeAccountViewModel();

            UserNotifications.Add(new NotificationCardViewModel());
            UserNotifications.Add(new NotificationCardViewModel());
            UserNotifications.Add(new NotificationCardViewModel());
            UserNotifications.Add(new NotificationCardViewModel());

            TeacherButton = new RelayCommand(o =>
            {
                MenuItemView = new Teacher.TeachersCourseViewModel();
            });

            AllCoursesButton = new RelayCommand(o =>
            {
                MenuItemView = new AllCoursesViewModel();
            });
            MyCoursesButton = new RelayCommand(o =>
            {
                MenuItemView = new AllCoursesViewModel(_userModel);
            });
        }


        private UserModel _userModel;
        private string[] UserRoles = { "Студент", "Преподаватель", "Разработчик" };
        public MainViewModel(UserModel user)
        {
            Instance = this;
            // это всё дебаг, нужно переделать на запросы.. пипяу
            //
            _userModel = user;
            if (_userModel.uRole == 1 || _userModel.uRole == 2)
                AdminButtonVisibility = Visibility.Visible;

            UserName = $"{_userModel.uName} {_userModel.uSurname}";
            UserRole = UserRoles[_userModel.uRole];
            //вынести всё это в отдельный инициализирующий метод
            UserNotifications = new ObservableCollection<NotificationCardViewModel>();
            MenuItemView = new WelcomeAccountViewModel() { UserWelcomeName = UserName};

            UserNotifications.Add(new NotificationCardViewModel());
            UserNotifications.Add(new NotificationCardViewModel());
            UserNotifications.Add(new NotificationCardViewModel());
            UserNotifications.Add(new NotificationCardViewModel());

            TeacherButton = new RelayCommand(o =>
            {
                MenuItemView = new Teacher.TeachersCourseViewModel(_userModel);
            });

            AllCoursesButton = new RelayCommand(o =>
            {
                MenuItemView = new AllCoursesViewModel();
            });
            MyCoursesButton = new RelayCommand(o =>
            {
                MenuItemView = new AllCoursesViewModel(user);
            });
        }
    }
}
