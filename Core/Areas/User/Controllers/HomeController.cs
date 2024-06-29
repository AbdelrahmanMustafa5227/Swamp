using Models;
using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using _Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace Core.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Context _context;

        public HomeController(ILogger<HomeController> logger , Context context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var claim = (ClaimsIdentity)User.Identity;
            var user = claim.FindFirst(ClaimTypes.NameIdentifier);

            //everyone ids except me
            List<string> AllIds  = new List<string>();
            // friends ids
            List<string> friendsIds = new List<string>();
            // non-friends ids
            List<string> nonFriendsIds = new List<string>();
            List<ApplicationUser> nonFriends = new List<ApplicationUser>();

            if (user != null)
            {
                //everyone ids except me
                AllIds = _context.applicationUsers.Where(u => u.Id != user.Value).Select(x => x.Id).ToList();
                // friends ids
                friendsIds = _context.user_Friends.Where(u => u.UserId == user.Value).Select(x => x.FriendId).ToList();
                // non-friends ids
                nonFriendsIds = AllIds.Except(friendsIds).ToList();

                foreach (var x in nonFriendsIds)
                {
                    nonFriends.Add(_context.applicationUsers.FirstOrDefault(u => u.Id == x));
                }
            }

            return View(nonFriends);
        }
        
        public IActionResult Befriend(string id)
        {
            var claim = (ClaimsIdentity)User.Identity;
            var userid = claim.FindFirst(ClaimTypes.NameIdentifier).Value;

            User_Friend relation = new User_Friend
            {
                FriendId = id,
                UserId = userid,
                FriendsSince = DateTime.Now
            };
            
            _context.user_Friends.Add(relation);
            _context.SaveChanges();
            
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
