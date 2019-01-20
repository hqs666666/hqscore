using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hqs.IService.Logs
{
    public interface ILogService
    {
        void LogDebug(string message);
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogFail(string message);

        void LogDebug(string message, Exception ex);
        void LogInfo(string message, Exception ex);
        void LogWarning(string message, Exception ex);
        void LogError(string message, Exception ex);
        void LogFail(string message, Exception ex);

        Task LogDebugAsync(string message);
        Task LogInfoAsync(string message);
        Task LogWarningAsync(string message);
        Task LogErrorAsync(string message);
        Task LogFailAsync(string message);

        Task LogDebugAsync(string message, Exception ex);
        Task LogInfoAsync(string message, Exception ex);
        Task LogWarningAsync(string message, Exception ex);
        Task LogErrorAsync(string message, Exception ex);
        Task LogFailAsync(string message, Exception ex);
    }
}
