using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Common.Exceptions
{
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException(string message) : base(message)
        { 
        }
        public DataNotFoundException(string message, Exception inner): base(message, inner)
        {
        }
    }
}
