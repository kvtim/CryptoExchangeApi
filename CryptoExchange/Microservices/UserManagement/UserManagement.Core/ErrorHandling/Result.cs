using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Core.Exceptions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserManagement.Core.ErrorHandling
{
    public abstract class ResultBase
    {
        public bool Succeeded { get; }
        private Error _error;
        public Error Error {
            get
            {
                if(Succeeded)
                {
                    throw new ResultException(
                        "You attempted to access the ErrorMessage property" +
                        "for a successful result. A successful result has no ErrorMessage.");
                }

                return _error;
            }
            private set
            {
                _error = value;
            }
        }

        protected internal ResultBase(bool succeeded, Error error)
        {
            Succeeded = succeeded;
            Error = error;
        }
    }
    public class Result : ResultBase
    {
        public Result(bool succeeded, Error error) : base(succeeded, error)
        {
        }
        public static Result Ok()
        {
            return new Result(true, default(Error));
        }

        public static Result<T> Ok<T>(T value)
        {
            return new Result<T>(value, true, default(Error));
        }

        public static Result Failure(string message)
        {
            return new Result(false, new Error(message));
        }

        public static Result Failure(ErrorType errorType, string message)
        {
            return new Result(false, new Error(errorType, message));
        }
    }

    public class Result<T> : ResultBase
    {
        private readonly T _value;
        public T Value
        {
            get
            {
                if (!Succeeded)
                {
                    throw new InvalidOperationException("Cannot access the value of a failed result");
                }

                return _value;
            }
        }
        public T ValueOrDefault
        {
            get
            {
                return _value;
            }
        }

        protected internal Result(T value, bool succeeded, Error error) : base(succeeded, error)
        {
            _value = value;
        }

        public static implicit operator Result<T>(Result result)
        {
            if (result.Succeeded)
            {
                throw new ArgumentException(
                "Cannot convert a non-generic successful result to a generic one." +
                " The value would be null!",
                nameof(result));
            }

            return new Result<T>(default(T), false, result.Error);
        }

        public static implicit operator Result(Result<T> result)
        {
            return new Result(result.Succeeded, result.Error);
        }
    }
}
