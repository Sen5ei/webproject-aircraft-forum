using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebProjectAircraftForum.Areas.Identity.Data;
using WebProjectAircraftForum.Models;
using WebProjectAircraftForum.Models.Forum;
using WebProjectAircraftForum.Models.Post;
using WebProjectAircraftForum.Models.PostReply;

namespace WebProjectAircraftForum.Data
{
    public class WebProjectAircraftForumDbContext : IdentityDbContext<ApplicationUser>
    {
        public WebProjectAircraftForumDbContext(DbContextOptions<WebProjectAircraftForumDbContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Forum> Forums { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostReply> PostReplies { get; set; }
    }
}
