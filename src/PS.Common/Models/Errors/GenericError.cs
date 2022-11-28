using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Common.Models.Errors
{
    public class GenericError
    {
        public GenericError(string code, string message, string techError = "")
        {
            Code = code;
            Message = message;
            TechnicalError = techError;
        }
        public string Code { get; set; }
        public string Message { get; set; }
        public string TechnicalError { get; set; }
    }
}
