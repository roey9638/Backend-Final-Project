using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reactivities.DTOs;
using Reactivities.Modules;
using Reactivities.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Reactivities.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, TokenService tokenService)
        {
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        //To [allow] [users] to be [Able] to [log In]. [They'll] [need] to be [able] [to hit] [this] [Endpoint] [Anonymously]. Continue Down VV
        //To [Allow] that, In the [Top] of This [Class ^^] We [Added] an [Attribute] -> [AllowAnonymous].
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            //In the [Table] [AspNetUsers] we have a column called [NormalizeEmail]. if the [user] gonna [send] an [Email] With [lowerCase] it's [gonna be] [compared to] the [NormalizeEmail]. Continue Down VV
            //And it's [gonna] [return] the [user object] Or [null] [which means] That the [User] [dosen't exist] in the [DataBase].
            var user = await _userManager.Users.Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.Email== loginDto.Email);

            if (user == null)
            {
                return Unauthorized();
            }

            //The [CheckPasswordSignInAsync] needs [3 Params]
            //[Param 1] The [user]. He needs to know who is the [user] that [trying] to [logIn]
            //[Param 2] The [loginDto.Password]. He will [let us] [Login] with the [Password] that the [User] [supplied]
            //[Param 3] The [false]. He wants to know if to [block] the [user] if he [put in] the [wrong] [Password]. In [this case] [we don't want] to [block] so we did [false].
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (result.Succeeded)
            {
               return CreateUserObject(user);
            }

            return Unauthorized();
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            //Here I'm [Checking] if a [New] [User] [trying] To [Register] With an [Email] that [Already] [Exist]
            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                ModelState.AddModelError("email", "Email Taken");
                return ValidationProblem();
            }

            //Here I'm [Checking] if a [New] [User] [trying] To [Register] With an [UserName] that [Already] [Exist]

            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.UserName))
            {
                ModelState.AddModelError("userName", "UserName Taken");
                return ValidationProblem();
            }

            //If The [User] that was [Trying] to [Register] With an [Email] that [Dosen't] [Already] [Exist]. [From] The the (2) [if statments] [Above] ^^. Continue Down VV
            //Then will [Create] a [New User] [with what] the [User] has [Enterd]
            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
            };

            //Here I'm [Adding] the new [user] to the [DataBase]
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            //If [Adding] the [user] to the [DataBase] [Succeeded] [then] will [Return] to the [user] all of [his] [Information
            if (result.Succeeded)
            {
                return CreateUserObject(user);
            }

            return BadRequest("Problem Registering user");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            //Here I want to [get] the [User] [By Email]
            //[Email] is [One] of the [Claiming Types] We [Sending] [with] the [Token]
            var user = await _userManager.Users.Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));

            return CreateUserObject(user);
        }

        private UserDto CreateUserObject(AppUser user)
        {
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Image = user?.Photos?.FirstOrDefault(x => x.IsMain)?.Url,
                Token = _tokenService.CreateToken(user),
                UserName = user.UserName,
            };
        }
    }
}
