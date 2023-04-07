using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Core.Exceptions
{
    public class ResultException : Exception
    {
        public ResultException(string message)
            : base(message) { }
    }
}
