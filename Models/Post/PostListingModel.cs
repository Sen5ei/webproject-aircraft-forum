using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebProjectAircraftForum.Models.Forum;

namespace WebProjectAircraftForum.Models.Post
{
    public class PostListingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public int AuthorRating { get; set; }
        public string AuthorId { get; set; }
        public DateTime DatePosted { get; set; }

        public ForumListingModel Forum { get; set; }

        public int RepliesCount { get; set; }

        //Latest_Reply props
        public string LatestReplyAuthorId { get; set; }
        public string LatestReplyContent { get; set; }
        public string LatestReplyAuthorName { get; set; }
        public string LatestReplyAuthorImageUrl { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? LatestReplyCreatedOn { get; set; }
    }
}
