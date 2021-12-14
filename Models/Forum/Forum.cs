using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebProjectAircraftForum.Models.Forum
{
    [Table("Forum")]
    public class Forum
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You have to specify the Title")]
        [StringLength(100, ErrorMessage = "Title length can't be more than 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "You have to enter the Description")]
        [StringLength(200, ErrorMessage = "Description length can't be more than 200 characters")]
        public string Description { get; set; }

        [Display(Name = "Created on")]
        [Required]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }

        public virtual IEnumerable<Post.Post> Posts { get; set; }
    }
}
