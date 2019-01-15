using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hqs.Model;

namespace Hqs.IRepository
{
    public interface IBaseRepository<T> where T : AbstractEntity
    {
        T GetById(object id);
        Task<T> GetByIdAsync(object id);


        IQueryable<T> Table { get; }
        IQueryable<T> TableNoTracking { get; }
    }
}
