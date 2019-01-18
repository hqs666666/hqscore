using System.ComponentModel.DataAnnotations;

namespace Hqs.Helper
{
    public enum Gender
    {
        [Display(Name = "未知")]
        Unknown = 0,

        [Display(Name = "男")]
        Male = 1,

        [Display(Name = "女")]
        Female = 2
    }

    public enum LogType
    {
        [Display(Name = "业务操作")]
        Info = 0,
        [Display(Name = "调试信息")]
        Debug = 1,
        [Display(Name = "警告信息")]
        Warning = 2,
        [Display(Name = "错误信息")]
        Error = 3
    }
}
