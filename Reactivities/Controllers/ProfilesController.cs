using Microsoft.AspNetCore.Mvc;
using Reactivities.Aplication.Profiles;
using Reactivitiess.Controllers;
using System.Threading.Tasks;

namespace Reactivities.Controllers
{
    public class ProfilesController: BaseAPIController
    {
        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Username = username }));
        }

    }
}
