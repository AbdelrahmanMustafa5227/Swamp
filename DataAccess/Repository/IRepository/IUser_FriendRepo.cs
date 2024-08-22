using _Models.RelationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _DataAccess.Repository.IRepository
{
    public interface IUser_FriendRepo : IRepo<User_Friend>
    {
        public List<User_Friend> GetAllFriends(string userId);
        public List<string> GetAllFriendIds(string userId);
    }
}
