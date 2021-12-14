using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebProjectAircraftForum.Areas.Identity.Data;
using WebProjectAircraftForum.Data;

[assembly: HostingStartup(typeof(WebProjectAircraftForum.Areas.Identity.IdentityHostingStartup))]
namespace WebProjectAircraftForum.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<WebProjectAircraftForumDbContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("WebProjectAircraftForumDbContextConnection")));

                //password restrictions removed
                services.AddDefaultIdentity<ApplicationUser>(options => 
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                    .AddRoles<IdentityRole>() //added line
                    .AddEntityFrameworkStores<WebProjectAircraftForumDbContext>();
            });
        }
    }
}