using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Reactivities.DataDBContext;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reactivities.Secuirty
{
    public class IsHostRequirement : IAuthorizationRequirement
    {

    }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly DataContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //This [Variable] [_httpContextAccessor] is to get [Access] to the [Root ID] of the [Activity] we [trying] to [Access]
        public IsHostRequirementHandler(DataContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }


        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            //That means that the [User] is [Not Authorized] is [Not] the [Host], And he [Can't] [Edit] the [Activity]
            if (userId == null)
            {
                return Task.CompletedTask;
            }

            //The [Root ID] [Value] is [Guid]. So we did [Guid.Parse] to [Convert] is to [string]
            var activityId = Guid.Parse(_httpContextAccessor.HttpContext?.Request.RouteValues
                .SingleOrDefault(x => x.Key == "id").Value?.ToString());


            //This [Should] [Contain] the [Activity attendee] [Object]
            var attendee = _dbContext.ActivityAttendees
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.AppUserId == userId && x.ActivityId == activityId).Result;


            if (attendee == null)
            {
                return Task.CompletedTask;
            }

            if (attendee.IsHost)
            {
                context.Succeed(requirement);
            }

            //That means that the [User] is [Authorized] [He is] the [Host], And he [Can] [Edit] the [Activity]
            return Task.CompletedTask;
        }
    }


}
