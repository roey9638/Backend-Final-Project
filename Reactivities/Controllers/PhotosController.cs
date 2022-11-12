using Application.Photos;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Photos;
using Reactivitiess.Controllers;
using System.Threading.Tasks;

namespace Reactivities.Controllers
{
    public class PhotosController : BaseAPIController
    {
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Add.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id}));
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMain(string id)
        {
            return HandleResult(await Mediator.Send(new SetMain.Command { Id = id }));
        }


    }
}
