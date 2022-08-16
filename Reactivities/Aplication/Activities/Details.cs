using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.DataDBContext;
using Reactivities.Modules;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Aplication.Activities
{
    public class Details
    {
        //In Here we want to get a [specific] [Activity] so added to him and [Guid (Id)]
        //[Query] [Returns] [Data]
        public class Query : IRequest<Activity>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Activity>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }


            //Now this [Function] will [Return] a [Activity]. And we have [access] to the [Query] that will [get].
            //This will get all the [Activities] from the [DbSet<Activity> Activities] that is inside the [DataContext]
            public async Task<Activity> Handle(Query request, CancellationToken cancellationToken)
            {
                //This [request.Id] is coming from the [Query class] that we [created] Above^^.
                return await _context.Activities.FindAsync(request.Id);
            }
        }
    }
}
