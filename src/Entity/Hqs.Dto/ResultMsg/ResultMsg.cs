using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Hqs.Dto.ResultMsg
{
    public class ApiResultMsg
    {
        public int StatusCode { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
    }
}
