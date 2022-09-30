using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Aplication.Core;
using Reactivities.DataDBContext;
using Reactivities.Modules;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Aplication.Activities
{
    public class List
    {
        //For [fetching Data] we need to use [Query]
        //The [IRequest] is what we want to [Return] from the [paticuler] [Request]. in this case a [List of (Activity)]
        public class Query : IRequest<Result<List<Activity>>> { }

        //For [Handling Requests] we need to use [Handler]. 
        //[IRequestHandler] [defines] [how and what] we gonna [Handle]. Continue Down VV
        //In this case we want to [Handle] a [Query] and [Return] a [List of (Activity)].
        public class Handler : IRequestHandler<Query, Result<List<Activity>>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            //Now this [Function] will [Return] a [List<Activity>]. And we have [access] to the [Query] that will [get].
            //This will get all the [Activities] from the [DbSet<Activity> Activities] that is inside the [DataContext]
            public async Task<Result<List<Activity>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return Result<List<Activity>>.Success( await _context.Activities.ToListAsync());
            }
        }
    }
}
