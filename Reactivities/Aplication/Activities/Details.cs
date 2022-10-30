using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Aplication.Core;
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
        public class Query : IRequest<Result<ActivityDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ActivityDto>>
        {
            private readonly DataContext _context;

            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }


            //Now this [Function] will [Return] a [Activity]. And we have [access] to the [Query] that will [get].
            //This will get all the [Activities] from the [DbSet<Activity> Activities] that is inside the [DataContext]
            public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                //This [request.Id] is coming from the [Query class] that we [created] Above^^.
                var activity = await _context.Activities
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(x => x.Id == request.Id);

                //Here I'm sending the [activity] to the [Success()] [function] Inside the [Result class].
                //The [Result class] is a [Generic class] that's why we have the [<Activity>]. We [Returning] an a [Activity]
                return Result<ActivityDto>.Success(activity);
            }
        }
    }
}
