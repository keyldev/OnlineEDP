using EduPlatformAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EduPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        SqlConnection _connection;
        IConfiguration _configuration;
        
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
       
        // GET: api/<UserController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }
        [HttpGet("{id}")]
        [Route("likes/{id}")]
        public List<int> likes(int id)
        {
            List<int> coursesIDs = new List<int>();
            _connection.Open();
            string sql = $"SELECT * FROM UserCoursesLikes WHERE UserID={id}";
            using (SqlCommand getLikes = new SqlCommand(sql, _connection))
            {
                using (SqlDataReader reader = getLikes.ExecuteReader())
                {
                    while (reader.Read())
                        coursesIDs.Add((int)reader["CourseID"]);
                }
            }
            _connection.Close();
            return coursesIDs;

        }
        /// <summary>
        /// Получение всех курсов у коткрентог опользоватлея
        /// </summary>
        /// <param name="id">ID - это уникальный идентификатор пользователя</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Route("courses/{id}")]

        public ObservableCollection<CourseModel> courses(int id)
        {
            ObservableCollection<CourseModel> courses = new ObservableCollection<CourseModel>();
            List<int> coursesID = new List<int>();
            _connection.Open();
            string sqlQuery = $"SELECT * FROM UsersCourses WHERE UserID={id}";
            using (SqlCommand getCourses = new SqlCommand(sqlQuery, _connection))
            {
                using (SqlDataReader reader = getCourses.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        coursesID.Add((int)reader["CourseID"]);
                    }
                }
            }
            // сюда пихануть форыч
            for (int i = 0; i < coursesID.Count; i++)
            {
                sqlQuery = $"SELECT * FROM Courses WHERE cID = {coursesID[i]}";
                using (SqlCommand getCoursesInfo = new SqlCommand(sqlQuery, _connection))
                {
                    using (SqlDataReader reader = getCoursesInfo.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            courses.Add(new CourseModel()
                            {
                                CourseID = (int)reader["cID"],
                                CourseName = reader["cName"].ToString(),
                                CourseDescription = reader["cDescription"].ToString(),
                                CourseLikes = (int)reader["cLikesCount"],
                                CourseTags = reader["cTags"].ToString()
                            });

                            Debug.WriteLine(reader["cID"].ToString());
                        }
                    }
                }
            }
            _connection.Close();
            return courses;
        }
        [HttpPost]
        [Route("courses/find")]
        public ObservableCollection<CourseModel> find([FromBody] UserCourse user)
        {
            ObservableCollection<CourseModel> result = new ObservableCollection<CourseModel>();
            List<int> coursesID = new List<int>();

            _connection.Open();
            string sqlQuery = $"SELECT * FROM UsersCourses WHERE UserID={user.UserID}";
            using (SqlCommand getCourses = new SqlCommand(sqlQuery, _connection))
            {
                using (SqlDataReader reader = getCourses.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        coursesID.Add((int)reader["CourseID"]);
                    }
                }
            }
            // сюда пихануть форыч
            for (int i = 0; i < coursesID.Count; i++)
            {
                sqlQuery = $"SELECT * FROM Courses WHERE cID = {coursesID[i]} AND (cName LIKE '%{user.FindText}%' OR cTags LIKE '%{user.FindText}%')";
                using (SqlCommand getCoursesInfo = new SqlCommand(sqlQuery, _connection))
                {
                    using (SqlDataReader reader = getCoursesInfo.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new CourseModel()
                            {
                                CourseID = (int)reader["cID"],
                                CourseName = reader["cName"].ToString(),
                                CourseDescription = reader["cDescription"].ToString(),
                                CourseLikes = (int)reader["cLikesCount"],
                                CourseTags = reader["cTags"].ToString()
                            });

                            Debug.WriteLine(reader["cID"].ToString());
                        }
                    }
                }
            }
            _connection.Close();
            return result;
        }

        [HttpPost]
        [Route("login/")]
        public UserModel login([FromBody] Model user)
        {
            UserModel result = new UserModel();
            _connection.Open();
            Debug.WriteLine(user.ToString());
            string sqlQuery = $"SELECT * FROM Users WHERE uLogin='{user.uLogin}' AND uPassword='{user.uPassword}'";
            using (SqlCommand getUser = new SqlCommand(sqlQuery, _connection))
            {
                using (SqlDataReader reader = getUser.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.uID = Convert.ToInt32(reader["uID"]);
                        result.uLogin = reader["uLogin"].ToString();
                        result.uEmail = reader["uEmail"].ToString();
                        result.uName = reader["uName"].ToString();
                        result.uSurname = reader["uSurname"].ToString();
                        result.uRole = Convert.ToInt32(reader["uRole"]);
                        result.uGroup = reader["uGroup"].ToString();
                        result.uSkills = new string[] { "C++", "C#", "JS" };
                    }
                }
            }
            _connection.Close();
            return result;

        }
        [HttpPost]
        [Route("likes/")]
        public int likes([FromBody] UserLike user)
        {
            _connection.Open();
            string sql = $"INSERT INTO UserCoursesLikes (CourseID, UserID) VALUES ({user.CourseID}, {user.UserID})";

            string new_sql = $"IF NOT EXISTS (SELECT * FROM UserCoursesLikes WHERE CourseID={user.CourseID} AND UserID={user.UserID}) " +
                $"BEGIN INSERT INTO UserCoursesLikes (CourseID, UserID) VALUES ({user.CourseID}, {user.UserID}) END " +
                $"ELSE BEGIN DELETE FROM UserCoursesLikes WHERE CourseID={user.CourseID} AND UserID={user.UserID} END";

            //string sql_query = $"IF NOT EXISTS (SELECT * FROM Planes WHERE planeID={plane.planeID}) " +
            //    $"BEGIN INSERT INTO Planes (planeType, planePlaces, planeSpeed) VALUES ('{plane.planeType}', {plane.planePlaces}, 400) END " +
            //    $"ELSE BEGIN UPDATE Planes SET planeType='{plane.planeType}', planePlaces={plane.planePlaces}, planeSpeed=500 WHERE planeID=@planeID END";

            Debug.WriteLine(sql);
            using (SqlCommand get = new SqlCommand(new_sql, _connection))
            {

                get.ExecuteNonQuery();
            }
            new_sql = $"SELECT * FROM UserCoursesLikes WHERE CourseID={user.CourseID} AND UserID={user.UserID}";
            using (SqlCommand get = new SqlCommand(new_sql, _connection))
            {
                using (SqlDataReader reader = get.ExecuteReader())
                {
                    if (reader.HasRows)
                        return 1;
                    else return 0;
                }
            }
            _connection.Close();
            return 0;
        }

        // POST api/<UserController>
        [HttpPost]
        public UserModel Post([FromBody] Model user)
        {
            UserModel result = new UserModel();
            _connection.Open();
            string sqlQuery = $"SELECT * FROM Users WHERE uLogin='{user.uLogin}' AND uPassword='{user.uPassword}'";
            using (SqlCommand getUser = new SqlCommand(sqlQuery, _connection))
            {
                using (SqlDataReader reader = getUser.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        result.uID = Convert.ToInt32(reader["uID"]);
                        result.uLogin = reader["uLogin"].ToString();
                        result.uEmail = reader["uEmail"].ToString();
                        result.uName = reader["uName"].ToString();
                        result.uSurname = reader["uSurname"].ToString();
                        result.uRole = Convert.ToInt32(reader["uRole"]);
                        result.uGroup = reader["uGroup"].ToString();
                        result.uSkills = new string[] { "C++", "C#", "JS" };

                    }
                }
            }
            return result;
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
    public class UserLike
    {
        public int UserID { get; set; }
        public int CourseID { get; set; }
    }
    public class Model
    {
        public string uLogin { get; set; }
        public string uPassword { get; set; }
    }
    public class UserCourse
    {
        public int UserID { get; set; }
        public string FindText { get; set; }
    }
}
