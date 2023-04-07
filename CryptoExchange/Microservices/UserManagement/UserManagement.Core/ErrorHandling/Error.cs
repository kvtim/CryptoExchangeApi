using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Core.ErrorHandling
{
    public class Error
    {
        public ErrorType Type;
        public string Message;

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
