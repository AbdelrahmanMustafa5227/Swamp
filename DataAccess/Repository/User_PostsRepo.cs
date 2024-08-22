using _DataAccess.Repository.IRepository;
using _Models.RelationModels;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace _DataAccess.Repository
{
    public class User_PostsRepo : Repo<User_Posts> , IUser_PostsRepo
    { 

        private readonly Context _context;

        public User_PostsRepo(Context context) : base(context)
        {
            _context = context;
        }

    }
}
