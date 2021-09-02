using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Qorrect.Integration.Helper;
using Qorrect.Integration.Models;
using RestSharp;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace Qorrect.Integration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModleController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public string QorrectBaseUrl { get; set; }
        public ModleController(IConfiguration configuration)
        {
            _configuration = configuration;
            QorrectBaseUrl = _configuration.GetValue<string>("QorrectBaseUrl");
        }

        [HttpPost]
        [Route("GenerateToken")]
        public async Task<IActionResult> GenerateToken(DTOLogin model)
        {
            var client = new RestClient("http://ahmadhafez-001-site1.ftempurl.com/login/token.php");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddParameter("username", model.username, ParameterType.QueryString);
            request.AddParameter("password", model.password, ParameterType.QueryString);
            request.AddParameter("service", "moodle_mobile_app", ParameterType.QueryString);
            IRestResponse response = await client.ExecuteAsync(request);
            return Ok(JsonConvert.DeserializeObject<DTOTokenResponse>(response.Content));
        }


        [HttpGet]
        [Route("CourseList")]
        public async Task<IActionResult> CourseList([FromQuery] string wstoken)
        {
            var client = new RestClient("http://ahmadhafez-001-site1.ftempurl.com/webservice/rest/server.php");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddParameter("wstoken", wstoken, ParameterType.QueryString);
            request.AddParameter("wsfunction", "core_course_get_courses_by_field", ParameterType.QueryString);
            request.AddParameter("field", "category", ParameterType.QueryString);
            request.AddParameter("value", "1", ParameterType.QueryString);
            request.AddParameter("moodlewsrestformat", "json", ParameterType.QueryString);
            IRestResponse response = await client.ExecuteAsync(request);
            return Ok(JsonConvert.DeserializeObject<DTOModleCourse>(response.Content));
        }

        [HttpPost]
        [Route("ImportAllFromModle")]
        public async Task<IActionResult> ImportAllFromModle([FromBody] DTOAddCourseRequest courseRequest)
        {
            string token = $"Bearer {courseRequest.BearerToken}";

            foreach (var bedoCourseitem in courseRequest.Courses)
            {
                DTOAddEditCourse model = new DTOAddEditCourse()
                {
                    Name = bedoCourseitem.CourseName,
                    Code = bedoCourseitem.CourseCode,
                    CourseSubscriptionId = new Guid(courseRequest.CourseSubscriptionId),
                    CourseData = new DTOCourseData
                    {
                        CourseType = CourseType.Elective,
                        CreditHours = 100,
                        Description = "No Description",
                        LecturesHours = 60,
                        PracticalHours = 20,
                        TotalHours = 80,
                        TotalMarks = 80
                    }
                };

                var client = new RestClient($"{QorrectBaseUrl}/courses");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", token);
                request.AddHeader("Content-Type", "application/json");

                request.AddParameter("application/json", JsonConvert.SerializeObject(model), ParameterType.RequestBody);
                IRestResponse response = await client.ExecuteAsync(request);
                var item = JsonConvert.DeserializeObject<DTOAddEditCourse>(response.Content);



                #region Apply Outline structure to course

                {
                    var applyOutlineclient = new RestClient($"{QorrectBaseUrl}/course/applyOutline");
                    applyOutlineclient.Timeout = -1;
                    var applyOutlinerequest = new RestRequest(Method.POST);
                    applyOutlinerequest.AddHeader("Authorization", token);
                    applyOutlinerequest.AddHeader("Content-Type", "application/json");

                    var body = new DTOApplyOutlineStructure
                    {
                        Id = item.Id.Value
                    };

                    applyOutlinerequest.AddParameter("application/json", JsonConvert.SerializeObject(body), ParameterType.RequestBody);
                    IRestResponse applyOutlineresponse = await applyOutlineclient.ExecuteAsync(applyOutlinerequest);
                }
                #endregion



            }

            return Ok();
        }
    }
}
