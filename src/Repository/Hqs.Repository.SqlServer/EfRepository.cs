using System;
using System.Linq;
using System.Threading.Tasks;
using Hqs.Dto.ResultMsg;
using Hqs.IRepository;
using Hqs.Model;
using Microsoft.EntityFrameworkCore;

namespace Hqs.Repository.SqlServer
{
    public class EfRepository<T> : IBaseRepository<T> where T : AbstractEntity
    {
        private readonly IDataContext _dataContext;
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

        public ResultMsg Insert(T entity)
        {
            Entities.Add(entity);
            return SaveChanges();
        }

        //public async Task<ResultMsg> InsertAsync(T entity)
        //{
        //    await Entities.AddAsync(entity);
        //    return SaveChanges();
        //}

        public ResultMsg SaveChanges()
        {
            ResultMsg errorResult = null;
            try
            {
                var saveResult = _dataContext.SaveChanges();
                if (saveResult == 0)
                    return new ResultMsg();
                if (saveResult >= 1)
                    return new ResultMsg();
                return new ResultMsg();
            }
            catch (DbUpdateException ex)
            {
                return new ResultMsg();
            }
            catch (Exception ex)
            {
                return new ResultMsg();
            }
        }

        //public async Task<ResultMsg> SaveChangeAsync()
        //{
        //    ResultMsg errorResult = null;
        //    try
        //    {
        //        var saveResult = await _dataContext.SaveChangesAsync();
        //        if (saveResult == 0)
        //            return new ResultMsg();
        //        if (saveResult >= 1)
        //            return new ResultMsg();
        //        return new ResultMsg();
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        return new ResultMsg();
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResultMsg();
        //    }
        //}

        public virtual IQueryable<T> Table => Entities;
        public virtual IQueryable<T> TableNoTracking => Entities.AsNoTracking();
        protected virtual DbSet<T> Entities => mEntities ?? (mEntities = _dataContext.Set<T>());
    }
}
