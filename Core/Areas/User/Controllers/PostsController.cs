using _Models.RelationModels;
using _Models;
using Microsoft.AspNetCore.Mvc;
using _DataAccess.Repository.IRepository;
using Models;
using _Models.ViewModels;
using System.Security.Cryptography;

namespace Core.Areas.User.Controllers
{
    [Area("User")]
    public class PostsController : Controller
    {

        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;

        public PostsController(IUserService userService , IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index(int id)
        {
            string userId = _userService.GetCurrentUserId();
            ApplicationUser user = _unitOfWork.ApplicationRepo.Get(x => x.Id == userId);
            Post post = _unitOfWork.PostRepo.Get(x => x.Id == id , includes:"Poster");
            List<Comment> comments = _unitOfWork.CommentRepo.GetAll(x => x.postId == id, includes: "User").OrderBy(x => x.date).ToList();
            bool isLiked = false;
            bool isSaved = false;

            if (_unitOfWork.UserVoteUpsRepo.Get(x => x.UserId == userId && x.postId == id) != null)
                isLiked = true;
            if (_unitOfWork.UserPostsRepo.Get(x => x.UserId == userId && x.postId == id) != null)
                isSaved = true;

            PostUserVM vm = new PostUserVM
            {
                postvm = new PostVM { post = post , comments = comments},
                IsSaved = isSaved,
                IsLiked = isLiked,
                user = user
            };

            return View(vm);
        }

        public IActionResult SavedPosts()
        {
            string userId = _userService.GetCurrentUserId();
            ApplicationUser user = _unitOfWork.ApplicationRepo.Get(x=>x.Id == userId);
            List<int> postsIds = _unitOfWork.UserPostsRepo.GetAll(x=>x.UserId ==userId).Select(x=>x.postId).ToList();

            List<Post> posts = new List<Post>();
            foreach (int postId in postsIds)
            {
                posts.Add(_unitOfWork.PostRepo.Get(x => x.Id == postId, includes: "Poster"));
            }

            posts = posts.OrderByDescending(x => x.PostDate).ToList();

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

            List<int> likedPosts = _unitOfWork.UserVoteUpsRepo.GetAll(x=>x.UserId == userId).Select(x=>x.postId).ToList();

            HomeVM vm = new HomeVM
            {
                posts = postModels,
                voteUps = likedPosts,
                user = user
            };
            return View(vm);
        }
        
        [HttpPost]
        [Route("/CreatePost")]
        public IActionResult CreatePost(string title, string body)
        {
            Post post = new Post();
            string userId = _userService.GetCurrentUserId();
            post.userId = userId;
            post.Title = title;
            post.Body = body;
            post.PostDate = DateTime.Now;
            post.Loves = 0;

            _unitOfWork.PostRepo.Add(post);
            _unitOfWork.Save();
            TempData["Success"] = "Post Created Successfully";
            return RedirectToAction("Index");
        }
        
        public IActionResult EditPost(int id)
        {
            Post post = _unitOfWork.PostRepo.Get(x => x.Id == id);
            return View(post);
        }

        [HttpPost]
        public IActionResult _EditPost(Post post)
        {
            _unitOfWork.PostRepo.Update(post);
            _unitOfWork.Save();
            TempData["Success"] = "Updated Successfully";
            return RedirectToAction("Index" , "Profile");
        }
        
        public IActionResult DeletePost(int id)
        {
            List<User_Posts> savedposts = _unitOfWork.UserPostsRepo.GetAll(x => x.postId == id).ToList();
            _unitOfWork.UserPostsRepo.RemoveRange(savedposts);

            List<User_VoteUps> likedPosts = _unitOfWork.UserVoteUpsRepo.GetAll(x => x.postId == id).ToList();
            _unitOfWork.UserVoteUpsRepo.RemoveRange(likedPosts);

            Post post = _unitOfWork.PostRepo.Get(x => x.Id == id);
            _unitOfWork.PostRepo.Remove(post);
            _unitOfWork.Save();

            TempData["Success"] = "Deleted Successfully";
            return RedirectToAction("Index", "Profile");
        }

        public IActionResult Like([FromBody] PostModel m)
        {
            string userId = _userService.GetCurrentUserId();
            Post post = _unitOfWork.PostRepo.Get(u => u.Id == m.id);
            post.Loves += 1;
            _unitOfWork.PostRepo.Update(post);

            User_VoteUps userVoteUps = new User_VoteUps
            {
                UserId = userId,
                postId = m.id,
            };
            _unitOfWork.UserVoteUpsRepo.Add(userVoteUps);
            _unitOfWork.Save();

            return Json(new { url = "" });
        }

        public IActionResult Dislike([FromBody] PostModel m)
        {
            string userId = _userService.GetCurrentUserId();
            Post post = _unitOfWork.PostRepo.Get(u => u.Id == m.id);
            post.Loves -= 1;
            _unitOfWork.PostRepo.Update(post);

            User_VoteUps? x = _unitOfWork.UserVoteUpsRepo.Get(x => x.UserId == userId && x.postId == m.id);
            if (x != null)
            {
                _unitOfWork.UserVoteUpsRepo.Remove(x);
            }
            _unitOfWork.Save();

            return Json(new { url = "" });
        }

        public IActionResult AddToBookmarks([FromBody] PostModel m)
        {
            string userId = _userService.GetCurrentUserId();

            User_Posts relation = new User_Posts
            {
                UserId = userId,
                postId = m.id,
            };

            _unitOfWork.UserPostsRepo.Add(relation);
            _unitOfWork.Save();

            return Json(new { url = "" });
        }

        public IActionResult RemoveFromBookmarks([FromBody] PostModel m)
        {
            string userId = _userService.GetCurrentUserId();

            User_Posts relation  = _unitOfWork.UserPostsRepo.Get(x=> x.UserId == userId && x.postId == m.id);

            _unitOfWork.UserPostsRepo.Remove(relation);
            _unitOfWork.Save();

            return Json(new { url = "" });
        }

        public IActionResult AddComment([FromBody] CommentModel m)
        {
            if(!string.IsNullOrEmpty(m.comment))
            {
                string userId = _userService.GetCurrentUserId();

                Comment comment = new Comment
                {
                    userId = userId,
                    postId = m.postId,
                    Body = m.comment,
                    date = DateTime.Now
                };

                _unitOfWork.CommentRepo.Add(comment);
                _unitOfWork.Save();
            }

            return Json(new { url = "" });
        }
    }

    public class PostModel
    {
        public int id { get; set; }
    }

    public class CommentModel
    {
        public int postId { get; set; }
        public string comment { get; set; }
    }
}
