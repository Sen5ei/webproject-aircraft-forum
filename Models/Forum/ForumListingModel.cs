using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebProjectAircraftForum.Models.Forum
{
    public class ForumListingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        
        public int NumberOfPosts { get; set; }
        public int NumberOfUsers { get; set; }
        public bool HasRecentPost { get; set; }

        //Forum_Latest_Post props
        public string LatestPostAuthorId { get; set; }
        public string LatestPostAuthorName { get; set; }
        public string LatestPostAuthorImageUrl { get; set; }
        public int? LatestPostId { get; set; }
        public string LatestPostTitle { get; set; }
        public string LatestPostCreatedOn { get; set; }
    }
}
