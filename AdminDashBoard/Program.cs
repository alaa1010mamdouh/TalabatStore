using AdminDashBoard.Helper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Talabat.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.identity;

namespace AdminDashBoard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<AppIdentityDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            builder.Services.AddIdentity<AppUser, IdentityRole>()
              .AddEntityFrameworkStores<AppIdentityDBContext>();
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            builder.Services.AddAutoMapper(typeof(MappsProfile));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                 //pattern: "{controller=Home}/{action=Index}/{id?}");
                 pattern: "{controller=Admin}/{action=Login}/{id?}");
            app.Run();
        }
    }
}
