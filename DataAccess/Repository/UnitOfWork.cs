using _DataAccess.Repository.IRepository;
using DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context _db;
        public AppUserRepo ApplicationRepo { get; private set; }
        public PostRepo PostRepo { get; private set; }
        public CommentRepo CommentRepo { get; private set; }
        public FriendRequestRepo FriendRequestRepo { get; private set; }
        public User_FriendRepo UserFriendRepo { get; private set; }
        public User_VoteUpsRepo UserVoteUpsRepo { get; private set; }
        public User_PostsRepo UserPostsRepo { get; private set; }

        public UnitOfWork(Context db)
        {
            _db = db;
            ApplicationRepo = new AppUserRepo(_db); 
            PostRepo = new PostRepo(_db);
            FriendRequestRepo = new FriendRequestRepo(_db);
            UserFriendRepo = new User_FriendRepo(_db);
            UserVoteUpsRepo = new User_VoteUpsRepo(_db);
            UserPostsRepo = new User_PostsRepo(_db);
            CommentRepo = new CommentRepo(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
