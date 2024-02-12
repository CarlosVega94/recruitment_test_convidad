using LibraryDatabase.Entitites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryDatabase.Common

{
    public interface IApplicationDbContext
    {
        DbSet<Author> Authors { get; set; }
        DbSet<Book> Books { get; set; }

        DatabaseFacade Database { get; }
        EntityEntry Entity<TEntity>(TEntity entity)
            where TEntity : class;
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
