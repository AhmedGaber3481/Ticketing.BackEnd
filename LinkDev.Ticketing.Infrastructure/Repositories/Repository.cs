using LinkDev.Ticketing.Application.Interfaces;
using LinkDev.Ticketing.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LinkDev.Ticketing.Infrastructure.Repositories
{
    public class Repository<T> :IRepository<T> where T:class
    {
        protected DbSet<T> dbSet;
        protected TicketingContext _dBContext;

        public Repository(TicketingContext dbContext)
        {
            dbSet = dbContext.Set<T>();
            _dBContext = dbContext;

            //_dBContext.Database.SqlQueryRaw
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void AddRange(T[] entities)
        {
            dbSet.AddRange(entities);
        }

        public void Update(T entity)
        {
            dbSet.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(int Id)
        {
            T? entity = dbSet.Find(Id);
            if (entity != null)
            {
                dbSet.Remove(entity);
            }
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();
        }

        public IQueryable<T> GetAll(string navigationProperty)
        {
            return dbSet.Include(navigationProperty);
        }

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] dependencies)
        {
            return Includes(dbSet.AsQueryable(), dependencies);
        }

        public T? GetById(int Id)
        {
            return dbSet.Find(Id);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression)
        {
            return dbSet.Where(expression);
        }

        public IQueryable<T> Where(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] dependencies)
        {
            return Includes(dbSet.Where(expression), dependencies);
        }

        public T? FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return dbSet.FirstOrDefault(expression);
        }

        public T? FirstOrDefault(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] dependencies)
        {
            return Includes(dbSet.AsQueryable(), dependencies).FirstOrDefault(expression);
        }

        public IQueryable<T> AsQuerable()
        {
            return dbSet.AsQueryable<T>();
        }

        #region Helpers

        private IQueryable<T> Includes(IQueryable<T> query, Expression<Func<T, object>>[] dependencies)
        {
            if (dependencies != null && dependencies.Length > 0)
            {
                var queryDependencies = query.Include(dependencies[0]);
                for (int i = 1; i < dependencies.Length; i++)
                {
                    queryDependencies = queryDependencies.Include(dependencies[i]);
                }
                return queryDependencies;
            }
            return query;
        }

        #endregion

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }
    }
}
