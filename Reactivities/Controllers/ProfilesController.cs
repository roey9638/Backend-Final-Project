using Microsoft.AspNetCore.Mvc;
using Reactivities.Aplication.Profiles;
using Reactivitiess.Controllers;
using System.Threading.Tasks;

namespace Reactivities.Controllers
{
    public class ProfilesController : BaseAPIController
    {
        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Username = username }));
        }


        [HttpPut]
        public async Task<IActionResult> Edit(Edit.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }


        [HttpGet("{username}/activities")]
        public async Task<IActionResult> GetUserActivities(string username, string predicate)
        {
            return HandleResult(await Mediator.Send(new ListActivities.Query
            { Username = username, Predicate = predicate }));
        }

    }
}
