using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hqs.Model;
using Microsoft.EntityFrameworkCore;

namespace Hqs.IRepository
{
    public interface IDataContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : AbstractEntity;
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
