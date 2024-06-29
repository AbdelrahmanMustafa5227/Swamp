using _Models;
using DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Security.Claims;

namespace Core.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class FriendsController : Controller
    {
        private readonly Context _context;
        public FriendsController(Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var claim = (ClaimsIdentity)User.Identity;
            var userid = claim.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<User_Friend> user = _context.user_Friends.Include(x => x.Friend).Where(u => u.UserId == userid).ToList();
            return View(user);
        }
        public IActionResult Remove(string id)
        {
            var claim = (ClaimsIdentity)User.Identity;
            var userid = claim.FindFirst(ClaimTypes.NameIdentifier).Value;
            User_Friend user = _context.user_Friends.FirstOrDefault(u => u.UserId == userid && u.FriendId == id);
            _context.user_Friends.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("Index",user);
        }
    }
}
