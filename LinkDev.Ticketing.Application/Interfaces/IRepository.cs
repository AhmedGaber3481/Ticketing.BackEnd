using System.Linq.Expressions;

namespace LinkDev.Ticketing.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);

        void AddRange(T[] entities);

        void Update(T entity);

        void Delete(int Id);

        IEnumerable<T> GetAll();

        IQueryable<T> GetAll(string navigationProperty);

        IQueryable<T> GetAll(params Expression<Func<T, object>>[] expressions);

        T? GetById(int Id);

        IQueryable<T> Where(Expression<Func<T, bool>> expression);

        IQueryable<T> Where(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] dependencies);

        T? FirstOrDefault(Expression<Func<T, bool>> expression);

        T? FirstOrDefault(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] dependencies);

        IQueryable<T> AsQuerable();

        Task<IEnumerable<T>> GetAllAsync();
    }
}
