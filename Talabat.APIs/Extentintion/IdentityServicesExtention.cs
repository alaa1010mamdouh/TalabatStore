using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;
using Talabat.Repository.identity;
using Talabat.Serices;

namespace Talabat.APIs.Extentintion
{
    public static class IdentityServicesExtention
    {
        public static IServiceCollection AddIdentityServices (this IServiceCollection Services,IConfiguration configuration)
        {
            Services.AddScoped<ITokenServices, TokenService>();
            Services.AddIdentity<AppUser, IdentityRole>()
              .AddEntityFrameworkStores<AppIdentityDBContext>();
            Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                 .AddJwtBearer(options=>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters()
                     {
                         ValidateIssuer = true,
                         ValidIssuer = configuration["Jwt:Issuer"],
                         ValidateAudience = true,
                         ValidAudience = configuration["Jwt:Audience"],
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),


                     };
                       
                 });
            return Services;
        }
    }
}
