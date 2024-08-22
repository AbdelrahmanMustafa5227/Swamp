using _DataAccess.Repository.IRepository;
using _Models;
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
    public class FriendRequestRepo : Repo<FriendRequest> , IFriendRequestRepo  { 

        private readonly Context _context;

        public FriendRequestRepo(Context context) : base(context)
        {
            _context = context;
        }

        public List<FriendRequest>  GetAllPendingFriendRequests(string userId)
        {
            List<string> PendingFriendRequestsIds = _context.friendRequests
                .Where(f => f.FromId == userId)
                .Select(x => x.ToId)
                .ToList();

            return _context.friendRequests.Include(x => x.ToUser).Where(f => f.FromId == userId).ToList();
        }

        public List<FriendRequest> GetAllReceivedFriendRequests(string userId)
        {
            List<string> RecievedFriendRequestsIds = _context.friendRequests
                .Where(f => f.ToId == userId)
                .Select(x => x.FromId)
                .ToList();

            return _context.friendRequests.Include(x => x.FromUser).Where(f => f.ToId == userId).ToList();
        }

    }
}
