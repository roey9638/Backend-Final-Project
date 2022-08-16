using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Reactivitiess.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseAPIController: ControllerBase
    {
        private IMediator _mediator;

        //The [??=] means that if [_mediator] is not avilable which means is [null]. Continue Down vv
        //Then he will be [HttpContext] and we want to get the [Service] which is type of [IMediator] which will [encapsulate] [/reques/response]
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
    }
}
