using _DataAccess.Repository.IRepository;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace _DataAccess.Repository
{
    public class Repo<T> : IRepo<T>  where T : class
    {
        private readonly Context _dbContext;
        internal DbSet<T> _dbSet;

        public Repo(Context context) 
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<T>();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public T Get (Expression<Func<T,bool>> filter , string? includes = null , bool tracked = false)
        {
            IQueryable<T> query = tracked? _dbSet : _dbSet.AsNoTracking();
            query = query.Where(filter);

            if(includes != null)
            {
                foreach(string include in includes.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(include);
                }
            }
            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll (Expression<Func<T, bool>>? filter = null, string? includes = null, bool tracked = false)
        {
            IQueryable<T> query = tracked ? _dbSet : _dbSet.AsNoTracking();

            if(filter != null)
            {
                query = query.Where(filter);
            }
            if (includes != null)
            {
                foreach (string include in includes.Split(",", StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(include);
                }
            }
            return query.ToList();
        }

        public void Update (T entity)
        {
            _dbSet.Update(entity);
        }

        public void Remove (T entity)
        {
            _dbSet.Remove(entity);
        }

        public void RemoveRange(List<T> entity)
        {
            _dbSet.RemoveRange(entity);
        }

    }
}
