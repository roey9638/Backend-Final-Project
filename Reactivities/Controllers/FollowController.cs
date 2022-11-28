using Microsoft.AspNetCore.Mvc;
using Reactivities.Aplication.Followers;
using Reactivitiess.Controllers;
using System.Threading.Tasks;

namespace Reactivities.Controllers
{
    public class FollowController :BaseAPIController
    {
        [HttpPost("{username}")]
        public async Task<IActionResult> Follow(string username)
        {
            return HandleResult(await Mediator.Send(new FollowToggle.Command { TargetUsername = username }));
        }


        [HttpGet("{username}")]
        public async Task<IActionResult> GetFollowings(string username, string predicate)
        {
            return HandleResult(await Mediator.Send(new List.Query { Username = username, Predicate = predicate }));
        }
    }
}
