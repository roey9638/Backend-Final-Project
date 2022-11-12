﻿using Microsoft.AspNetCore.Identity;
using Reactivities.Aplication.Activities;
using System.Collections.Generic;

namespace Reactivities.Modules
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public ICollection<ActivityAttendee> Activities { get; set; }
        public ICollection<Photo> Photos  { get; set; }

    }
}
