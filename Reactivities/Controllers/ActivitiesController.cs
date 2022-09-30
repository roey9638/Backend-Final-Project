using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reactivities.Aplication.Activities;
using Reactivities.Modules;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Reactivitiess.Controllers
{
    public class ActivitiesController : BaseAPIController
    {

        [HttpGet]
        public async Task<IActionResult> GetActivities()
        {
            //This will (Get) the [Query] which in this case a [Get methood] like [CRUD]. Continue Down VV.
            //[Maybe Not True] --> and will get the [Query] From the [List Class] that we created.
            //We get The [Mediator] from the [BaseAPIController] the this [class] is [inherating]. The explanation for him his [in that class]
            //[Impotant] --> This [Send()] is like and [HttpRequest]!!! Continue Down vv
            //[Impotant] --> And its Gonna [send it] to the [Mediator Handler] that in out [List class] that we created.
            return HandleResult(await Mediator.Send(new List.Query()));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivityById(Guid id)
        {
            //This will get all the [Activity] from the [DbSet<Activity> Activities] that is inside the [DataContext]. Continue Down VV
            //But ONLY THE (1) Activity with the [id] that we [Pass in]

            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> CreatActivity(Activity activity)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Activity = activity }));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity)
        {
            activity.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command { Activity = activity }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }

    }
}
