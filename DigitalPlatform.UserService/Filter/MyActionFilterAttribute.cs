using DigitalPlatform.UserService.Domain._base.ResultBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DigitalPlatform.UserService.Api.Filter
{
    public class MyActionFilterAttribute : IActionFilter
    {

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!filterContext.ModelState.IsValid)
            {
                var result = new OkObjectResult(new ResultBase<int>(400,
                    filterContext.ModelState.Values
                        .SelectMany(c => c.Errors)
                        .Select(c => c.ErrorMessage)
                        .ToArray()));
                filterContext.Result = result;
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}
