using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hqs.Dto.ResultMsg;
using Hqs.Model;

namespace Hqs.IRepository
{
    public interface IBaseRepository<T> where T : AbstractEntity
    {
        T GetById(object id);
        Task<T> GetByIdAsync(object id);
        ResultMsg Insert(T entity);
        //Task<ResultMsg> InsertAsync(T entity);
        ResultMsg SaveChanges();
       // Task<ResultMsg> SaveChangeAsync();

        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }
    }
}
