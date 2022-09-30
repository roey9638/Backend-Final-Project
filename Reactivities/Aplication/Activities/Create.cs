using FluentValidation;
using MediatR;
using Reactivities.Aplication.Core;
using Reactivities.DataDBContext;
using Reactivities.Modules;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Aplication.Activities
{
    public class Create
    {
        //[Command] [Do Not] [Returns] [Data]. But in this case we want to [Return] a [Result class]. But we [don't actually] want to [return anything] so we did [Unit]
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

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                _context.Activities.Add(request.Activity);

                //If [nothing] has been [writing] [into] the [Databse]. The [result] is [going] to be [False]. Continue Down VV.
                //But [_context.SaveChangesAsync() > 0]. ---> if the [SaveChangesAsync] is [bigger / >] then  (0). The [result] is [going] to be [True].
                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                {
                    return Result<Unit>.Failure("Failed to create activity");
                }

                return Result<Unit>.Success(Unit.Value); //This is like [returning] [Nothing]
            }
        }
    }
}
