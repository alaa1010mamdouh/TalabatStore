using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.Serices
{
    public class TokenService : ITokenServices
    {
        private readonly IConfiguration _configuration;

        public TokenService( IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser user,UserManager<AppUser> userManager)
        {
            //payload
            //private claim [user defined]

            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.GivenName, user.DisplayName)
            };
            var userRole = userManager.GetRolesAsync(user).Result;
            foreach (var Role in userRole) {

                claims.Add(new Claim(ClaimTypes.Role, Role));

            }
            //key


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var Token = new JwtSecurityToken(
                issuer:_configuration["Jwt:Issuer"],
                audience:_configuration["Jwt:Audience"],
                claims:claims,
                expires:DateTime.Now.AddDays(double.Parse(_configuration["Jwt:ExpireDays"])),
                signingCredentials : new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return  new JwtSecurityTokenHandler().WriteToken(Token);


        }
    }
}
