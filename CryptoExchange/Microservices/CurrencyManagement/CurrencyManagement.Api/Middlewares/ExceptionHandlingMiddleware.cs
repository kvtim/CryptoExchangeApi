using CurrencyManagement.Data.Dtos.Error;
using CurrencyManagement.Data.Exceptions;
using System.Net;

namespace CurrencyManagement.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch(KeyNotFoundException ex)
            {
                await HandleExceptionAsync(httpContext, 
                    ex.Message,
                    HttpStatusCode.NotFound,
                    "Entity not found");
            }
            catch(CurrencyPriceLessThanZeroException ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    HttpStatusCode.BadRequest,
                    "Currency price can't be less than 0");
            }
            catch(ArgumentException ex)
            {
                await HandleExceptionAsync(httpContext, 
                    ex.Message,
                    HttpStatusCode.BadRequest,
                    "Data is not valid");
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext,
                    ex.Message,
                    HttpStatusCode.InternalServerError,
                    "Internal server error");
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, string exMessage, 
            HttpStatusCode httpStatusCode, string message)
        {
            _logger.LogError(exMessage);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)httpStatusCode;

            await context.Response.WriteAsync(new ErrorDto()
            {
                StatusCode = context.Response.StatusCode,
                Message = message
            }.ToString());
        }
    }
}
