using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Aplication.Core;
using Reactivities.DataDBContext;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Aplication.Activities
{
    public class List
    {
        //For [fetching Data] we need to use [Query]
        //The [IRequest] is what we want to [Return] from the [paticuler] [Request]. in this case a [List of (Activity)]
        public class Query : IRequest<Result<List<ActivityDto>>> { }

        //For [Handling Requests] we need to use [Handler]. 
        //[IRequestHandler] [defines] [how and what] we gonna [Handle]. Continue Down VV
        //In this case we want to [Handle] a [Query] and [Return] a [List of (Activity)].
        public class Handler : IRequestHandler<Query, Result<List<ActivityDto>>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            //Now this [Function] will [Return] a [List<Activity>]. And we have [access] to the [Query] that will [get].
            //This will get all the [Activities] from the [DbSet<Activity> Activities] that is inside the [DataContext]
            public async Task<Result<List<ActivityDto>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var activities = await _context.Activities
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();

                return Result<List<ActivityDto>>.Success(activities);
            }
        }
    }
}
