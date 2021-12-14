using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebProjectAircraftForum.Models.Contact
{
    public class ContactIndexModel
    {
        [Required(ErrorMessage = "You have to specify the Subject")]
        [StringLength(100, ErrorMessage = "Subject length can't be more than 100 characters")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "You have to enter the Message")]
        public string Message { get; set; }

        [Required(ErrorMessage = "You have to enter your First name")]
        [Display(Name = "First name")]
        [StringLength(100, ErrorMessage = "First name length can't be more than 100 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "You have to enter your Last name")]
        [Display(Name = "Last name")]
        [StringLength(100, ErrorMessage = "Last name length can't be more than 100 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "You have to enter your Email address")]
        [EmailAddress]
        [Display(Name = "Email address")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "You have to enter your Username")]
        [Display(Name = "Username")]
        public string UserName { get; set; }
    }
}
