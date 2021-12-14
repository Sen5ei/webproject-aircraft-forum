using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebProjectAircraftForum.Data;
using WebProjectAircraftForum.Models;
using WebProjectAircraftForum.Models.Forum;
using WebProjectAircraftForum.Models.Home;
using WebProjectAircraftForum.Models.Post;

namespace WebProjectAircraftForum.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IPost _postService;

        public HomeController(ILogger<HomeController> logger, IPost postService)
        {
            _logger = logger;
            _postService = postService;
        }

        //INDEX GET
        public IActionResult Index()
        {
            var model = BuildHomeIndexModel();

            return View(model);
        }

        private HomeIndexModel BuildHomeIndexModel()
        {
            var latestPosts = _postService.GetLatestPosts(10);

            var posts = latestPosts.Select(post => new PostListingModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorName = post.User.UserName,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                DatePosted = post.CreatedOn,
                RepliesCount = post.Replies.Count(),
                Forum = GetForumListingForPost(post),

                LatestReplyAuthorId = post.Replies.LastOrDefault()?.User.Id ?? null,
                LatestReplyAuthorName = post.Replies.LastOrDefault()?.User.UserName ?? null,
                LatestReplyAuthorImageUrl = post.Replies.LastOrDefault()?.User.ProfileImageUrl ?? null,
                LatestReplyCreatedOn = post.Replies.LastOrDefault()?.CreatedOn/*.ToString("dd/MM/yyyy HH:mm")*/ ?? null,
                LatestReplyContent = post.Replies.LastOrDefault()?.Content ?? null
            });

            return new HomeIndexModel
            {
                LatestPosts = posts,
                SearchQuery = ""
            };
        }

        private ForumListingModel GetForumListingForPost(Post post)
        {
            var forum = post.Forum;

            return new ForumListingModel
            {
                Id = forum.Id,
                Title = forum.Title,
                ImageUrl = forum.ImageUrl,
            };
        }

        //ABOUT GET
        public IActionResult About()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
