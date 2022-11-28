using PS.Common.Models.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS.Common.Utility
{
    public static class CommonFunctions
    {
        public static GenericError GenerateError(string message, Exception exception = null)
        {
            var guid = new Guid().ToString();
            GenericError error = new(guid, message, exception != null ? exception.Message : "");

            return error;
        }
    }
}
