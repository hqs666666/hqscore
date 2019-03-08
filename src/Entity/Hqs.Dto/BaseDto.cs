using System.Collections.Generic;

namespace Hqs.Dto
{
    public abstract class BaseDto
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int PerPage => (Page - 1) * PageSize;
    }

    public class PageDto<T> where T : class 
    {
        public int Count { get; set; }
        public List<T> Item { get; set; }
    }
}
