using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Aplication.Core;
using Reactivities.Aplication.Interfaces;
using Reactivities.DataDBContext;
using Reactivities.Modules;
using System.Threading;
using System.Threading.Tasks;

namespace Reactivities.Aplication.Activities
{
    public class Create
    {
        public class Command : IRequest<Result<Unit>>
        {
            public Activity Activity { get; set; } 
        }

        public class CommandVlidator : AbstractValidator<Command>
        {
            public CommandVlidator()
            {
                RuleFor(x => x.Activity).SetValidator(new ActivityValidator());
            }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _context;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext context, IUserAccessor userAccesor)
            {
                _context = context;
                _userAccessor = userAccesor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == _userAccessor.GetUsername());

                var attendee = new ActivityAttendee
                {
                    AppUser = user,
                    Activity = request.Activity,
                    IsHost = true
                };

                request.Activity.Attendees.Add(attendee);

                _context.Activities.Add(request.Activity);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                {
                    return Result<Unit>.Failure("Failed to create activity");
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
