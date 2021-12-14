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

    public class PostController : Controller
    {
        private readonly IPost _postService;
        private readonly IForum _forumService;
        private readonly IAppUser _userService;

        //providing APIs for interacting with Users
        private static UserManager<ApplicationUser> _userManager;
        public PostController(IPost postService, IForum forumService, IAppUser userService, UserManager<ApplicationUser> userManager)
        {
            _postService = postService;
            _forumService = forumService;
            _userManager = userManager;
            _userService = userService;
        }

        //INDEX GET
        public IActionResult Index(int id)
        {
            var post = _postService.GetById(id);

            if (post == null)
            {
                return View("OopsPage");
            }

            var replies = BuildPostReplies(post.Replies);

            var model = new PostIndexModel
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorImageUrl = post.User.ProfileImageUrl,
                AuthorRating = post.User.Rating,
                AuthorMemberSince = post.User.MemberSince,
                CreatedOn = post.CreatedOn,
                PostContent = post.Content,
                Replies = replies,
                ForumId = post.Forum.Id,
                ForumName = post.Forum.Title,
                ForumImageUrl = post.Forum.ImageUrl,
                IsAuthorAdmin = IsAuthorAdmin(post.User)
            };

            return View(model);
        }

        //CREATE_POST GET
        [Authorize]
        public async Task<IActionResult> Create(int id)
        {
            var forum = _forumService.GetById(id);

            if (forum == null)
            {
                return View("OopsPage");
            }

            var user = await _userManager.FindByNameAsync(User.Identity.Name);

            var model = new NewPostModel
            {
                ForumName = forum.Title,
                ForumId = forum.Id,
                ForumImageUrl = forum.ImageUrl,
                AuthorName = User.Identity.Name,

                AuthorId = user.Id,
                AuthorImageUrl = user.ProfileImageUrl,
                AuthorMemberSince = user.MemberSince,
                AuthorRating = user.Rating,
                IsAuthorAdmin = IsAuthorAdmin(user)
            };

            return View(model);
        }

        //CREATE_POST POST
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(NewPostModel model)
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;

            var post = BuildPost(model, user);

            await _postService.Add(post);
            await _userService.UpdateRating(userId, typeof(Post));

            return RedirectToAction("Index", "Post", new { id = post.Id });
        }

        //EDIT_POST POST
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditPost(PostIndexModel model)
        {
            var post = _postService.GetById(model.Id);
            await _postService.EditPostContent(post.Id, model.PostContent);
            TempData["EditSuccess"] = "Content changed successfully";

            return RedirectToAction("Index", new { id = post.Id });
        }

        //DELETE_POST POST
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = _postService.GetById(id);
            await _postService.Delete(id);

            return RedirectToAction("Topic", "Forum", new { id = post.Forum.Id });
        }

        //EDIT/DELETE_POSTREPLY GET
        [Authorize]
        public IActionResult LoadPostReplyPartial(int id)
        {
            var reply = _postService.GetReplyById(id);
            var model = new PostReplyModel
            {
                Id = reply.Id,
                ReplyContent = reply.Content,
                CreatedOn = reply.CreatedOn,
                PostId = reply.Post.Id
            };
            return PartialView("_PostReplyPartial", model);  
        }

        //EDIT_POSTREPLY POST
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditReply(PostReplyModel model)
        {
            var reply = _postService.GetReplyById(model.Id);
            await _postService.EditPostReplyContent(reply.Id, model.ReplyContent);
            TempData["EditSuccess"] = "Content changed successfully";

            return RedirectToAction("Index", new { id = reply.Post.Id });
        }

        //DELETE_POSTREPLY POST
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteReply(int id)
        {
            var reply = _postService.GetReplyById(id);
            await _postService.DeletePostReply(reply.Id);

            return RedirectToAction("Index", new { id = reply.Post.Id });
        }


        //Private Methods
        private bool IsAuthorAdmin(ApplicationUser user)
        {
            return _userManager.GetRolesAsync(user)
                .Result.Contains("Admin");
        }

        private Post BuildPost(NewPostModel model, ApplicationUser user)
        {
            var forum = _forumService.GetById(model.ForumId);

            return new Post
            {
                Title = model.Title,
                Content = model.Content,
                CreatedOn = DateTime.Now,
                User = user,
                Forum = forum
            };
        }

        private IEnumerable<PostReplyModel> BuildPostReplies(IEnumerable<PostReply> replies)
        {
            return replies.Select(reply => new PostReplyModel
            {
                Id = reply.Id,
                AuthorName = reply.User.UserName,
                AuthorId = reply.User.Id,
                AuthorImageUrl = reply.User.ProfileImageUrl,
                AuthorRating = reply.User.Rating,
                AuthorMemberSince = reply.User.MemberSince,
                CreatedOn = reply.CreatedOn,
                ReplyContent = reply.Content,
                IsAuthorAdmin = IsAuthorAdmin(reply.User),
            });
        }
    }
}
