using AutoMapper;
using Reactivities.Aplication.Activities;
using Reactivities.Modules;
using System.Linq;

namespace Reactivities.Aplication.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //This is just [Mapping] all the [Properties] that [Activity] has into a new [Activity] that's why it's [<Activity, Activity>]
            //We needed to [Add] this [AutoMapper] as a [Service] inr are [Startup class]
            CreateMap<Activity, Activity>();

            CreateMap<Activity, ActivityDto>()
                .ForMember(d => d.HostUsername, options => options.MapFrom(source => source.Attendees
                .FirstOrDefault(x => x.IsHost).AppUser.UserName));

            CreateMap<ActivityAttendee, Profiles.Profile>()
                .ForMember(d => d.DisplayName, opt => opt.MapFrom(source => source.AppUser.DisplayName))
                .ForMember(d => d.Username, opt => opt.MapFrom(source => source.AppUser.UserName))
                .ForMember(d => d.Bio, opt => opt.MapFrom(source => source.AppUser.Bio));
                
        }
    }
}
