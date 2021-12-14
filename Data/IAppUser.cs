using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProjectAircraftForum.Areas.Identity.Data;

namespace WebProjectAircraftForum.Data
{
    public interface IAppUser
    {
        ApplicationUser GetById(string id);
        IEnumerable<ApplicationUser> GetAll();

        Task SetProfileImage(string id, Uri uri);
        Task UpdateRating(string id, Type type);
        Task UpdateUserInfo(string id, string firstName, string lastName, string email, DateTime birthDate, string country);
    }
}
