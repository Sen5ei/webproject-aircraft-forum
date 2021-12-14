using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebProjectAircraftForum.Models.PostReply;

namespace WebProjectAircraftForum.Models.Post
{
    public class PostIndexModel
    {
        //Post props
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Post Content")]
        [Required(ErrorMessage = "You have to enter the Post Content")]
        public string PostContent { get; set; }

        //Post_Author props
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int AuthorRating { get; set; }
        public string AuthorImageUrl { get; set; }
        public DateTime AuthorMemberSince { get; set; }
        public bool IsAuthorAdmin { get; set; }

        //Post_Forum props
        public int ForumId { get; set; }
        public string ForumName { get; set; }
        public string ForumImageUrl { get; set; }


        public IEnumerable<PostReplyModel> Replies { get; set; }

    }
}
