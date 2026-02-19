using LinkDev.Ticketing.Application.Interfaces;
using LinkDev.Ticketing.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace LinkDev.Ticketing.Infrastructure.Uow
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly TicketingContext _ticketingContext;
        public UnitOfWork(TicketingContext ticketingContext)
        {
            _ticketingContext = ticketingContext;
        }

        public void SaveChanges()
        {
            _ticketingContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _ticketingContext.SaveChangesAsync();
        }

        public IDbContextTransaction BeginTransaction()
        {
            return _ticketingContext.Database.BeginTransaction();
        }

        public void CommitTransaction(IDbContextTransaction transaction)
        {
            transaction.Commit();
        }

        public void RollbackTransaction(IDbContextTransaction transaction)
        {
            transaction.Rollback();
        }

        public void Dispose()
        {
            _ticketingContext.Dispose();
        }
    }
}
