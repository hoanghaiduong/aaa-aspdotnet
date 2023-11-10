using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace aaa_aspdotnet.src.DAL.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> GetById(string id);
        Task<List<T>> GetAll();
        Task<List<T>> Find(Expression<Func<T, bool>> expression);
        Task<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Add(T entity);
        void Update(T entity);

        void Remove(T entity);


        Task<bool> SaveChangesAsync();
    }
}
