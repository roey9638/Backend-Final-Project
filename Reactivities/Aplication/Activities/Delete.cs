using AutoMapper;
using MediatR;
using Reactivities.DataDBContext;
using System.Threading.Tasks;
using System.Threading;
using System;
using Reactivities.Aplication.Core;

namespace Reactivities.Aplication.Activities
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Id);

                //if (activity == null)
                //{
                //    return null;
                //}

                _context.Remove(activity);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                {
                    Result<Unit>.Failure("Failed to delete the activity");
                }

                return Result<Unit>.Success(Unit.Value); 
            }
        }
    }
}
