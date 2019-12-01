using System;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace ChelHackApi
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly IHostingEnvironment _env;
        private readonly ILogger<HttpGlobalExceptionFilter> _logger;

        public HttpGlobalExceptionFilter(IHostingEnvironment env, ILogger<HttpGlobalExceptionFilter> logger)
        {
            this._env = env;
            this._logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);

            InternalServerError(context.Exception, "An error occur.Try it again.");
            context.ExceptionHandled = true;

            void InternalServerError(Exception ex, string userMessage)
            {
                var error = new ErrorModel(ErrorCode.ApiError, userMessage);
                if (_env.IsDevelopment())
                {
                    error = new DevErrorModel(error)
                    {
                        DeveloperMessage = new
                        {
                            Message = string.Join("=>", ex.Message, ex.InnerException?.Message),
                            ex.Source,
                            ex.StackTrace
                        }
                    };
                }
                context.Result = new InternalServerErrorObjectResult(error);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }

        private class DevErrorModel : ErrorModel
        {
            public DevErrorModel(ErrorModel error)
            {
                Code = error.Code;
                Message = error.Message;
                MultipleErrors = error.MultipleErrors;
                PropertyErrors = error.PropertyErrors;
            }

            public object DeveloperMessage { get; set; }
        }
    }
}