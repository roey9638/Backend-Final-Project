using AutoMapper;
using MediatR;
using Reactivities.DataDBContext;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace Reactivities.Aplication.Activities
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Id);

                _context.Remove(activity);

                await _context.SaveChangesAsync();

                return Unit.Value; //This is like [returning] [Nothing]
            }
        }
    }
}
