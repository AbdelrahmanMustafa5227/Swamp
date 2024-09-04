using _Models.RelationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public AppUserRepo ApplicationRepo { get; }
        public PostRepo PostRepo { get;}
        public FriendRequestRepo FriendRequestRepo { get; }
        public CommentRepo CommentRepo { get; }
        public User_FriendRepo UserFriendRepo { get; }
        public User_VoteUpsRepo UserVoteUpsRepo { get; }
        public User_PostsRepo UserPostsRepo { get; }
        public UnreadMessagesRepo UnreadMessagesRepo { get; }
        public void Save();

    }
}
