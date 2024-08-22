using _DataAccess.Repository.IRepository;
using _Models;
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
    public class CommentRepo : Repo<Comment> , ICommentRepo
    { 

        private readonly Context _context;

        public CommentRepo(Context context) : base(context)
        {
            _context = context;
        }

    }
}
