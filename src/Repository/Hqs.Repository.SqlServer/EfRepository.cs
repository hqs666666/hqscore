using System.Linq;
using System.Threading.Tasks;
using Hqs.IRepository;
using Hqs.Model;
using Microsoft.EntityFrameworkCore;

namespace Hqs.Repository.SqlServer
{
    public class EfRepository<T> : IBaseRepository<T> where T : AbstractEntity
    {
        private IDataContext _dataContext;
        private DbSet<T> mEntities;

        public EfRepository(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public virtual T GetById(object id)
        {
            return Entities.Find(id);
        }

        public Task<T> GetByIdAsync(object id)
        {
            return Entities.FindAsync(id);
        }

        public virtual IQueryable<T> Table => Entities;
        public virtual IQueryable<T> TableNoTracking => Entities.AsNoTracking();
        protected virtual DbSet<T> Entities => mEntities ?? (mEntities = _dataContext.Set<T>());
    }
}
