using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProjectAircraftForum.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebProjectAircraftForum.Models.PostReply
{
    [Table("PostReply")]
    public class PostReply
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You have to enter the Content")]
        public string Content { get; set; }

        [Display(Name = "Created on")]
        [Required]
        public DateTime CreatedOn { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Post.Post Post { get; set; }
    }
}
