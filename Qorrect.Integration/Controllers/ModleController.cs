using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Qorrect.Integration.Models;
using RestSharp;
using System.Threading.Tasks;

namespace Qorrect.Integration.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ModleController : ControllerBase
    {

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

    }
}
