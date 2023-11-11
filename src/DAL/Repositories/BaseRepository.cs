using aaa_aspdotnet.src.DAL.Entities;
using aaa_aspdotnet.src.DAL.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace aaa_aspdotnet.src.DAL.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _set;


        public BaseRepository(AppDbContext context)
        {
            _context = context;
            _set = context.Set<T>();
        }

        public void Add(T entity)
        {
            _set.Add(entity);
        }

        public async Task<List<T>> Find(Expression<Func<T, bool>> expression)
        {
            return await _set.Where(expression).ToListAsync();
        }
        public IQueryable<T> FindAll()
        {
            return  _set.AsNoTracking();
        }

        public async Task<List<T>> GetAll()
        {
            return await _set.ToListAsync();
        }

        public async Task<T> GetById(string id)
        {
            return await _set.FindAsync(id);
        }



        public void Remove(T entity)
        {
            _set.Remove(entity);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(T entity)
        {
            _set.Update(entity);
        }
        public async Task<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return await _context.Set<T>().Where(expression).FirstOrDefaultAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await _set.FindAsync(id);
        }
    }
}
