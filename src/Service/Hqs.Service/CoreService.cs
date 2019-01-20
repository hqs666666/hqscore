using System;
using System.Reflection;
using System.Threading.Tasks;
using Hqs.Dto.ResultMsg;
using Hqs.Helper;
using Hqs.IService;
using Hqs.IService.Logs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Hqs.Service
{
    public abstract class CoreService
    {
        #region Constants

        private const BindingFlags BindingFlags = System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase;

        #endregion

        protected readonly DbContext DataContext;

        protected CoreService(DbContext dataContext)
        {
            DataContext = dataContext;
        }

        protected ResultMsg SaveChanges()
        {
            try
            {
                var lSavedResult = DataContext.SaveChanges();
                if (lSavedResult >= 1)
                    return CreateResultMsg("Success");
                if (lSavedResult == 0)
                    return CreateResultMsg("DataNotChange");
            }
            catch (DbUpdateConcurrencyException e)
            {
                Logger.LogFail(e.Message,e);
                return CreateErrorMsg(e.Message);
            }
            catch (DbUpdateException e)
            {
                Logger.LogFail(e.Message, e);
                return CreateErrorMsg(e.Message);
            }
            catch (Exception e)
            {
                Logger.LogFail(e.Message, e);
                return CreateErrorMsg(e.Message);
            }
            return CreateErrorMsg("Fail");
        }

        protected async Task<ResultMsg> SaveChangesAsync()
        {
            try
            {
                var lSavedResult = await DataContext.SaveChangesAsync();
                if (lSavedResult >= 1)
                    return CreateResultMsg("Success");
                if (lSavedResult == 0)
                    return CreateResultMsg("DataNotChange");
            }
            catch (DbUpdateConcurrencyException e)
            {
                await Logger.LogFailAsync(e.Message, e);
                return CreateErrorMsg(e.Message);
            }
            catch (DbUpdateException e)
            {
                await Logger.LogFailAsync(e.Message, e);
                return CreateErrorMsg(e.Message);
            }
            catch (Exception e)
            {
                await Logger.LogFailAsync(e.Message, e);
                return CreateErrorMsg(e.Message);
            }
            return CreateErrorMsg("Fail");
        }

        protected TModel Create<TModel>()
        {
            var lType = typeof(TModel);
            var lObject = Activator.CreateInstance(lType);

            var lPropInfo = lType.GetProperty("Id", BindingFlags);
            if (null != lPropInfo && lPropInfo.PropertyType == typeof(string))
                lPropInfo.SetValue(lObject, Guid.NewGuid().ToString("N"));

            var lNowTime = DateTime.Now;
            lPropInfo = lType.GetProperty("CTime", BindingFlags);
            if (null != lPropInfo && lPropInfo.PropertyType == typeof(DateTime))
                lPropInfo.SetValue(lObject, lNowTime);

            lPropInfo = lType.GetProperty("MTime", BindingFlags);
            if (null != lPropInfo && lPropInfo.PropertyType == typeof(DateTime))
                lPropInfo.SetValue(lObject, lNowTime);

            lPropInfo = lType.GetProperty("CreateBy", BindingFlags);
            if (null != lPropInfo && lPropInfo.PropertyType == typeof(string))
                lPropInfo.SetValue(lObject, UserId);

            lPropInfo = lType.GetProperty("ModifyBy", BindingFlags);
            if (null != lPropInfo && lPropInfo.PropertyType == typeof(string))
                lPropInfo.SetValue(lObject, UserId);

            return (TModel)lObject;
        }

        protected static ResultMsg CreateResultMsg(string message = null)
        {
            var lMsg = new ResultMsg
            {
                StatusCode = (int)AppErrorCode.Success,
                Message = message
            };
            return lMsg;
        }

        protected static ResultMsg CreateErrorMsg(string message = null)
        {
            var lMsg = new ResultMsg
            {
                StatusCode = (int)AppErrorCode.Exception,
                Message = message
            };
            return lMsg;
        }

        protected string UserId => "-1";

        protected ILogService Logger => LogService();

        private ILogService LogService()
        {
            var provider = DIHelper.ServiceProvider.CreateScope();
            return provider.ServiceProvider.GetService<ILogService>();
        }
    }
}
