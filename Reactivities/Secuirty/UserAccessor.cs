﻿using Microsoft.AspNetCore.Http;
using Reactivities.Aplication.Interfaces;
using System.Security.Claims;

namespace Reactivities.Secuirty
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUsername()
        {
            return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
        }
    }
}
