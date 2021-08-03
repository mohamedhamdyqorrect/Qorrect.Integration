using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Qorrect.Integration.Helper;
using Qorrect.Integration.Models;
using Qorrect.Integration.Services;
using RestSharp;
using System;
using System.Collections.Generic;
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

        [HttpPost]
        [Route("ImportCourseStandardFromBedo")]
        public async Task<IActionResult> ImportCourseStandardFromBedo([FromBody] DTOAddCourseRequest courseRequest)
        {

            string token = $"Bearer {courseRequest.BearerToken}";

            var bedoCourses = await courseDataAccessLayer.GetAllCourses();
            List<DTOAddEditCourse> addedCoursed = new List<DTOAddEditCourse>();

            foreach (var item in bedoCourses)
            {
                DTOAddEditCourse model = new DTOAddEditCourse()
                {
                    Name = item.CourseName,
                    Code = item.CourseCode,
                    CourseSubscriptionId = new Guid(courseRequest.CourseSubscriptionId),
                    CourseData = new DTOCourseData
                    {
                        CourseType = CourseType.Elective,
                        CreditHours = item.CreditHours == null ? 0 : item.CreditHours,
                        Description = item.Description,
                        LecturesHours = item.LectureHours,
                        PracticalHours = item.PracticalHours,
                        TotalHours = item.ClassesHours
                    }
                };

                var client = new RestClient("http://localhost:5001/courses");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", token);
                request.AddHeader("Content-Type", "application/json");

                request.AddParameter("application/json", JsonConvert.SerializeObject(model), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var result = JsonConvert.DeserializeObject<DTOAddEditCourse>(response.Content);
                addedCoursed.Add(result);
            }
            return Ok(addedCoursed);
        }

        [HttpPost]
        [Route("ImportCourseStandardFromBedoLeaf")]
        public async Task<IActionResult> ImportCourseStandardFromBedoLeaf([FromBody] DTOAddCourseLevelRequest courseRequest)
        {

            string token = $"Bearer {courseRequest.BearerToken}";

            var bedoCourseLevels = await courseDataAccessLayer.GetCourseLevels(courseRequest.CourseId);
            List<CourseLeaf> addedCourseLevels = new List<CourseLeaf>();


            foreach (var item in bedoCourseLevels)
            {
                var client = new RestClient("http://localhost:5001/courses/leaf");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", token);
                request.AddHeader("Content-Type", "application/json");



                request.AddParameter("application/json", JsonConvert.SerializeObject(item), ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);



                //var result = JsonConvert.DeserializeObject<DTOAddEditCourse>(response.Content);
                //addedCoursed.Add(result);
            }
            return Ok();
        }

    }
}
