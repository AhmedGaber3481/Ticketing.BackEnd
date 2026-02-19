using Microsoft.EntityFrameworkCore.Storage;

namespace LinkDev.Ticketing.Application.Interfaces
{
    public interface IUnitOfWork
    {
        void SaveChanges();
        Task SaveChangesAsync();
        IDbContextTransaction BeginTransaction();
        void CommitTransaction(IDbContextTransaction transaction);
        void RollbackTransaction(IDbContextTransaction transaction);
    }
}
