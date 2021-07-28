using Qorrect.Integration.Helper;
using Qorrect.Integration.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Qorrect.Integration.Services
{
    public class CourseDataAccessLayer
    {
        string connectionString = ConnectionString.CName;

        public async Task<List<DTOBedoCourse>> GetAllCourses()
        {
            List<DTOBedoCourse> lstCourse = new List<DTOBedoCourse>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetAllCourses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    lstCourse.Add(new DTOBedoCourse()
                    {
                        Id = Convert.ToInt32(rdr["Id"]),
                        CourseName = rdr["CourseName"].ToString(),
                        CourseCode = rdr["CourseCode"].ToString(),
                        Description = rdr["Description"].ToString(),
                        ClassesHours = Convert.ToInt32(rdr["ClassesHours"]),
                        CreditHours = Convert.ToInt32(rdr["CreditHours"]),
                        LectureHours = Convert.ToInt32(rdr["LectureHours"]),
                        LevelID = Convert.ToInt32(rdr["LevelID"]),
                        PracticalHours = Convert.ToInt32(rdr["PracticalHours"]),
                    });
                }
                con.Close();
            }
            return lstCourse.ToList();
        }
    }
}
