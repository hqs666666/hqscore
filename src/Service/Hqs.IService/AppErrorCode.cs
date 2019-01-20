using System.ComponentModel.DataAnnotations;

namespace Hqs.IService
{
    public enum AppErrorCode
    {
        [Display(Name = "成功")]
        Success = 0,

        [Display(Name = "失败")]
        Exception = 1,
    }
}
