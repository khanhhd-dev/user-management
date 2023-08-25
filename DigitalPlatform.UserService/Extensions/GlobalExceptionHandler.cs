using DigitalPlatform.UserService.Domain._base.ResultBase;
using Microsoft.AspNetCore.Diagnostics;

namespace DigitalPlatform.UserService.Api.Extensions
{
    public static class GlobalExceptionHandler
    {
        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        var result = new ResultBase<bool>(500, error.Error.Message);
                        await context.Response.WriteAsJsonAsync(result);
                    }
                });
            });
        }
    }
}
