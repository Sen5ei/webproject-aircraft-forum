using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebProjectAircraftForum.Models.Post;

namespace WebProjectAircraftForum.Models.PostReply
{
    public class PostReplyModel
    {
        //Reply props
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Reply Content")]
        [Required(ErrorMessage = "You have to enter the Reply Content")]
        public string ReplyContent { get; set; }


        //Reply_Author props
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int AuthorRating { get; set; }
        public string AuthorImageUrl { get; set; }
        public bool IsAuthorAdmin { get; set; }
        public DateTime AuthorMemberSince { get; set; }


        //Reply_Post props
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string PostContent { get; set; }

        public string PostAuthorName { get; set; }
        public int PostAuthorRating { get; set; }
        public string PostAuthorId { get; set; }
        public bool IsPostAuthorAdmin { get; set; }
        public DateTime PostAuthorMemberSince { get; set; }
        public string PostAuthorImageUrl { get; set; }
        public DateTime PostCreatedOn { get; set; }


        //Forum props
        public int ForumId { get; set; }
        public string ForumName { get; set; }
        public string ForumImageUrl { get; set; }
    }
}
