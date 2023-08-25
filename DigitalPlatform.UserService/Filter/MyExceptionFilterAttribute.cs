using DigitalPlatform.UserService.Domain._base.ResultBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace DigitalPlatform.UserService.Api.Filter
{
    public class MyExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// The function for catching action exception
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            context.Result = new OkObjectResult(new ResultBase<bool>((int)HttpStatusCode.BadRequest, "Request invalid."));

            //var message = context.Exception.InnerException != null
            //    ? context.Exception.InnerException.Message
            //    : context.Exception.Message;

            //var logger = (ILogger)context.HttpContext.RequestServices.GetService(typeof(ILogger));
            //logger.InsertLog(LogLevel.Error, message, context.Exception.Message);
        }
    }
}
