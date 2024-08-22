using _Models.RelationModels;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _Models.ViewModels
{
    public class FriendsVM
    {
        public List<User_Friend> Friends {  get; set; }
        public List<ApplicationUser> PeopleYouMayKnow { get; set; }
        public List<FriendRequest> PendingRequests { get; set; }
        public List<FriendRequest> ReceivedRequests { get; set; }
    }
}
