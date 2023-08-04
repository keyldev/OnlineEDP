using EduPlatformAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EduPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        SqlConnection _connection;
        IConfiguration _configuration;
        public CoursesController(IConfiguration configuration)
        {
            _configuration = configuration;

            _connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
        /// <summary>
        /// получение всех курсов без ID курса
        /// </summary>
        /// <returns>возвращает коллекцию из всех курсов</returns>
        // GET: api/<CoursesController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CoursesController>/5
        [HttpGet]
        [Route("get/all")]
        public ObservableCollection<CourseModel> All()
        {
            ObservableCollection<CourseModel> collection = new ObservableCollection<CourseModel>(); 
            _connection.Open();
            string sql = $"SELECT * FROM Courses";
            using(SqlCommand cmd = new SqlCommand(sql, _connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        collection.Add(new CourseModel()
                         {
                             CourseID = (int)reader["cID"],
                             CourseName = reader["cName"].ToString(),
                             CourseDescription = reader["cDescription"].ToString(),
                             CourseLikes = (int)reader["cLikesCount"],
                             CourseTags = reader["cTags"].ToString()
                         });
                    }
                }
            }
            _connection.Close();
            return collection;
        }
        [HttpGet]
        [Route("get/all/{text}")]
        public ObservableCollection<CourseModel> All(string text)
        {
            ObservableCollection<CourseModel> collection = new ObservableCollection<CourseModel>();
            _connection.Open();
            string sql = $"SELECT * FROM Courses WHERE (cName LIKE '%{text}%' OR cTags LIKE '%{text}%')";
            using (SqlCommand cmd = new SqlCommand(sql, _connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        collection.Add(new CourseModel()
                        {
                            CourseID = (int)reader["cID"],
                            CourseName = reader["cName"].ToString(),
                            CourseDescription = reader["cDescription"].ToString(),
                            CourseLikes = (int)reader["cLikesCount"],
                            CourseTags = reader["cTags"].ToString()
                        });
                    }
                }
            }
            _connection.Close();
            return collection;
        }
        // POST api/<CoursesController>
        [HttpPost]
        public void Post([FromBody] CourseModel course)
        {
            _connection.Open();
            //string sql = $"INSERT INTO Courses (cName, cDescription, cTags, cLikesCount) VALUES ({course.CourseName},{course.CourseDescription},{course.CourseTags},{course.CourseLikes})";
            string sql_update = $"UPDATE Courses SET cName='{course.CourseName}', cDescription='{course.CourseDescription}', cTags='{course.CourseTags}', cLikesCount={course.CourseLikes} WHERE cID={course.CourseID}";
            Debug.WriteLine(sql_update);
            using(SqlCommand insertToSQL = new SqlCommand(sql_update, _connection))
            {
                insertToSQL.ExecuteNonQuery();
            }


            _connection.Close();
        }

        // PUT api/<CoursesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CoursesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

}
