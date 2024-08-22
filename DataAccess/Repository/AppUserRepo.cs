using _DataAccess.Repository.IRepository;
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
    public class AppUserRepo : Repo<ApplicationUser> , IAppUserRepo { 

        private readonly Context _context;

        public AppUserRepo(Context context) : base(context)
        {
            _context = context;
        }
         
        

    }
}
