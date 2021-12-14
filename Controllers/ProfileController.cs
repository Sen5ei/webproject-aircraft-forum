using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebProjectAircraftForum.Areas.Identity.Data;
using WebProjectAircraftForum.Data;
using WebProjectAircraftForum.Models.AppUser;

namespace WebProjectAircraftForum.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAppUser _userService;
        private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;

        public ProfileController(UserManager<ApplicationUser> userManager, IAppUser userService, IUpload uploadService, IConfiguration configuration)
        {
            _userManager = userManager;
            _userService = userService;
            _uploadService = uploadService;
            _configuration = configuration;
        }

        //DETAIL_GET
        public IActionResult Detail(string id)
        {
            var user = _userService.GetById(id);

            if (user == null)
            {
                return View("OopsPage");
            }

            var userRoles = _userManager.GetRolesAsync(user).Result;

            var model = new ProfileModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                UserRating = user.Rating.ToString(),
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                MemberSince = user.MemberSince,
                IsAdmin = userRoles.Contains("Admin"),
                
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Country = user.Country
            };

            return View(model);
        }

        //EDIT POST
        [HttpPost]
        public async Task<IActionResult> EditUser(ProfileModel model)
        {
            var userId = _userManager.GetUserId(User);

            await _userService.UpdateUserInfo(userId, model.FirstName, model.LastName, model.Email, model.DateOfBirth, model.Country);

            return RedirectToAction("Detail", "Profile", new { id = userId });
        }


        //Update_User_Profile_Image POST
        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var userId = _userManager.GetUserId(User);

            if (file != null)
            {
                //connect to Azure Storage container
                var connString = _configuration.GetConnectionString("AzureStorageAccount");

                //Get Blob container (responsibility of UploadService)
                var container = _uploadService.GetBlobContainer(connString, "profile-images");

                //Parse the Content Disposition response header
                var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);

                //Grab the filename
                var fileName = contentDisposition.FileName.Trim('"');

                //Get a reference to a BlobClient
                var blobClient = container.GetBlobClient(AppendTimeStamp(fileName));

                //On that BlobClient, upload our file <-- file uploaded to the cloud
                await blobClient.UploadAsync(file.OpenReadStream());

                //Set the User's profile image to the URI
                await _userService.SetProfileImage(userId, blobClient.Uri);
            }

            //Redirect to the User's profile page
            return RedirectToAction("Detail", "Profile", new { id = userId });
        }

        //INDEX GET
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var profiles = _userService.GetAll()
                .OrderBy(user => user.UserName)
                .Select(u => new ProfileModel
                {
                    UserId = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    ProfileImageUrl = u.ProfileImageUrl,
                    UserRating = u.Rating.ToString(),
                    MemberSince = u.MemberSince
                });

            var model = new ProfileListingModel
            {
                Profiles = profiles
            };

            return View(model);
        }

        private string AppendTimeStamp(string fileName)
        {
            string filenameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);

            return string.Concat(filenameWithoutExtension, DateTime.Now.ToFileTime().ToString(), extension);
        }
    }
}
