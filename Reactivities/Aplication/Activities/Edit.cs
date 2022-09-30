using MediatR;
using Reactivities.DataDBContext;
using Reactivities.Modules;
using System.Threading.Tasks;
using System.Threading;
using AutoMapper;
using FluentValidation;
using Reactivities.Aplication.Core;
//using System.Diagnostics;

namespace Reactivities.Aplication.Activities
{
    public class Edit
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; }
        }

        //Here I'm Doint the [Error Handling]
        public class CommandVlidator : AbstractValidator<Command>
        {
            public CommandVlidator()
            {
                //The [(x) is the (Activity)] we doing [Error Handling] on. We getting the [Activity] from the [Command class] when we do [AbstractValidator<Command>]
                //And the [SetValidator()] [tells] where we doing the [Validation] and [it's inside] the [ActivityValidator() class]
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IMapper _mapper;

            public Handler(DataContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
                
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _context.Activities.FindAsync(request.Activity.Id);

                if (activity == null)
                {
                    return null;
                }

                //The [request.Activity] is [where we want to] [Map] the [Properties] [from]. And the [acitvity] is [where we] [Map] the [Properties] to.
                _mapper.Map(request.Activity, activity);
           
                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                {
                   return Result<Unit>.Failure("Failed to update activity");
                }

                return Result<Unit>.Success(Unit.Value); //This is like [returning] [Nothing]
            }
        }
    }
}
