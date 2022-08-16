using AutoMapper;
using Reactivities.Modules;

namespace Reactivities.Aplication.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            //This is just [Mapping] all the [Properties] that [Activity] has into a new [Activity] that's why it's [<Activity, Activity>]
            //We needed to [Add] this [AutoMapper] as a [Service] inr are [Startup class]
            CreateMap<Activity, Activity>();
        }
    }
}
