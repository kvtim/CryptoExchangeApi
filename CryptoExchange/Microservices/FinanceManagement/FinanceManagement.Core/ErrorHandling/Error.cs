using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceManagement.Core.ErrorHandling
{
    public class Error
    {
        public ErrorType Type { get; set; }
        public string Message { get; set; }

        public Error(ErrorType errorType, string message)
        {
            Type = errorType;
            Message = message;
        }

        public Error(string message) : this(ErrorType.InternalServerError, message)
        {

        }
    }
}
