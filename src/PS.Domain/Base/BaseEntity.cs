using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Domain.Base
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            UpdatedAt = DateTime.Now;
            CreatedAt = DateTime.Now;
        }
        public int Id { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
