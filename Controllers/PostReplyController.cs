using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebProjectAircraftForum.Areas.Identity.Data;
using WebProjectAircraftForum.Data;
using WebProjectAircraftForum.Models.Post;
using WebProjectAircraftForum.Models.PostReply;

namespace WebProjectAircraftForum.Controllers
{
    [Authorize]
    public class PostReplyController : Controller
    {
        private readonly IPost _postService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAppUser _userService;

        public PostReplyController(IPost postService, IAppUser userService, UserManager<ApplicationUser> userManager)
        {
            _postService = postService;
            _userManager = userManager;
            _userService = userService;
        }

        //CREATE GET
        public async Task<IActionResult> Create(int id)
        {
            var post = _postService.GetById(id);

            if (post == null)
            {
                return View("OopsPage");
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var model = new PostReplyModel
            {
                PostContent = post.Content,
                PostTitle = post.Title,
                PostId = post.Id,
                PostCreatedOn = post.CreatedOn,

                AuthorId = user.Id,
                AuthorName = User.Identity.Name,
                AuthorImageUrl = user.ProfileImageUrl,
                AuthorRating = user.Rating,
                IsAuthorAdmin = User.IsInRole("Admin"),
                AuthorMemberSince = user.MemberSince, // inspect

                ForumId = post.Forum.Id,
                ForumName = post.Forum.Title,
                ForumImageUrl = post.Forum.ImageUrl,

                CreatedOn = DateTime.Now, // may be unnecessary

                PostAuthorId = post.User.Id,
                PostAuthorName = post.User.UserName,
                PostAuthorImageUrl = post.User.ProfileImageUrl,
                PostAuthorRating = post.User.Rating,
                PostAuthorMemberSince = post.User.MemberSince,
                IsPostAuthorAdmin = IsPostAuthorAdmin(post.User)
            };

            return View(model);
        }
        
        //CREATE POST
        [HttpPost]
        public async Task<IActionResult> Create(PostReplyModel model)
        {
            var userId = _userManager.GetUserId(User);
            var user = await _userManager.FindByIdAsync(userId);

            var reply = BuildReply(model, user);

            await _postService.AddReply(reply);
            await _userService.UpdateRating(userId, typeof(PostReply));

            return RedirectToAction("Index", "Post", new { id = model.PostId });
        }


        //Private Methods
        private bool IsPostAuthorAdmin(ApplicationUser user)
        {
            return _userManager.GetRolesAsync(user)
                .Result.Contains("Admin");
        }
        private PostReply BuildReply(PostReplyModel model, ApplicationUser user)
        {
            var post = _postService.GetById(model.PostId);

            return new PostReply
            {
                Post = post,
                Content = model.ReplyContent,
                CreatedOn = DateTime.Now,
                User = user
            };
        }
    }
}
