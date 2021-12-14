using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProjectAircraftForum.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebProjectAircraftForum.Models.Post
{
    [Table("Post")]
    public class Post
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You have to specify the Title")]
        [StringLength(150, ErrorMessage = "Title length can't be more than 150 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "You have to enter the Content")]
        public string Content { get; set; }

        [Display(Name = "Created on")]
        [Required]
        public DateTime CreatedOn { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Forum.Forum Forum { get; set; }

        public virtual IEnumerable<PostReply.PostReply> Replies { get; set; }
    }
}
