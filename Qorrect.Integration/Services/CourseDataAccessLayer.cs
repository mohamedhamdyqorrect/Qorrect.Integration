﻿using Qorrect.Integration.Helper;
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
                        PracticalHours = Convert.ToInt32(rdr["PracticalHours"]),
                        TotalMarks = Convert.ToDouble(rdr["TotalMarks"])
                    });
                }
                con.Close();
            }
            return lstCourse.ToList();
        }

        public async Task<List<DTOBedoUnites>> GetCourseLevels(int crsId)
        {
            List<DTOBedoUnites> crsLevels = new List<DTOBedoUnites>();
            int order = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("spGetLevelByCourseId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@crsId", crsId);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {

                        using (DataSet ds = new DataSet())
                        {

                            da.Fill(ds);
                            con.Close();

                            if (ds.Tables[1].Rows.Count > 1)
                            {
                                order++;
                                crsLevels = ds.Tables[0].AsEnumerable().Select(QdataRow => new DTOBedoUnites
                                {
                                    Id = QdataRow.Field<int>("ID"),
                                    Name = QdataRow.Field<string>("Name"),
                                    Code = QdataRow.Field<string>("Code"),
                                    Order = order,
                                    Lessons = ds.Tables[1].AsEnumerable().Select(AdataRow => new DTOBedoLessons
                                    {
                                        Id = AdataRow.Field<int>("ID"),
                                        Name = AdataRow.Field<string>("Name"),
                                        Code = AdataRow.Field<string>("Code"),
                                        Order = order,
                                        ParentId = AdataRow.Field<int>("ID_Ref")
                                    }).Where(a => a.ParentId == QdataRow.Field<int>("ID")).ToList()
                                }).ToList();

                            }
                        }

                    }
                }
            }
            return crsLevels.ToList();
        }

        public async Task<List<DTOBedoILO>> GetLevelIlo(int levelId)
        {
            List<DTOBedoILO> Ilos = new List<DTOBedoILO>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("spGetILOByLevelId", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@levelId", levelId);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Ilos.Add(new DTOBedoILO()
                        {
                            Name = rdr["Name"].ToString(),
                            Code = rdr["Code"].ToString()
                        });
                    }
                    con.Close();
                }
            }
            return Ilos.ToList();
        }
    }
}
