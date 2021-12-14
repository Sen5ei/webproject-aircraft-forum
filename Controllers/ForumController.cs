using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebProjectAircraftForum.Data;
using WebProjectAircraftForum.Models;
using WebProjectAircraftForum.Models.Forum;
using WebProjectAircraftForum.Models.Post;

namespace WebProjectAircraftForum.Controllers
{
    public class ForumController : Controller
    {
        private readonly IForum _forumService;
        private readonly IPost _postService;
        private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;

        public ForumController(IForum forumService, IPost postService, IUpload uploadService, IConfiguration configuration)
        {
            _forumService = forumService;
            _postService = postService;
            _uploadService = uploadService;
            _configuration = configuration;
        }

        //INDEX GET
        public IActionResult Index()
        {
            IEnumerable<ForumListingModel> forums = _forumService.GetAll().ToList() //ToList() prevents "There is already an open DataReader associated with this Command which must be closed first." error 
                .Select(forum => new ForumListingModel
                {
                    Id = forum.Id,
                    Title = forum.Title,
                    Description = forum.Description,
                    ImageUrl = forum.ImageUrl,

                    NumberOfPosts = forum.Posts?.Count() ?? 0,
                    NumberOfUsers = _forumService.GetActiveUsers(forum.Id).Count(),
                    HasRecentPost = _forumService.HasRecentPost(forum.Id),
                    
                    LatestPostAuthorId = _postService.GetPostsByForum(forum.Id).LastOrDefault()?.User.Id ?? null,
                    LatestPostAuthorName = _postService.GetPostsByForum(forum.Id).LastOrDefault()?.User.UserName ?? null,
                    LatestPostAuthorImageUrl = _postService.GetPostsByForum(forum.Id).LastOrDefault()?.User.ProfileImageUrl ?? null,
                    LatestPostTitle = _postService.GetPostsByForum(forum.Id).LastOrDefault()?.Title ?? null,
                    LatestPostCreatedOn = _postService.GetPostsByForum(forum.Id).LastOrDefault()?.CreatedOn.ToString("dd/MM/yyyy HH:mm") ?? null,
                    LatestPostId = _postService.GetPostsByForum(forum.Id).LastOrDefault()?.Id ?? null
                });

            var model = new ForumIndexModel
            {
                ForumList = forums.OrderBy(f => f.Title)
            };

            return View(model);
        }

        //TOPIC GET
        public IActionResult Topic(int id, string searchQuery)
        {
            var forum = _forumService.GetById(id);

            if (forum == null)
            {
                return View("OopsPage");
            }

            var posts = new List<Post>();

            posts = _postService.GetFilteredPosts(forum, searchQuery).ToList();

            var postListings = posts.Select(post => new PostListingModel
            {
                Id = post.Id,
                AuthorId = post.User.Id,
                AuthorRating = post.User.Rating,
                AuthorName = post.User.UserName,
                Title = post.Title,
                DatePosted = post.CreatedOn,
                RepliesCount = post.Replies.Count(),
                Forum = BuildForumListing(post),

                LatestReplyAuthorId = post.Replies.LastOrDefault()?.User.Id ?? null,
                LatestReplyAuthorName = post.Replies.LastOrDefault()?.User.UserName ?? null,
                LatestReplyAuthorImageUrl = post.Replies.LastOrDefault()?.User.ProfileImageUrl ?? null,
                LatestReplyCreatedOn = post.Replies.LastOrDefault()?.CreatedOn ?? null,
                LatestReplyContent = post.Replies.LastOrDefault()?.Content ?? null
                
            }).OrderByDescending(p => p.LatestReplyCreatedOn > p.DatePosted ? p.LatestReplyCreatedOn : p.DatePosted); //sorting by LatestReplyCreatedOn and DatePosted simultaneously

            var model = new ForumTopicModel
            {
                Posts = postListings,
                Forum = BuildForumListing(forum)
            };

            return View(model);
        }

        //SEARCH POST
        [HttpPost]
        public IActionResult Search(int id, string searchQuery)
        {
            return RedirectToAction("Topic", new { id, searchQuery });
        }

        //CREATE GET
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var model = new CreateForumModel();

            return View(model);
        }

        //CREATE POST
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(CreateForumModel model)
        {
            var imageUri = "/images/forum/default.png";

            if (model.ImageUpload != null)
            {
                var blobClient = UploadForumImage(model.ImageUpload);
                imageUri = blobClient.Uri.AbsoluteUri;
            }

            var forum = new Forum
            {
                Title = model.Title,
                Description = model.Description,
                CreatedOn = DateTime.Now,
                ImageUrl = imageUri
            };

            await _forumService.Create(forum);

            return RedirectToAction("Index", "Forum");
        }

        //EDIT GET
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var forum = _forumService.GetById(id);

            if (forum == null)
            {
                return View("OopsPage");
            }

            var model = new EditForumModel
            {
                Id = forum.Id,
                Title = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageUrl
            };

            return View(model);
        }

        //EDIT POST
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(EditForumModel model)
        {
            var forum = _forumService.GetById(model.Id);

            var imageUrl = forum.ImageUrl;

            if (model.ImageUpload != null)
            {
                var blobClient = UploadForumImage(model.ImageUpload);
                imageUrl = blobClient.Uri.AbsoluteUri;
            }

            await _forumService.Edit(model.Id, model.Title, model.Description, imageUrl);

            return RedirectToAction("Index", "Forum");
        }

        //Upload Forum Image to Cloud
        private BlobClient UploadForumImage(IFormFile file)
        {
            var connString = _configuration.GetConnectionString("AzureStorageAccount");

            var container = _uploadService.GetBlobContainer(connString, "forum-images");

            var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

            var fileName = contentDisposition.FileName.Trim('"');

            var blobClient = container.GetBlobClient(AppendTimeStamp(fileName));

            blobClient.UploadAsync(file.OpenReadStream()).Wait(); //Wait() creates issues in prod, without Wait() image is not uploaded (throws an exception)

            return blobClient;
        }

        private string AppendTimeStamp(string fileName)
        {
            string filenameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            return string.Concat(filenameWithoutExtension, DateTime.Now.ToFileTime().ToString(), extension);
        }

        private ForumListingModel BuildForumListing(Post post)
        {
            var forum = post.Forum;

            return BuildForumListing(forum);
        }

        private ForumListingModel BuildForumListing(Forum forum)
        {

            return new ForumListingModel
            {
                Id = forum.Id,
                Title = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageUrl
            };
        }
    }
}
