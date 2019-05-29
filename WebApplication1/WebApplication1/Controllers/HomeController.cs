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

        #region Peticiones GET

        [HttpGet]
        public IActionResult Index()
        {
            List<Teacher> teachersList = new List<Teacher>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetAllTeacher", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                connection.Open();

                SqlDataReader dataReader = cmd.ExecuteReader();

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
                SqlCommand cmd = new SqlCommand("spGetTeacher", connection);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                connection.Open();

                SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    teacher.Id = Convert.ToInt32(dataReader["Id"]);
                    teacher.Name = Convert.ToString(dataReader["Name"]);
                    teacher.Skills = Convert.ToString(dataReader["Skills"]);
                    teacher.TotalStudents = Convert.ToInt32(dataReader["TotalStudents"]);
                    teacher.Salary = Convert.ToDecimal(dataReader["Salary"]);
                    teacher.AddedOn = Convert.ToDateTime(dataReader["AddedOn"]);
                }

                connection.Close();
            }

            return View(teacher);
        }

        #endregion

        #region Peticiones POST

        [HttpPost]
        [ActionName("Create")]
        public IActionResult Create(Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("spAddTeacher", connection);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Name", teacher.Name);
                    cmd.Parameters.AddWithValue("@Skills", teacher.Skills);
                    cmd.Parameters.AddWithValue("@TotalStudents", teacher.TotalStudents);
                    cmd.Parameters.AddWithValue("@Salary", teacher.Salary);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                    connection.Close();

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
                SqlCommand cmd = new SqlCommand("spUpdateTeacher", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", teacher.Id);
                cmd.Parameters.AddWithValue("@Name", teacher.Name);
                cmd.Parameters.AddWithValue("@Skills", teacher.Skills);
                cmd.Parameters.AddWithValue("@TotalStudents", teacher.TotalStudents);
                cmd.Parameters.AddWithValue("@Salary", teacher.Salary);
                cmd.Parameters.AddWithValue("@AddedOn", teacher.AddedOn);

                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();
            }

            return RedirectToAction("Index");
        }

        [ActionName("Delete")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spDeleteTeacher", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);

                connection.Open();

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    ViewBag.Result = "Operation got Error:" + ex.Message;
                }
                
                connection.Close();
            }

            return RedirectToAction("Index");
        }

        #endregion

    }
}