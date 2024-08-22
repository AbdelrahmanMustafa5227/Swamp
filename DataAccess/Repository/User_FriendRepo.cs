using _DataAccess.Repository.IRepository;
using _Models.RelationModels;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace _DataAccess.Repository
{
    public class User_FriendRepo : Repo<User_Friend> ,  IUser_FriendRepo { 

        private readonly Context _context;

        public User_FriendRepo(Context context) : base(context)
        {
            _context = context;
        }

        public List<User_Friend> GetAllFriends(string userId)
        {
            List<User_Friend> peopleIFriended = _context.user_Friends.Include(x => x.Friend).Where(u => u.UserId == userId).ToList();
            List<User_Friend> peopleFriendedMe = _context.user_Friends.Include(x => x.User).Where(u => u.FriendId == userId).ToList();
            /* Here, I had to replace friends with user because in the view i can access @Model.friendId to get all
               friends ID in one shot  */
            peopleFriendedMe.ForEach(x => x.Friend = x.User);
            peopleFriendedMe.ForEach(y => y.FriendId = y.UserId);

            return peopleIFriended.Union(peopleFriendedMe).ToList();
        }

        public List<string> GetAllFriendIds(string userId)
        {
            List<User_Friend> frnds = GetAllFriends(userId);
            List<string> ids = frnds.Select(x => x.FriendId).ToList();
            return ids;
        }
    }
}
