using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Qorrect.Integration.Helper;
using Qorrect.Integration.Models;
using Qorrect.Integration.Services;
using RestSharp;
using System;
using System.Linq;
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
                        TotalHours = item.ClassesHours,
                        TotalMarks = item.TotalMarks
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
                if (result == null)
                {
                    return Ok(response.Content);
                }
                addedCoursed.Add(result);
            }



            #region Apply Outline structure to course
            {
                foreach (var item in addedCoursed)
                {
                    var client = new RestClient("http://localhost:5001/course/applyOutline");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Authorization", token);
                    request.AddHeader("Content-Type", "application/json");

                    var body = new DTOApplyOutlineStructure
                    {
                        Id = item.Id.Value
                    };

                    request.AddParameter("application/json", JsonConvert.SerializeObject(body), ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                }
            }
            #endregion


            return Ok(addedCoursed);
        }

        [HttpPost]
        [Route("ImportCourseStandardFromBedoLeaf")]
        public async Task<IActionResult> ImportCourseStandardFromBedoLeaf([FromBody] DTOAddCourseLevelRequest courseRequest)
        {
            string token = $"Bearer {courseRequest.BearerToken}";
            List<CourseLeaf> addedCourseLevels = new List<CourseLeaf>();
            DTOAddEditNodeLevel unitResponse = new DTOAddEditNodeLevel();
            DTOCognitiveLevelResponse cognitiveLevelResponse = new DTOCognitiveLevelResponse();
            List<DTOBedoILO> bedoIlos = new List<DTOBedoILO>();


            var bedoCourseLevels = await courseDataAccessLayer.GetCourseLevels(courseRequest.CourseId);

            #region Get Cognitive Level
            {
                var client = new RestClient($"http://localhost:5001/cognitivelevels?page=1&pageSize=10&courseId={courseRequest.ParentId}");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("accept", "*/*");
                request.AddHeader("Authorization", token);
                IRestResponse response = client.Execute(request);
                cognitiveLevelResponse = JsonConvert.DeserializeObject<List<DTOCognitiveLevelResponse>>(response.Content).FirstOrDefault();
            }

            #endregion

            foreach (var item in bedoCourseLevels)
            {

                #region Add Node level authorized by teacher

                {
                    var client = new RestClient("http://localhost:5001/courses/node");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Authorization", token);
                    request.AddHeader("Content-Type", "application/json");
                    var body = new DTOAddEditNodeLevel
                    {
                        Code = item.Code,
                        Name = item.Name,
                        Order = item.Order,
                        ParentId = courseRequest.ParentId
                    };

                    request.AddParameter("application/json", JsonConvert.SerializeObject(body), ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    unitResponse = JsonConvert.DeserializeObject<DTOAddEditNodeLevel>(response.Content);
                    if (unitResponse == null)
                    {
                        return Ok(response.Content);
                    }
                }

                #endregion



                #region Add Leaf Level to course outline

                {

                    foreach (var node in item.Lessons)
                    {

                        #region Get Ilos from Bedo

                        {
                            bedoIlos = await courseDataAccessLayer.GetLevelIlo(node.Id);
                            foreach (var bedoIlo in bedoIlos)
                            {

                                var client = new RestClient("http://localhost:5001/intendedlearningoutcome");
                                client.Timeout = -1;
                                var request = new RestRequest(Method.POST);
                                request.AddHeader("Authorization", token);
                                request.AddHeader("Content-Type", "application/json");
                                var body = new DTOQorrectILORequest
                                {
                                    Name = bedoIlo.Name,
                                    Code = bedoIlo.Code,
                                    CourseCognitiveLevelId = cognitiveLevelResponse.Id,
                                    CourseCognitiveLevelName = cognitiveLevelResponse.Name,
                                    CourseId = courseRequest.ParentId
                                };
                                request.AddParameter("application/json", JsonConvert.SerializeObject(body), ParameterType.RequestBody);
                                IRestResponse response = client.Execute(request);
                            }

                        }

                        #endregion

                        #region Add Lesson

                        {
                            var body = new DTOAddEditNodeLevel
                            {
                                Code = node.Code,
                                Name = node.Name,
                                Order = node.Order,
                                ParentId = unitResponse.Id.Value
                            };

                            var client = new RestClient("http://localhost:5001/courses/leaf");
                            client.Timeout = -1;
                            var request = new RestRequest(Method.POST);
                            request.AddHeader("Authorization", token);
                            request.AddHeader("Content-Type", "application/json");
                            request.AddParameter("application/json", JsonConvert.SerializeObject(body), ParameterType.RequestBody);
                            IRestResponse response = client.Execute(request);
                        }

                        #endregion

                    }

                }

                #endregion
            }
            return Ok();
        }

    }
}
