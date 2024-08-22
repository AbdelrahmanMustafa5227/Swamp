using Models;
using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using _Models.RelationModels;
using _Models;
using Newtonsoft.Json;
using _DataAccess.Repository.IRepository;
using Core.Filters;
using System.Collections.Specialized;
using _Models.ViewModels;

namespace Core.Areas.User.Controllers
{
    [Area("User")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger , IUserService userService , IUnitOfWork unitOfWork )
        {
            _logger = logger;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }
        
        public IActionResult Index()
        {
            string userId = _userService.GetCurrentUserId();
            ApplicationUser user = _unitOfWork.ApplicationRepo.Get(x => x.Id == userId);

            List<string> FriendsId = _unitOfWork.UserFriendRepo.GetAllFriendIds(userId);
            FriendsId.Add(userId);

            List<Post> posts = new List<Post>();

            foreach(string id in FriendsId)
            {
                posts.AddRange(_unitOfWork.PostRepo.GetAll(x=>x.userId==id , includes:"Poster"));
            }

            posts = posts.OrderByDescending(x => x.PostDate).ToList();

            List<PostVM> postModels = new List<PostVM>();

            foreach(Post post in posts)
            {
                List<Comment> comments = new List<Comment>();
                comments = _unitOfWork.CommentRepo.GetAll(x => x.postId == post.Id, includes: "User").OrderBy(x=>x.date).ToList();

                postModels.Add(new PostVM
                {
                    post = post,
                    comments = comments
                });
            }

            IEnumerable<int> votesup = _unitOfWork.UserVoteUpsRepo.GetAll(x => x.UserId == userId).Select(x => x.postId);
            IEnumerable<int> savedposts = _unitOfWork.UserPostsRepo.GetAll(x => x.UserId == userId).Select(x => x.postId);

            HomeVM vm = new HomeVM
            {
                user = user,
                posts = postModels,
                voteUps = votesup,
                savedPosts = savedposts
            };

            return View(vm);

        }


        public IActionResult ChatRoom()
        {
            return View();
        }

        public IActionResult CheckName(string Fullname , string Bio) 
        {
            if(Fullname != Bio) {
                return Json(true);
            }
            return Json(false);
        }

        public IActionResult testAjax(string Id)
        {
            ApplicationUser asd = _unitOfWork.ApplicationRepo.Get(r => r.Id == Id);
            return Json(asd);
        }


        


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
