using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Models;
using _Models;
using System.Security.Claims;
using _Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using _Models.RelationModels;
using _DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using _Utilities;
using _DataAccess.Migrations;
using System.Security.Cryptography;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Core.Areas.User.Controllers
{
    [Area(nameof(User))]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProfileController(IUserService userService, IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }
        
        public IActionResult Index()
        {
            string userId = _userService.GetCurrentUserId();
            ApplicationUser user = _unitOfWork.ApplicationRepo.Get(u => u.Id == userId);

            IEnumerable<Post> posts = _unitOfWork.PostRepo.GetAll(u => u.userId == userId , includes:"Poster").OrderByDescending(u => u.Id);

            List<PostVM> postModels = new List<PostVM>();

            foreach (Post post in posts)
            {
                List<Comment> comments = new List<Comment>();
                comments = _unitOfWork.CommentRepo.GetAll(x => x.postId == post.Id, includes: "User").OrderBy(x => x.date).ToList();

                postModels.Add(new PostVM
                {
                    post = post,
                    comments = comments
                });
            }

            IEnumerable<int> votesup = _unitOfWork.UserVoteUpsRepo.GetAll(x => x.UserId == userId).Select(x => x.postId);
            IEnumerable<int> savedposts = _unitOfWork.UserPostsRepo.GetAll(x => x.UserId == userId).Select(x => x.postId);

            ProfileVM profileVM = new ProfileVM
            {
                pro = user,
                user = user,
                posts = postModels,
                voteUps = votesup,
                savedPosts = savedposts
            };
            return View(profileVM);
        }

        public IActionResult Edit()
        {
            ApplicationUser old = _unitOfWork.ApplicationRepo.Get(x => x.Id == _userService.GetCurrentUserId());
            return View(old);
        }
        
        [HttpPost]
        public IActionResult EditInfo(ApplicationUser user)
        {
            ApplicationUser old = _unitOfWork.ApplicationRepo.Get(x => x.Id == _userService.GetCurrentUserId());
            old.Bio = user.Bio;
            old.Fullname = user.Fullname;
            old.Email = user.Email;
            _unitOfWork.ApplicationRepo.Update(old);
            _unitOfWork.Save();
            TempData["Success"] = "Updated Successfully !";
            return RedirectToAction("Index");
        }

        public IActionResult ChangePic()
        {
            ApplicationUser old = _unitOfWork.ApplicationRepo.Get(x => x.Id == _userService.GetCurrentUserId());
            return View(old);
        }
        
        public IActionResult AdjustMarginForProfilePic(int margin)
        {
            ApplicationUser old = _unitOfWork.ApplicationRepo.Get(x => x.Id == _userService.GetCurrentUserId());
            old.ProfilePicMargin = margin;
            _unitOfWork.ApplicationRepo.Update(old);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        [Route("/ChangeBackImage")]
        public IActionResult ChangeBackImage([FromForm] IFormFile file)
        {
            ApplicationUser old = _unitOfWork.ApplicationRepo.Get(x => x.Id == _userService.GetCurrentUserId());

            // delete old Background Image
            string path = Path.Combine(_webHostEnvironment.WebRootPath, SD.backImgPathC);
            if (old.BackgroundPicUrl != null)
            {
                if (System.IO.File.Exists(Path.Combine(path, old.BackgroundPicUrl)))
                {
                    System.IO.File.Delete(Path.Combine(path, old.BackgroundPicUrl));
                }
            }

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            using (var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                file.CopyTo(stream);
            }

            old.BackgroundPicUrl = fileName;
            _unitOfWork.ApplicationRepo.Update(old);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("/changeProfilePicture")]
        public IActionResult ChangeProfileImage(IFormFile file)
        {
            ApplicationUser user = _unitOfWork.ApplicationRepo.Get(x => x.Id == _userService.GetCurrentUserId());
            string path = Path.Combine(_webHostEnvironment.WebRootPath, SD.ProImgPathC);
            
            if (System.IO.File.Exists(Path.Combine(path, user.ProfilePicUrl)) && !user.ProfilePicUrl.Contains("/"))
            {
                System.IO.File.Delete(Path.Combine(path, user.ProfilePicUrl));
            }
            
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            using (var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
            {
                file.CopyTo(stream);
            };

            user.ProfilePicUrl = fileName;
            _unitOfWork.ApplicationRepo.Update(user);
            _unitOfWork.Save();

            bool crop = ImgCalculations.RequireCropping(Path.Combine(path, user.ProfilePicUrl));
            
            if (crop)
            {
                HttpContext.Session.SetString("url", user.ProfilePicUrl);
                HttpContext.Session.SetString("mrg", user.ProfilePicMargin.ToString());
                return Json(new { redirectToUrl = Url.Action("AdjustPic", "Profile") });
            }
            else
            {
                return Json(new { redirectToUrl = Url.Action("Index", "Profile") });
            }

        }
 
        public IActionResult UseDefaultImage(string num)
        {
            ApplicationUser user = _unitOfWork.ApplicationRepo.Get(x => x.Id == _userService.GetCurrentUserId());
            string path = Path.Combine(_webHostEnvironment.WebRootPath, SD.ProImgPathC);

            if (System.IO.File.Exists(Path.Combine(path, user.ProfilePicUrl)) && !user.ProfilePicUrl.Contains("/"))
            {
                System.IO.File.Delete(Path.Combine(path, user.ProfilePicUrl));
            }

            user.ProfilePicUrl = "default/default" + num + ".png";
            _unitOfWork.ApplicationRepo.Update(user);
            _unitOfWork.Save();
            return Json(new { RedirectToUrl = Url.Action("Index", "Profile") });
        }

        public IActionResult AdjustPic()
        {
            return View();
        }

        
    }
}
