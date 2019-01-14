using System;
using System.Collections.Generic;
using System.Text;
using Hqs.IRepository;

namespace Hqs.Repository.SqlServer
{
    public class BaseRepository<TEntity>
    {
        private IDataContext _dataContext;

        public BaseRepository(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public TModel GetById<TModel>(string id)
        {
            return default(TModel);
        }


    }
}
