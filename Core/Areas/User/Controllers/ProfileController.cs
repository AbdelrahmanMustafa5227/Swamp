using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Models;
using _Models;
using System.Security.Claims;
using _Models.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Core.Areas.User.Controllers
{
    [Area(nameof(User))]
    [Authorize]
    public class ProfileController : Controller
    {
        Context _context { get; set; }
        public ProfileController(Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var claims = (ClaimsIdentity)User.Identity;
            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;
            ApplicationUser user = _context.applicationUsers.FirstOrDefault(u => u.Id == userId);
            IEnumerable<Post> posts =  _context.posts.Where(u=>u.userId == userId).ToList().OrderByDescending(u=>u.Id);
            ProfileVM profileVM = new ProfileVM
            {
                user = user,
                postsHistory = posts
            };
            return View(profileVM);
        }
        [HttpPost]
        [ActionName("Index")]
        public IActionResult IndexPost(ProfileVM profileVM)
        {
            var claims = (ClaimsIdentity)User.Identity;
            var userId = claims.FindFirst(ClaimTypes.NameIdentifier).Value;

            profileVM.newpost.userId = userId;

            profileVM.newpost.PostDate = DateTime.Now;
            profileVM.newpost.Likes = 0;
            profileVM.newpost.Dislikes = 0;

            _context.posts.Add(profileVM.newpost);
            _context.SaveChanges();
            
            return RedirectToAction();
        }
    }
}
