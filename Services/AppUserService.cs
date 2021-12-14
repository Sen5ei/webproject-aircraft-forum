using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProjectAircraftForum.Areas.Identity.Data;
using WebProjectAircraftForum.Data;
using WebProjectAircraftForum.Models.Post;
using WebProjectAircraftForum.Models.PostReply;

namespace WebProjectAircraftForum.Services
{
    public class AppUserService : IAppUser
    {
        private readonly WebProjectAircraftForumDbContext _context;

        public AppUserService(WebProjectAircraftForumDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.ApplicationUsers;
        }

        public ApplicationUser GetById(string id)
        {
            return GetAll().FirstOrDefault(user => user.Id == id);
        }

        public async Task UpdateRating(string userId, Type type)
        {
            var user = GetById(userId);
            user.Rating = CalculateRating(type, user.Rating);

            await _context.SaveChangesAsync();
        }

        private int CalculateRating(Type type, int userRating)
        {
            var increment = 0;

            if (type == typeof(Post))
            {
                increment = 1;
            }

            if (type == typeof(PostReply))
            {
                increment = 2;
            }

            return userRating + increment;
        }

        public async Task SetProfileImage(string id, Uri uri)
        {
            var user = GetById(id);
            user.ProfileImageUrl = uri.AbsoluteUri;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserInfo(string id, string firstName, string lastName, string email, DateTime birthDate, string country)
        {
            var user = GetById(id);
            user.Email = email;
            user.FirstName = firstName;
            user.LastName = lastName;
            user.DateOfBirth = birthDate;
            user.Country = country;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
