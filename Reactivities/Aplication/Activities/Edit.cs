using MediatR;
using Reactivities.DataDBContext;
using Reactivities.Modules;
using System.Threading.Tasks;
using System.Threading;
using AutoMapper;

namespace Reactivities.Aplication.Activities
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Activity Activity { get; set; }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
                
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var acitvity = await _context.Activities.FindAsync(request.Activity.Id);

                //The [request.Activity] is [where we want to] [Map] the [Properties] [from]. And the [acitvity] is [where we] [Map] the [Properties] to.
                _mapper.Map(request.Activity, acitvity);
           
                await _context.SaveChangesAsync();

                return Unit.Value; //This is like [returning] [Nothing]
            }
        }
    }
}
