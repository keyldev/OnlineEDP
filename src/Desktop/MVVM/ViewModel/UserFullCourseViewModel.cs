using OnlineEDP.Core;
using OnlineEDP.MVVM.ViewModel.Course;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineEDP.MVVM.ViewModel
{
    internal class UserFullCourseViewModel : ObservableObject
    {
        private string _courseNameBlock;

        public string CourseNameBlock
        {
            get { return _courseNameBlock; }
            set { _courseNameBlock = value; NotifyPropertyChanged(); }
        }
        private string _enterLeaveText = "Присоединиться";

        public string EnterLeaveText
        {
            get { return _enterLeaveText; }
            set { _enterLeaveText = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<object> _courseElementsList;

        public ObservableCollection<object> CourseElementsList
        {
            get { return _courseElementsList; }
            set { _courseElementsList = value; NotifyPropertyChanged(); }
        }


        private ObservableCollection<UserCardViewModel> _studentsList;

        public ObservableCollection<UserCardViewModel> StudentsList
        {
            get { return _studentsList; }
            set { _studentsList = value; NotifyPropertyChanged(); }
        }
        private ObservableCollection<UserCardViewModel> _teacherList;

        public ObservableCollection<UserCardViewModel> TeachersList
        {
            get { return _teacherList; }
            set { _teacherList = value; NotifyPropertyChanged(); }
        }

        public RelayCommand ReturnToCoursesCommand { get; set; }
        public RelayCommand EnterLeaveCommand { get; set; }
        public UserFullCourseViewModel()
        {
            StudentsList = new ObservableCollection<UserCardViewModel>();
            StudentsList.Add(new UserCardViewModel()
            {
                UserName = "Sample Student",
                UserRole = "Студент",
                UserSkillsList = new ObservableCollection<UserSkill>()
                {
                    new UserSkill(){UserSkillName = "Python"}
                }
            });
            TeachersList = new ObservableCollection<UserCardViewModel>();
            TeachersList.Add(new UserCardViewModel()
            {
                UserName = "Test Name1",
                UserRole = "Преподаватель",
                UserSkillsList = new ObservableCollection<UserSkill>()
                {
                    new UserSkill(){ UserSkillName = "C++"},
                    new UserSkill(){ UserSkillName = "C#"},
                    new UserSkill(){ UserSkillName = "Web"},
                }
            });

            // попробовать
            CourseElementsList = new ObservableCollection<object>() {
                new AdditionalFileViewModel()
                {
                    FileName = "Worked?",
                    FileURL = "https://vk.com/",
                    HeaderText = "Hello"
                }, 
                new SimpleTextViewModel()
                {
                    HeaderBlockText = "Начало курса..",
                    DescriptionBlockText = "Добрый день уважаемые я",
                    IsReadOnly = true
                }
            };
            ReturnToCoursesCommand = new RelayCommand(o =>
            {
                MainViewModel.Instance.MenuItemView = new AllCoursesViewModel(); // и сюда можно чето передать
            });
            EnterLeaveCommand = new RelayCommand(o =>
            {
                // если ID == ID в юзере, мы ливаем, иначе - присоединяемся
            });
        }
        public UserFullCourseViewModel(ObservableCollection<object> ui)
        {
            StudentsList = new ObservableCollection<UserCardViewModel>();
            StudentsList.Add(new UserCardViewModel()
            {
                UserName = "Sample Student",
                UserRole = "Студент",
                UserSkillsList = new ObservableCollection<UserSkill>()
                {
                    new UserSkill(){UserSkillName = "Python"}
                }
            });
            TeachersList = new ObservableCollection<UserCardViewModel>();
            TeachersList.Add(new UserCardViewModel()
            {
                UserName = "Test Name1",
                UserRole = "Преподаватель",
                UserSkillsList = new ObservableCollection<UserSkill>()
                {
                    new UserSkill(){ UserSkillName = "C++"},
                    new UserSkill(){ UserSkillName = "C#"},
                    new UserSkill(){ UserSkillName = "Web"},
                }
            });

            // попробовать
            CourseElementsList = ui;
            ReturnToCoursesCommand = new RelayCommand(o =>
            {
                MainViewModel.Instance.MenuItemView = new AllCoursesViewModel(); // и сюда можно чето передать
            });
            EnterLeaveCommand = new RelayCommand(o =>
            {
                // если ID == ID в юзере, мы ливаем, иначе - присоединяемся
            });
        }
    }
}
