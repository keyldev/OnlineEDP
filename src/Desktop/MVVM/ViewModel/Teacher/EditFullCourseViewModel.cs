using Newtonsoft.Json;
using OnlineEDP.Core;
using OnlineEDP.MVVM.Model;
using OnlineEDP.MVVM.ViewModel.Course;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineEDP.MVVM.ViewModel.Teacher
{
    internal class EditFullCourseViewModel : ObservableObject
    {
        private ObservableCollection<object> _courseElementsList;

        public ObservableCollection<object> CourseElementsList
        {
            get { return _courseElementsList; }
            set { _courseElementsList = value; NotifyPropertyChanged(); }
        }
        private ObservableCollection<string> _controlsComboBox = new ObservableCollection<string>()
        {
            "Подраздел",
            "Заголовок&описание",
            "Файл(название/ссылка)"
        };
        private int _courseID;

        public int CourseID
        {
            get { return _courseID; }
            set { _courseID = value; NotifyPropertyChanged(); }
        }

        public ObservableCollection<string> ControlsComboBox
        {
            get { return _controlsComboBox; }
            set { _controlsComboBox = value; NotifyPropertyChanged(); }
        }
        private int _selectedControlIndex;
        //
        public int SelectedControlIndex
        {
            get { return _selectedControlIndex; }
            set { _selectedControlIndex = value; NotifyPropertyChanged(); }
        }
        private string _courseTags = "C++, C#, UI/UX";
        public string CourseTags
        {
            get { return _courseTags; }
            set { _courseTags = value; NotifyPropertyChanged(); }
        }
        public RelayCommand AddControlCommand { get; set; }

        private string _courseNameBlock;

        public string CourseNameBlock
        {
            get { return _courseNameBlock; }
            set { _courseNameBlock = value; NotifyPropertyChanged(); }
        }
        private string _courseDescriptionBlock;

        public string CourseDescriptionBlock
        {
            get { return _courseDescriptionBlock; }
            set { _courseDescriptionBlock = value; NotifyPropertyChanged(); }
        }
        private ObservableCollection<string> _teachersList = new ObservableCollection<string>()
        {
            "ID 0: Vasiliy ",
            "ID 1: Vasich ",
            "ID 2: Vasilevs ",
            "ID 3: Vasvvils ",
        };

        public ObservableCollection<string> TeachersList
        {
            get { return _teachersList; }
            set { _teachersList = value; NotifyPropertyChanged(); }
        }
        private int _selectedTeacherIndex;

        public int SelectedTeacherIndex
        {
            get { return _selectedTeacherIndex; }
            set
            {
                _selectedTeacherIndex = value; NotifyPropertyChanged(); SelectedTeachersList.Add(new UserCardViewModel()
                {
                    UserName = TeachersList[value],
                    UserRole = "Преподаватель",
                    UserSkillsList = new ObservableCollection<UserSkill>() {
                        new UserSkill() {
                            UserSkillName = "WPF"
                        },
                        new UserSkill() {
                            UserSkillName = "C#"
                        }
                    }
                });
            }
        }
        private ObservableCollection<UserCardViewModel> _selectedTeachersList;

        public ObservableCollection<UserCardViewModel> SelectedTeachersList
        {
            get { return _selectedTeachersList; }
            set { _selectedTeachersList = value; NotifyPropertyChanged(); }
        }
        private int _courseLikesCount;

        public int CourseLikesCount
        {
            get { return _courseLikesCount; }
            set { _courseLikesCount = value; NotifyPropertyChanged(); }
        }


        public RelayCommand ReturnToCoursesCommand { get; set; }
        public RelayCommand SaveCourseCommand { get; set; }

        CourseUIModel courseUI;
        UserModel _teacher;
        public EditFullCourseViewModel()
        {
            courseUI = new CourseUIModel();

            //test
            SelectedTeachersList = new ObservableCollection<UserCardViewModel>();

            CourseElementsList = new ObservableCollection<object>();
            ReturnToCoursesCommand = new RelayCommand(o =>
            {
                MainViewModel.Instance.MenuItemView = new TeachersCourseViewModel(UserModel.User); // и сюда можно чето передать
            });
            SaveCourseCommand = new RelayCommand(o =>
            {
                //вывести мессадж с инфой по курсу, типа: название, описаине в виде json
                //MessageBox.Show(CourseNameBlock);
                courseUI.CourseID = CourseID;
                courseUI.CourseName = CourseNameBlock;
                courseUI.CourseDescription = CourseDescriptionBlock;
                courseUI.CourseLikes = CourseLikesCount;

                courseUI.CourseTeachersID = new List<int>() { 1, 2, 3 };// new для теста, нужно сначала получить с БД
                courseUI.CourseTags = CourseTags;
                courseUI.UIBlocks = new Dictionary<string, object>();// new для теста, нужно сначала получить с БД
                for (int i = 0; i < CourseElementsList.Count; i++)
                {
                    if (CourseElementsList[i] is SectionElementViewModel) courseUI.UIBlocks.Add("SectionElementViewModel_" + i, new SectionViewClass() { Title = "Hello" });
                    else if (CourseElementsList[i] is SimpleTextViewModel) courseUI.UIBlocks.Add("SimpleTextViewModel_" + i, new HeaderWithDescriptionClass() { Header = "Hello", Description = "World" });
                }

                string json = JsonConvert.SerializeObject(courseUI);
                updateCourseInfo(courseUI);

                using (StreamWriter sw = new StreamWriter(Environment.CurrentDirectory + "/request.json"))
                {
                    sw.Write(json);
                    sw.Close();

                }
                MessageBox.Show(json);

            });
            AddControlCommand = new RelayCommand(o =>
            {
                // создаем CourseUIModel и при нажатии сохранить вбиваем инфу и отсюда и с описания и т.п.
                switch (SelectedControlIndex)
                {
                    case 0: // подраздел
                        {
                            CourseElementsList.Add(new SectionElementViewModel() { IsReadOnly = false });
                            break;
                        }
                    case 1: // заголовок и описание
                        {
                            CourseElementsList.Add(new SimpleTextViewModel() { IsReadOnly = false });
                            break;
                        }
                    case 2: // файл
                        {
                            CourseElementsList.Add(new AdditionalFileViewModel() { IsReadOnly = false });
                            break;
                        }
                    default: // Заголовок и описание
                        {
                            CourseElementsList.Add(new SimpleTextViewModel() { IsReadOnly = false });
                            break;
                        }
                }
            });
        }
        private async Task updateCourseInfo(CourseUIModel course)
        {
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(course);
            var data = new StringContent(json, UTF8Encoding.UTF8, "application/json");
            var result = await httpClient.PostAsync($"https://localhost:7138/api/courses/", data); // сделать стринги в отдельном файле
        }

    }
}
