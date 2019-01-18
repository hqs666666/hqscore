namespace Hqs.Dto.ResultMsg
{
    public class ApiResultMsg
    {
        public int StatusCode { get; set; }
        public object Data { get; set; }
        public string Message { get; set; }
    }

    public class ResultMsg : ApiResultMsg
    {
        public bool Result => StatusCode == 0;
    }
}
