using Newtonsoft.Json;
using OnlineEDP.Core;
using OnlineEDP.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace OnlineEDP.MVVM.ViewModel
{
    internal class CourseCardViewModel : ObservableObject
    {

        public RelayCommand OpenCourseCommand { get; set; }
        public RelayCommand SetLikeCommand { get; set; }

        private bool _isReadOnlyBlocks = false;
        /// <summary>
        /// Определяет, можно ли редактировать заголовок или описание карточки
        /// </summary>
        public bool IsReadOnlyBlocks
        {
            get { return _isReadOnlyBlocks; }
            set { _isReadOnlyBlocks = value; NotifyPropertyChanged(); }
        }
        private int _cID;

        public int CourseID
        {
            get { return _cID; }
            set { _cID = value; NotifyPropertyChanged(); }
        }

        private string _courseNameBlock = "Измените меня, бажожда";

        public string CourseNameBlock
        {
            get { return _courseNameBlock; }
            set { _courseNameBlock = value; NotifyPropertyChanged(); }
        }
        private string _courseDescriptionBlock = "Это описание и бла бла бла бла";

        public string CourseDescriptionBlock
        {
            get { return _courseDescriptionBlock; }
            set { _courseDescriptionBlock = value; NotifyPropertyChanged(); }
        }
        private string _courseLikeColor = "#FFDED3D4";

        public string CourseLikeColor
        {
            get { return _courseLikeColor; }
            set { _courseLikeColor = value; NotifyPropertyChanged(); }
        }

        private string _courseLikesBlock = "3.4k";

        public string CourseLikesBlock
        {
            get { return _courseLikesBlock; }
            set { _courseLikesBlock = value; NotifyPropertyChanged(); }
        }
        private ObservableCollection<TagModel> _courseTagsBlock;

        public ObservableCollection<TagModel> CourseTagsBlock
        {
            get { return _courseTagsBlock; }
            set { _courseTagsBlock = value; NotifyPropertyChanged(); }
        }
        private string _courseImageSource = "/Assets/skripty.jpg";

        public string CourseImageSource
        {
            get { return _courseImageSource; }
            set { _courseImageSource = value; NotifyPropertyChanged(); }
        }
        private ObservableCollection<object> _testUI;

        public ObservableCollection<object> TestUI
        {
            get { return _testUI; }
            set { _testUI = value; NotifyPropertyChanged(); }
        }

        public CourseCardViewModel()
        {
            // удалить если не понадобится

            CourseTagsBlock = new ObservableCollection<TagModel>()
            {
                new TagModel()
                {
                    TagText = "UI/UX"
                },
                new TagModel()
                {
                    TagText = "C#/WPF"
                },
                new TagModel()
                {
                    TagText = "Web"
                }
            };
            TestUI = new ObservableCollection<object>(); // test dynamic ui generator 
            //MessageBox.Show();
            //если пользователь - учитель, тогда он может менять название
            //IsTeacherEdit = user.IsTeacher? false: true;
            OpenCourseCommand = new RelayCommand(o =>
            {
                if (!IsReadOnlyBlocks)
                {
                    // если userIsTeacher - open teacherview

                    MainViewModel.Instance.MenuItemView = new Teacher.EditFullCourseViewModel()
                    {
                        CourseID = CourseID,
                        CourseNameBlock = CourseNameBlock,
                        CourseDescriptionBlock = CourseDescriptionBlock,
                        CourseLikesCount = (CourseLikesBlock.Contains("k") ? Convert.ToInt32(CourseLikesBlock.Trim('k')) : Convert.ToInt32(CourseLikesBlock))
                    }; // сюда передавать инфу о курсе из БД
                }
                else
                {

                    MainViewModel.Instance.MenuItemView = new UserFullCourseViewModel(TestUI)
                    {

                        CourseNameBlock = CourseNameBlock,
                    };
                }
                //MainViewModel.Instance.MenuItemView = new UserFullCourseViewModel(); // сюда передавать инфу о курсе из БД
            });
            SetLikeCommand = new RelayCommand(async o =>
            {
                var httpClient = new HttpClient();
                var json = JsonConvert.SerializeObject(new UserLike()
                {
                    UserID = UserModel.User.uID,
                    CourseID = CourseID
                });
                var data = new StringContent(json, UTF8Encoding.UTF8, "application/json");
                var result = await httpClient.PostAsync($"https://localhost:7138/api/user/likes/", data);
                string message = await result.Content.ReadAsStringAsync();
                //MessageBox.Show(message);
                var user = JsonConvert.DeserializeObject<int>(message);
                if (user == 1)
                {
                    CourseLikeColor = "#f45b3c";
                    CourseLikesBlock = CourseLikesBlock.Contains("k") ? (Convert.ToInt32(CourseLikesBlock.Trim('k')) + 1) + "k" : (Convert.ToInt32(CourseLikesBlock) + 1).ToString();
                }
                else
                {
                    CourseLikeColor = "#FFDED3D4";
                    CourseLikesBlock = CourseLikesBlock.Contains("k") ? (Convert.ToInt32(CourseLikesBlock.Trim('k')) - 1) + "k" : (Convert.ToInt32(CourseLikesBlock) - 1).ToString();
                }

            });
        }
    }
    public class UserLike
    {
        public int UserID { get; set; }
        public int CourseID { get; set; }
    }
    class TagModel : ObservableObject
    {
        private string _tagText;

        public string TagText
        {
            get { return _tagText; }
            set { _tagText = value; NotifyPropertyChanged(); }
        }

    }
}
