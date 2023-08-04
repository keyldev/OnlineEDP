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

namespace OnlineEDP.MVVM.ViewModel
{
    internal class AllCoursesViewModel : ObservableObject
    {
        private ObservableCollection<CourseCardViewModel> _coursesList;

        public ObservableCollection<CourseCardViewModel> CoursesList
        {
            get { return _coursesList; }
            set { _coursesList = value; NotifyPropertyChanged(); }
        }

        private string _textToFind;

        public string TextToFind
        {
            get { return _textToFind; }
            set { _textToFind = value; NotifyPropertyChanged(); }
        }
        public RelayCommand FindButtonCommand { get; set; }
        public AllCoursesViewModel()
        {
            CoursesList = new ObservableCollection<CourseCardViewModel>();

            getAllCourses();
            FindButtonCommand = new RelayCommand(async o =>
            {
                await getAllCourses(TextToFind);
            });
        }
        private async Task getAllCourses(string text = null)
        {
            var httpClient = new HttpClient();
            //likes/
            dynamic result = null;
            if (text == String.Empty || text == null)
                result = await httpClient.GetAsync($"https://localhost:7138/api/courses/get/all/"); // сюда + user.uID
            else
                result = await httpClient.GetAsync($"https://localhost:7138/api/courses/get/all/" + text); // сюда + user.uID

            string message = await result.Content.ReadAsStringAsync();
            //MessageBox.Show(message);
            var courses = JsonConvert.DeserializeObject<ObservableCollection<CourseUIModel>>(message);
            var httpClient2 = new HttpClient();
            var result2 = await httpClient2.GetAsync($"https://localhost:7138/api/user/likes/" + UserModel.User.uID);
            string message2 = await result2.Content.ReadAsStringAsync();
            var likedIDs = JsonConvert.DeserializeObject<List<int>>(message2);
            if (CoursesList.Count > 0)
                CoursesList.Clear();
            if (CoursesList.Count > 0)
                CoursesList.Clear();
            //MessageBox.Show($"{likedIDs[0]} : {likedIDs[1]}");
            for (int i = 0; i < courses.Count; i++)
            {
                CourseCardViewModel course = new CourseCardViewModel();
                course.CourseID = i;
                course.IsReadOnlyBlocks = true;
                course.CourseNameBlock = courses[i].CourseName;
                course.CourseDescriptionBlock = courses[i].CourseDescription;
                course.CourseLikesBlock = courses[i].CourseLikes > 1000 ? (courses[i].CourseLikes / 1000) + "k" : courses[i].CourseLikes.ToString();
                course.CourseTagsBlock = new ObservableCollection<TagModel>();

                if (likedIDs.Contains(course.CourseID))
                    course.CourseLikeColor = "#f45b3c";

                foreach (var tag in courses[i].CourseTags.Split(','))
                {
                    course.CourseTagsBlock.Add(new TagModel() { TagText = tag });
                }
                CoursesList.Add(course);
            }
        }
        private UserModel _userModel;
        public AllCoursesViewModel(UserModel user)
        {
            _userModel = user;
            CoursesList = new ObservableCollection<CourseCardViewModel>();
            try
            {
                getUserCourses();
            }
            catch
            {
                MessageBox.Show("exsdas");
            }
            FindButtonCommand = new RelayCommand(async o =>
            {
                await getUserCourses(TextToFind);
            });

            //CoursesList.Add(new CourseCardViewModel() { IsReadOnlyBlocks = true });
            //CoursesList.Add(new CourseCardViewModel() { IsReadOnlyBlocks = true });
        }
        private async Task<ObservableCollection<CourseUIModel>> getUserCourses(string text = null)
        {

            var httpClient = new HttpClient();
            //likes/
            dynamic result = null;
            if (text == String.Empty || text == null)
                result = await httpClient.GetAsync($"https://localhost:7138/api/user/courses/" + _userModel.uID); // сюда + user.uID
            else
            {
                var json = JsonConvert.SerializeObject(new UserCourse()
                {
                    UserID = _userModel.uID,
                    FindText = text
                });
                var data = new StringContent(json, UTF8Encoding.UTF8, "application/json");
                result = await httpClient.PostAsync($"https://localhost:7138/api/user/courses/find/", data); // сюда + user.uID
            }
            string message = await result.Content.ReadAsStringAsync();
            //MessageBox.Show(message);
            var courses = JsonConvert.DeserializeObject<ObservableCollection<CourseUIModel>>(message);
            //вот это убрать
            var httpClient2 = new HttpClient();
            var result2 = await httpClient2.GetAsync($"https://localhost:7138/api/user/likes/" + _userModel.uID);
            string message2 = await result2.Content.ReadAsStringAsync();
            var likedIDs = JsonConvert.DeserializeObject<List<int>>(message2);
            if (CoursesList.Count > 0)
                CoursesList.Clear();
            //MessageBox.Show($"{likedIDs[0]} : {likedIDs[1]}");
            for (int i = 0; i < courses.Count; i++)
            {
                CourseCardViewModel course = new CourseCardViewModel();
                course.CourseID = i;
                course.IsReadOnlyBlocks = true;
                course.CourseNameBlock = courses[i].CourseName;
                course.CourseDescriptionBlock = courses[i].CourseDescription;
                course.CourseLikesBlock = courses[i].CourseLikes > 1000 ? (courses[i].CourseLikes / 1000) + "k" : courses[i].CourseLikes.ToString();
                course.CourseTagsBlock = new ObservableCollection<TagModel>();
                if (likedIDs.Contains(course.CourseID))
                    course.CourseLikeColor = "#f45b3c";

                foreach (var tag in courses[i].CourseTags.Split(','))
                {
                    course.CourseTagsBlock.Add(new TagModel() { TagText = tag });
                }
                CoursesList.Add(course);
            }

            return courses;
        }
    }
    public class UserCourse
    {
        public int UserID { get; set; }
        public string FindText { get; set; }
    }
}
