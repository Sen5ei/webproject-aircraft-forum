using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebProjectAircraftForum.Models.AppUser
{
    public class ProfileModel
    {
        public string UserId { get; set; }

        [EmailAddress(ErrorMessage = "Not a valid Email format")]
        [Display(Name = "Email address")]
        public string Email { get; set; }
        public string UserName { get; set; }
        public string UserRating { get; set; }
        public string ProfileImageUrl { get; set; }
        public bool IsAdmin { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public DateTime MemberSince { get; set; }
        public IFormFile ImageUpload { get; set; }

        [Display(Name = "First name")]
        [StringLength(100, ErrorMessage = "First name length can't be more than 100 characters")]
        public string FirstName { get; set; }

        [Display(Name = "Last name")]
        [StringLength(100, ErrorMessage = "Last name length can't be more than 100 characters")]
        public string LastName { get; set; }

        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }
        public string Country { get; set; }
    }
}
