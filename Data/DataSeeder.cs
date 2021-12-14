using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProjectAircraftForum.Areas.Identity.Data;

namespace WebProjectAircraftForum.Data
{
    //Populating a database with an initial set of data. If a SuperUser with the Admin Role doesn't exist in the database, it will be created.
    public class DataSeeder
    {
        private WebProjectAircraftForumDbContext _context;

        public DataSeeder(WebProjectAircraftForumDbContext context)
        {
            _context = context;
        }

        public async Task SeedSuperUser()
        {
            var roleStore = new RoleStore<IdentityRole>(_context);
            var userStore = new UserStore<ApplicationUser>(_context);

            var user = new ApplicationUser
            {
                UserName = "ForumAdmin",
                NormalizedUserName = "forumadmin",
                Email = "admin@aircraftforum.com",
                NormalizedEmail = "admin@aircraftforum.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var hasher = new PasswordHasher<ApplicationUser>();
            var hashedPassword = hasher.HashPassword(user, "{admin_password}"); //enter password here
            user.PasswordHash = hashedPassword;

            var hasAdminRole = _context.Roles.Any(roles => roles.Name == "Admin");
            if (!hasAdminRole)
            {
                await roleStore.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "admin" });
            }

            var hasSuperUser = _context.Users.Any(u => u.UserName == user.UserName);
            if (!hasSuperUser)
            {
                await userStore.CreateAsync(user);
                await userStore.AddToRoleAsync(user, "Admin");
            }

            await _context.SaveChangesAsync();
        }
    }
}
