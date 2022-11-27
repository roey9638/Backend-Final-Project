﻿using AutoMapper;
using Reactivities.Aplication.Activities;
using Reactivities.Aplication.Comments;
using Reactivities.Aplication.Profiles;
using Reactivities.Modules;
using System.Linq;

namespace Reactivities.Aplication.Core
{
    public class MappingProfiles : AutoMapper.Profile
    {
        public MappingProfiles()
        {
            string currentUsername = null;

            //This is just [Mapping] all the [Properties] that [Activity] has into a new [Activity] that's why it's [<Activity, Activity>]
            //We needed to [Add] this [AutoMapper] as a [Service] inr are [Startup class]
            CreateMap<Activity, Activity>();

            CreateMap<Activity, ActivityDto>()
                .ForMember(d => d.HostUsername, options => options.MapFrom(source => source.Attendees
                .FirstOrDefault(x => x.IsHost).AppUser.UserName));

            CreateMap<ActivityAttendee, AttendeeDto>()
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
                .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.AppUser.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.AppUser.Followers.Count))
                .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.AppUser.Followings.Count))
                .ForMember(d => d.Following,
                    o => o.MapFrom(s => s.AppUser.Followers.Any(x => x.Observer.UserName == currentUsername)));


            CreateMap<AppUser, Profiles.Profile>()
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count))
                .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count))
                .ForMember(d => d.Following,
                    o => o.MapFrom(s => s.Followers.Any(x => x.Observer.UserName == currentUsername)));


            CreateMap<Comment, CommentDto>()
                .ForMember(d => d.DisplayName, opt => opt.MapFrom(source => source.Author.DisplayName))
                .ForMember(d => d.Username, opt => opt.MapFrom(source => source.Author.UserName))
                .ForMember(d => d.Image, opt => opt.MapFrom(source => source.Author.Photos.FirstOrDefault(x => x.IsMain).Url));


            CreateMap<ActivityAttendee, UserActivityDto>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Activity.Id))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Activity.Date))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Activity.Title))
                .ForMember(d => d.Category, o => o.MapFrom(s => s.Activity.Category))
                .ForMember(d => d.HostUsername, o => o.MapFrom(s =>
                    s.Activity.Attendees.FirstOrDefault(x => x.IsHost).AppUser.UserName));
        }
    }
}
