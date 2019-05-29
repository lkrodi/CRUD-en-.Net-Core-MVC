using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CRUDADO.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CRUDADO.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; }

        public HomeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<Teacher> teachersList = new List<Teacher>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM dbo.Teacher";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        Teacher teacher = new Teacher();
                        teacher.Id = Convert.ToInt32(dataReader["Id"]);
                        teacher.Name = Convert.ToString(dataReader["Name"]);
                        teacher.Skills = Convert.ToString(dataReader["Skills"]);
                        teacher.TotalStudents = Convert.ToInt32(dataReader["TotalStudents"]);
                        teacher.Salary = Convert.ToDecimal(dataReader["Salary"]);
                        teacher.AddedOn = Convert.ToDateTime(dataReader["AddedOn"]);

                        teachersList.Add(teacher);
                    }
                }
                connection.Close();
            }

            return View(teachersList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            Teacher teacher = new Teacher();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT * FROM Teacher WHERE Id='{id}'";
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        teacher.Id = Convert.ToInt32(dataReader["Id"]);
                        teacher.Name = Convert.ToString(dataReader["Name"]);
                        teacher.Skills = Convert.ToString(dataReader["Skills"]);
                        teacher.TotalStudents = Convert.ToInt32(dataReader["TotalStudents"]);
                        teacher.Salary = Convert.ToDecimal(dataReader["Salary"]);
                        teacher.AddedOn = Convert.ToDateTime(dataReader["AddedOn"]);
                    }
                }
                connection.Close();
            }

            return View(teacher);
        }

        [HttpPost]
        [ActionName("Create")]
        public IActionResult Create(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = $"Insert Into Teacher(Name, Skills, TotalStudents, Salary) Values('{teacher.Name}', '{teacher.Skills}', '{teacher.TotalStudents}', '{teacher.Salary}')";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.Text;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ActionName("Update")]
        public IActionResult Update(Teacher teacher)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Teacher SET Name='{teacher.Name}', Skills='{teacher.Skills}', TotalStudents='{teacher.TotalStudents}', Salary='{teacher.Salary}' WHERE Id='{teacher.Id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }

            return RedirectToAction("Index");
        }

        [ActionName("Update")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"DELETE FROM Teacher WHERE Id='{id}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    connection.Open();

                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (SqlException ex)
                    {
                        ViewBag.Result = "Operation got Error:" + ex.Message;
                    }

                    connection.Close();
                }
            }

            return RedirectToAction("Index");
        }
    }
}