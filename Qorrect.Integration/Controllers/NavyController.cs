using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Qorrect.Integration.Helper;
using Qorrect.Integration.Models;
using Qorrect.Integration.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qorrect.Integration.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class NavyController : ControllerBase
    {
        CourseDataAccessLayer courseDataAccessLayer = null;

        public NavyController()
        {
            courseDataAccessLayer = new CourseDataAccessLayer();
        }


        [HttpGet, Route("ImportCourseStandardFromAPI/{id}")] // id = "D5FCB9F0-3131-4688-BBBE-6A719B54D25B"
        public async Task<IActionResult> ImportCourseStandardFromAPI([FromRoute] string id)
        {
            string token = Request.Headers["Authorization"];

            #region Get Course From API

            List<DTOCourses> courseResult = new List<DTOCourses>();

            {
                var client = new RestClient("http://ahmedhafez-001-site1.ctempurl.com/webservice/rest/server.php");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);

                request.AddQueryParameter("wstoken", "5b0eb98fc755581a73bdfb7bcb3842ee");
                request.AddQueryParameter("wsfunction", "core_course_get_courses_by_field");
                request.AddQueryParameter("field", "category");
                request.AddQueryParameter("value", "1");
                request.AddQueryParameter("moodlewsrestformat", "json");

                IRestResponse response = client.Execute(request);
                courseResult = JsonConvert.DeserializeObject<DTOExternalApiCourse>(response.Content).courses.ToList();
            }
            #endregion


            #region Call Qorrect API

            List<DTOAddEditCourse> addedCoursed = new List<DTOAddEditCourse>();

            {
                var client = new RestClient("http://localhost:5001/courses");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", $"{token}");
                request.AddHeader("Content-Type", "application/json");

                foreach (var item in courseResult)
                {
                    DTOAddEditCourse model = new DTOAddEditCourse()
                    {
                        Name = item.fullname,
                        Code = item.shortname,
                        CourseSubscriptionId = new Guid(id),
                        CourseData = new DTOCourseData
                        {
                            CourseType = CourseType.Compulsory,
                            CreditHours = 15,
                            Description = item.summary,
                            LecturesHours = 71,
                            PracticalHours = 9,
                            TotalHours = 120,
                            TotalMarks = 200
                        }

                    };

                    request.AddParameter("application/json", JsonConvert.SerializeObject(model), ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    var result = JsonConvert.DeserializeObject<DTOAddEditCourse>(response.Content);
                    addedCoursed.Add(result);
                }


            }

            #endregion


            return Ok(addedCoursed);
        }


        [HttpGet]
        [Route("ImportCourseStandardFromBedo/{id}")] // id = "D5FCB9F0-3131-4688-BBBE-6A719B54D25B"
        public async Task<IActionResult> ImportCourseStandardFromBedo([FromRoute] string id)
        {

            string token = Request.Headers["Authorization"];

            var bedoCourses = await courseDataAccessLayer.GetAllCourses();
            List<DTOAddEditCourse> addedCoursed = new List<DTOAddEditCourse>();

            var client = new RestClient("http://localhost:5001/courses");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"{token}");
            request.AddHeader("Content-Type", "application/json");

            foreach (var item in bedoCourses)
            {
                DTOAddEditCourse model = new DTOAddEditCourse()
                {
                    Name = item.CourseName,
                    Code = item.CourseCode,
                    CourseSubscriptionId = new Guid(id),
                    CourseData = new DTOCourseData
                    {
                        CourseType = CourseType.Elective,
                        CreditHours = item.CreditHours,
                        Description = item.Description,
                        LecturesHours = item.LectureHours,
                        PracticalHours = item.PracticalHours,
                        TotalHours = item.ClassesHours
                    }
                };

                request.AddParameter("application/json", JsonConvert.SerializeObject(model), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var result = JsonConvert.DeserializeObject<DTOAddEditCourse>(response.Content);
                addedCoursed.Add(result);
            }
            return Ok(addedCoursed);
        }

    }
}
