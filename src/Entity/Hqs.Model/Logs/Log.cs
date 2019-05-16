using System;

namespace Hqs.Model.Logs
{
    public class Log : BaseEntity<int>
    {
        public DateTime CTime { get; set; }
        public string CreatedBy { get; set; }
        public int LogType { get; set; }
        public string LogTypeName { get; set; }
        public string AccessUrl { get; set; }
        public string IpAddress { get; set; }
        public string ModuleName { get; set; }
        public string ActionName { get; set; }
        public string Description { get; set; }
        public string BeforeChange { get; set; }
        public string AfterChange { get; set; }
        public string Remarks { get; set; }

        public Log()
        {
            CTime = DateTime.Now;
        }
    }
}
