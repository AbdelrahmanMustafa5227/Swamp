using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace _DataAccess.Repository.IRepository
{
    public interface IRepo<T>
    {
        public void Add(T entity);
        public T Get(Expression<Func<T, bool>> filter, string? includes = null, bool tracked = false);
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includes = null, bool tracked = false);
        public void Update(T entity);
        public void Remove(T entity);
        public void RemoveRange(List<T> entity);
    }
}
