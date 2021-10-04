using Microsoft.AspNetCore.Mvc;
using Qorrect.Integration.Models;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Qorrect.Integration.Services;

namespace Qorrect.Integration.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ControlPanelController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        string BedoIntegrateConstr = "";

        public ControlPanelController(IConfiguration configuration)
        {
            _configuration = configuration;
            BedoIntegrateConstr = _configuration.GetConnectionString("BedoIntegrateConstr");
        }

        [HttpPost]
        [Route("ApplyMoodleSetting")]
        public async Task<IActionResult> ApplyMoodleSetting([FromBody] DTOManageUrl model)
        {
            await new CourseDataAccessLayer().MoodleConfigurationSetting(BedoIntegrateConstr, model);
            return Ok();
        }

        [HttpPost]
        [Route("ApplyBedoSetting")]
        public async Task<IActionResult> ApplyBedoSetting([FromBody] DTOManageUrl model)
        {
            await new CourseDataAccessLayer().BedoConfigurationSetting(BedoIntegrateConstr, model);
            return Ok();
        }
    }
}
