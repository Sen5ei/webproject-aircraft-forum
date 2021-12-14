using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebProjectAircraftForum.Models.Post
{
    public class NewPostModel
    {
        //Forum props
        public int ForumId { get; set; }
        public string ForumName { get; set; }
        public string ForumImageUrl { get; set; }

        //New_Post props
        [Required(ErrorMessage = "You have to specify the Title")]
        [StringLength(150, ErrorMessage = "Title length can't be more than 150 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "You have to enter the Post Content")]
        public string Content { get; set; }

        //Author_props
        public string AuthorName { get; set; }
        public string AuthorId { get; set; }
        public string AuthorImageUrl { get; set; }
        public DateTime AuthorMemberSince { get; set; }
        public int AuthorRating { get; set; }
        public bool IsAuthorAdmin { get; set; }
    }
}
