using _Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _DataAccess.Repository.IRepository
{
    public interface IFriendRequestRepo : IRepo<FriendRequest>
    {
        public List<FriendRequest> GetAllPendingFriendRequests(string userId);
        public List<FriendRequest> GetAllReceivedFriendRequests(string userId);
    }
}
