using System;
using System.Collections.Generic;
using System.Text;

namespace Hqs.Model
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; set; }
    }
}
