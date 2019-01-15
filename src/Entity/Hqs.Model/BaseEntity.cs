using System;
using System.Collections.Generic;
using System.Text;

namespace Hqs.Model
{
    public abstract class BaseEntity<TKey> : AbstractEntity
    {
        public TKey Id { get; set; }
    }

    public abstract class AbstractEntity
    {
    }
}
