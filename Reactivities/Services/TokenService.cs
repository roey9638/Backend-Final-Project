using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Reactivities.Modules;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security;
using System.Security.Claims;
using System.Text;

namespace Reactivities.Services
{
    public class TokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string CreateToken(AppUser user)
        {
            //[Important] ---------------------> In the [Website] [JWT.io], [It's like] the [PAYLOAD:DATA]!!!!!! The [Pink] [String]!!!!!!!!!!!!!!!!!
            //The [Token] can [contain] [Claims] abot the [User]. The [User] can [Claim] [things] like there [User name] is ["something"] or there [Email] is ["something"]. Continue Down VV
            //So we [created] a [List] of [Claims] that we [gonna] [Add] and [Send] [Back] [with our] [Token].
            var claims = new List<Claim>()
            {
                //The [User] can [Claim] that he [has] a [Name]. So the [ClaimTypes.Name] will be what the [User] [passed/put] in [user.UserName].
                new Claim(ClaimTypes.Name, user.UserName),

                //The [User] can [Claim] that he [has] a [NameIdentifier]. So the [ClaimTypes.NameIdentifier] will be what the [User] [passed/put] in [user.Id].
                new Claim(ClaimTypes.NameIdentifier, user.Id),

                //The [User] can [Claim] that he [has] a [Email]. So the [ClaimTypes.Email] will be what the [User] [passed/put] in [user.Email].
                new Claim(ClaimTypes.Email, user.Email),
            };


            //This is to [Sign] our [Token]. We [Sign] our [Token] with an [encrypted Key]
            //The [SymmetricSecurityKey] [means], That [It can] [only] be [decoded] [By a] [Single Key]. Continue Down VV
            //And that [Key] is [Going] to be [Stored] On [Our] [Server] And will never [Leave] the [Server]
            //The [SymmetricSecurityKey] [takes] [Param] [Byte[]] [array]. Continue Down VV
            //The [Encoding.UTF8.GetBytes] Can [Transform] a [String] into [Bytes]. [Important] --> It [has] [to be] [12 characters] [at least].
            //The [_config["TokenKey"]] is from the [File] [appsettings.Development.json]. It will [Pass] in the [Key] we want to [Use].
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["TokenKey"]));


            //[credentials - אישורים]
            //[Important] -------------> In the [Website] [JWT.io], The [key] [It's like] the [HEADER:ALGORITHM & TOKEN TYPE]!! The [Red] [String]!!!!!!!
            //[Important] -------------> In the [Website] [JWT.io], The [SecurityAlgorithms.HmacSha512Signature] [It's like] the [VERIFY SIGNATURE]!!! The [Red] [String]!!!!!!!!!!!!!!!!!!
            //We use [SigningCredentials] Because are [Token] needs to be [Signed] [By] the [Server]
            //The [SigningCredentials] [needs] [2 Params]
            //[Param 1] is the [key] that we [Created] [Above ^^]
            //[Param 2] is the [SecurityAlgorithms.HmacSha512Signature], And it's to [encrypt] The [Key]
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //This is to [Describe] the [Token]. We [need] to [tell it]. Continue Down VV
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //We [need] to [tell] The [SecurityTokenDescriptor] [About] the [Claims]
                Subject = new ClaimsIdentity(claims),

                //We [need] to [tell] The [SecurityTokenDescriptor] [How long] it's [gonna last] [before] it [expires]
                Expires = DateTime.Now.AddDays(7),

                //We [need] to [tell] The [SecurityTokenDescriptor] And [Pass] in our [Signing] [Credentials] That we [Created] Above ^^.
                SigningCredentials = creds

            };

            //Here I'm [creating] a [tokenHandler]
            var tokenHandler = new JwtSecurityTokenHandler();

            //This is where I [Actually] [Creating] the [Token].
            var token = tokenHandler.CreateToken(tokenDescriptor);

            //The [tokenHandler.WriteToken] will [Serialize] the [(token)] into a [JWT] In a [Compact Serialization Format]
            //This wil [Return] a [JWT] [Token] that we can [Return] when a [User] [Logs In]. Continue Down VV
            //And then the [User] [will be] [able] to [Use] that [Token] to [Authnticate]!!! [to our] [API].
            return tokenHandler.WriteToken(token);

        }
    }
}
