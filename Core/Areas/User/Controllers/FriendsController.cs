using _DataAccess.Repository.IRepository;
using _Models;
using _Models.RelationModels;
using _Models.ViewModels;
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
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        public FriendsController(IUserService userService ,IUnitOfWork unitOfWork)
        {
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            string userid = _userService.GetCurrentUserId();
            List<User_Friend> friends = _unitOfWork.UserFriendRepo.GetAllFriends(userid);
            List<FriendRequest> pending = _unitOfWork.FriendRequestRepo.GetAllPendingFriendRequests(userid);
            List<FriendRequest> received = _unitOfWork.FriendRequestRepo.GetAllReceivedFriendRequests(userid);

            // People You May Know are all users except => (you , friends , people whom you sent or receive a friend request)
            List<ApplicationUser> peopleYouMayKnow = _unitOfWork.ApplicationRepo
                .GetAll(x => x.Id != userid && !friends.Select(y => y.FriendId).Contains(x.Id)
                && !pending.Select(y => y.ToId).Contains(x.Id)
                && !received.Select(y => y.FromId).Contains(x.Id)).ToList();
        
            FriendsVM friendsVM = new FriendsVM
            {
                Friends = friends,
                PeopleYouMayKnow = peopleYouMayKnow,
                PendingRequests = pending,
                ReceivedRequests = received
            };

            return View(friendsVM);
        }
        
        public IActionResult SendFriendRequest(string id)
        {
            string userId = _userService.GetCurrentUserId();

            FriendRequest request = new FriendRequest
            {
                FromId = userId,
                ToId = id,
                Message = "Can U Accept my friend request ?",
                SendDate = DateTime.Now
            };

            _unitOfWork.FriendRequestRepo.Add(request);
            _unitOfWork.Save();
            TempData["Success"] = "Sent Successfully !";
            return RedirectToAction("Index");
        }

        public IActionResult AcceptFriendRequest(string id , string name)
        {
            string userId = _userService.GetCurrentUserId();

            User_Friend relation = new User_Friend
            {
                FriendId = id,
                UserId = userId,
                FriendsSince = DateTime.Now
            };

            _unitOfWork.UserFriendRepo.Add(relation);
            _unitOfWork.FriendRequestRepo.Remove(_unitOfWork.FriendRequestRepo.Get(f => f.ToId == userId && f.FromId == id));
            _unitOfWork.Save();
            TempData["Success"] = $"You and {name} are now Friends";
            return RedirectToAction("Index");
        }

        public IActionResult DenyFriendRequest(string id)
        {
            string userId = _userService.GetCurrentUserId();
            _unitOfWork.FriendRequestRepo.Remove(_unitOfWork.FriendRequestRepo.Get(f => f.ToId == userId && f.FromId == id));
            _unitOfWork.Save();
            TempData["Success"] = "Done";
            return RedirectToAction("Index");
        }

        public IActionResult CancelFriendRequest(string id)
        {
            string userId = _userService.GetCurrentUserId();
            _unitOfWork.FriendRequestRepo.Remove(_unitOfWork.FriendRequestRepo.Get(f => f.FromId == userId && f.ToId == id));
            _unitOfWork.Save();
            TempData["Success"] = "Invitation Canceled";
            return RedirectToAction("Index");
        }
        
        public IActionResult RemoveFromFriends(string id)
        {
            string userId = _userService.GetCurrentUserId();
            User_Friend toBeDeleted = _unitOfWork.UserFriendRepo
                .Get(u => u.UserId == userId && u.FriendId == id || u.UserId == id && u.FriendId == userId);

            _unitOfWork.UserFriendRepo.Remove(toBeDeleted);
            _unitOfWork.Save();
            TempData["Success"] = "Deleted Successfully !";
            return RedirectToAction("Index");
        }

        public IActionResult ViewFriend(string id)
        {
            string userId = _userService.GetCurrentUserId();
            ApplicationUser pro = _unitOfWork.ApplicationRepo.Get(u => u.Id == userId);

            ApplicationUser user = _unitOfWork.ApplicationRepo.Get(u => u.Id == id);
            IEnumerable<Post> posts = _unitOfWork.PostRepo.GetAll(u => u.userId == id , includes:"Poster").OrderByDescending(u => u.Id);

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

            ProfileVM vm = new ProfileVM
            {
                pro = pro,
                user = user,
                posts = postModels,
                voteUps = votesup,
                savedPosts = savedposts
            };
            return View("FriendsProfile", vm);
        }

        public IActionResult Like(int postid)
        {
            string userId = _userService.GetCurrentUserId();
            Post post = _unitOfWork.PostRepo.Get(u => u.Id == postid);
            post.Loves += 1;
            _unitOfWork.PostRepo.Update(post);

            User_VoteUps userVoteUps = new User_VoteUps
            {
                UserId = userId,
                postId = postid,
            };
            _unitOfWork.UserVoteUpsRepo.Add(userVoteUps);
            _unitOfWork.Save();
            return RedirectToAction("ViewFriend", new { id = post.userId });

        }

        public IActionResult Dislike(int postid)
        {
            string userId = _userService.GetCurrentUserId();
            Post post = _unitOfWork.PostRepo.Get(u => u.Id == postid);
            post.Loves -= 1;
            _unitOfWork.PostRepo.Update(post);

            User_VoteUps? x = _unitOfWork.UserVoteUpsRepo.Get(x => x.UserId == userId && x.postId == postid);
            if (x != null)
            {
                _unitOfWork.UserVoteUpsRepo.Remove(x);
            }
            _unitOfWork.Save();
            return RedirectToAction("ViewFriend", new { id = post.userId });
        }


    }
}
