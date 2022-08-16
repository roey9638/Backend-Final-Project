using MediatR;
using Reactivities.DataDBContext;
using Reactivities.Modules;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Aplication.Activities
{
    public class Create
    {
        //[Command] [Do Not] [Returns] [Data]
        public class Command : IRequest
        {
            public Activity Activity { get; set; } 
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Activities.Add(request.Activity);

                await _context.SaveChangesAsync();

                return Unit.Value; //This is like [returning] [Nothing]
            }
        }
    }
}
