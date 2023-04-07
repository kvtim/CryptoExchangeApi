using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Core.ErrorHandling
{
    public abstract class ApiResultBase
    {
        public readonly bool isSuccess;
        public readonly Error error;

        protected internal ApiResultBase(bool isSuccess, Error error)
        {
            this.isSuccess = isSuccess;
            this.error = error;
        }
    }

    public class ApiResult : ApiResultBase, IConvertToActionResult
    {
        public ApiResult(bool isSuccess = true, Error error = default(Error)) : base(isSuccess, error)
        {
        }

        public static ApiResult Ok()
        {
            return new ApiResult(true);
        }

        public static ApiResult<T> Ok<T>(Result<T> result)
        {
            return new ApiResult<T>(result, true);
        }

        public static ApiResult<T> Ok<T>(T value)
        {
            return new ApiResult<T>(
                new Result<T>(value, true, default(Error))
                );
        }

        public static ApiResult Failure(Error error)
        {
            return new ApiResult(false, error);
        }

        public IActionResult Convert()
        {
            if (!isSuccess)
            {
                return new ObjectResult(error.Message)
                {
                    StatusCode = (int)error.Type
                };
            }

            return new ObjectResult(default(ObjectResult))
            {
                StatusCode = 200
            };
        }
    }


    public class ApiResult<T> : ApiResultBase, IConvertToActionResult
    {
        public readonly Result<T> resultBody;

        public ApiResult(Result<T> result, bool isSuccess = true, Error error = default(Error)) :
            base(isSuccess, error)
        {
            resultBody = result;
        }

        public IActionResult Convert()
        {
            if (!isSuccess)
            {
                return new ObjectResult(error.Message)
                {
                    StatusCode = (int)error.Type
                };
            }

            return new ObjectResult(resultBody.ValueOrDefault)
            {
                StatusCode = 200
            };
        }

        public static implicit operator ApiResult(ApiResult<T> result)
        {
            return new ApiResult(result.isSuccess, result.resultBody.Error);
        }

        public static implicit operator ApiResult<T>(ApiResult result)
        {
            if (result.isSuccess)
            {
                throw new ArgumentException(
                    "Cannot convert a non-generic successful result to a generic one." +
                    " The value would be null!",
                    nameof(result));
            }

            return new ApiResult<T>(default(Result<T>), false, result.error);
        }
    }
}
