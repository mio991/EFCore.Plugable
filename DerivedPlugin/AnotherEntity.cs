using BasePlugin;
using System;
using System.Collections.Generic;
using System.Text;

namespace DerivedPlugin
{
    class AnotherEntity
    {
        public long Id { get; set; }

        public long BaseEntityId { get; set; }

        public virtual BaseEntity BaseEntity { get; set; }
    }
}
