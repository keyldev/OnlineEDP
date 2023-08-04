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

namespace OnlineEDP.MVVM.ViewModel.Teacher
{
    internal class TeachersCourseViewModel : ObservableObject
    {
        private ObservableCollection<CourseCardViewModel> _coursesList;

        public ObservableCollection<CourseCardViewModel> CoursesList
        {
            get { return _coursesList; }
            set { _coursesList = value; NotifyPropertyChanged(); }
        }

        public TeachersCourseViewModel()
        {
            //за преподом из бд закреплен определенный ID курса, и поэтому он может его редактировать и/ли заполнять.
            // при тыке на курс от имени препода, нужно загружать с БД новые элементы.
            // парсер ответа.
            CoursesList = new ObservableCollection<CourseCardViewModel>()
            {
                new CourseCardViewModel(){IsReadOnlyBlocks = false},
                new CourseCardViewModel(){IsReadOnlyBlocks = false},
                new CourseCardViewModel(){IsReadOnlyBlocks = false},

            };
        }
        UserModel _teacher;
        public TeachersCourseViewModel(UserModel teacher)
        {
            //за преподом из бд закреплен определенный ID курса, и поэтому он может его редактировать и/ли заполнять.
            // при тыке на курс от имени препода, нужно загружать с БД новые элементы.
            // парсер ответа.
            this._teacher = teacher;
            CoursesList = new ObservableCollection<CourseCardViewModel>();
            getUserCourses();

        }
        private async Task<ObservableCollection<CourseUIModel>> getUserCourses()
        {
            var httpClient = new HttpClient();

            var result = await httpClient.GetAsync($"https://localhost:7138/api/user/courses/" + _teacher.uID); // сюда + user.uID
            string message = await result.Content.ReadAsStringAsync();
            //MessageBox.Show(message);
            var courses = JsonConvert.DeserializeObject<ObservableCollection<CourseUIModel>>(message);
            for (int i = 0; i < courses.Count; i++)
            {
                CourseCardViewModel course = new CourseCardViewModel();
                course.CourseID = i;
                course.IsReadOnlyBlocks = false;
                course.CourseNameBlock = courses[i].CourseName;
                course.CourseDescriptionBlock = courses[i].CourseDescription;
                course.CourseLikesBlock = courses[i].CourseLikes > 1000 ? (courses[i].CourseLikes / 1000) + "k" : courses[i].CourseLikes.ToString();
                course.CourseTagsBlock = new ObservableCollection<TagModel>();
                foreach (var tag in courses[i].CourseTags.Split(','))
                {
                    course.CourseTagsBlock.Add(new TagModel() { TagText = tag });
                }
                CoursesList.Add(course);
            }
            return courses;
        }
    }
}
