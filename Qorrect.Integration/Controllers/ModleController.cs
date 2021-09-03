using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Qorrect.Integration.Helper;
using Qorrect.Integration.Models;
using RestSharp;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using Microsoft.AspNetCore.Hosting;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Qorrect.Integration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModleController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public string QorrectBaseUrl { get; set; }
        public ModleController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            QorrectBaseUrl = _configuration.GetValue<string>("QorrectBaseUrl");
            _webHostEnvironment = webHostEnvironment;

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


        [HttpPost]
        [Route("LoadQuestionsFromXML")]
        public async Task<IActionResult> LoadQuestionsFromXML([FromBody] DTOAddCourseRequest courseRequest)
        {
            string token = $"Bearer {courseRequest.BearerToken}";

            string xmlfile = Path.Combine(this._webHostEnvironment.ContentRootPath, "DataSource/") + "Quiz.xml";
            XDocument xmlDoc = XDocument.Load(xmlfile);
            IEnumerable<XElement> quizes = xmlDoc.Descendants("question");
            foreach (var quiz in quizes)
            {
                string qType = quiz.Attribute("type").Value;
                string qName = quiz.Element("name").Element("text").Value;
                string qText = quiz.Element("questiontext").Element("text").Value;
                string qFile = quiz.Element("questiontext").Element("file").Value;
                string fileName = quiz.Element("questiontext").Element("file").Attribute("name").Value;

                #region Convert Image File to Base64 Encoded string

                string uploadedPath = Path.Combine(this._webHostEnvironment.ContentRootPath, "Upload/");
                System.IO.File.WriteAllBytes(uploadedPath + fileName, Convert.FromBase64String(qFile));

                #endregion



                List<DTOAnswer> dTOAnswers = new List<DTOAnswer>();
                IEnumerable<XElement> answers = quiz.Elements("answer");
                foreach (var answer in answers)
                {

                    dTOAnswers.Add(new DTOAnswer
                    {
                        Text = answer.Element("text").Value,
                        IsCorrect = false
                    });
                }


                Guid CourseSubscriptionId = Guid.Parse(courseRequest.CourseSubscriptionId);
                var mcqclient = new RestClient($"{QorrectBaseUrl}/item/mcq");
                mcqclient.Timeout = -1;
                var mcqrequest = new RestRequest(Method.POST);
                mcqrequest.AddHeader("Authorization", token);
                mcqrequest.AddHeader("Content-Type", "application/json");

                var body = new DTOAddQuestion
                {
                    CourseSubscriptionId = CourseSubscriptionId,
                    Version = new DTOVersion
                    {
                        Stem = new DTOStem
                        {
                            Text = qText,
                            PlainText = qName,
                            Comment = "no",
                            Difficulty = 0,
                            Settings = new DTOSettings
                            {
                                IsShuffleAnswers = true,
                                IsAllowForTrialExams = true,
                                Difficulty = 1,
                                ExpectedTime = 1,
                                IsAllowedForComputerBasedOnly = true
                            },
                            Answers = dTOAnswers
                        },
                        ItemClassification = 1,
                        Tags = new List<Guid?>(),
                        ItemMappings = new List<DTOItemMapping>()
                                                //{
                                                //    new DTOItemMapping
                                                //    {
                                                //        IloId = Guid.Parse(resultILO.Id.ToString()),
                                                //        LevelId =  resultleaf.Id
                                                //    }
                                                //}
                    },
                    TransactionItemId = Guid.Parse("3fa85f64-5717-4562-b3fc-2c963f66afa6") // will chamge it

                };
                mcqrequest.AddParameter("application/json", JsonConvert.SerializeObject(body), ParameterType.RequestBody);
                IRestResponse mcqresponse = mcqclient.Execute(mcqrequest);


            }

            return Ok(quizes);
        }
    }
}
