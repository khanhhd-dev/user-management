using DigitalPlatform.UserService.Database;
using Microsoft.AspNetCore.Identity;

namespace DigitalPlatform.UserService.Api.Middleware
{
    public class PermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public PermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Get required services
            var userManager = context.RequestServices.GetRequiredService<UserManager<IdentityUser>>();
            var dbContext = context.RequestServices.GetRequiredService<DatabaseContext>();

            // Your logic to extract API endpoint or route here
            var requestedEndpoint = context.Request.Path.Value;

            var user = await userManager.GetUserAsync(context.User);
            if (user != null)
            {
                var roleIds = await userManager.GetRolesAsync(user);

                var permissionExists = dbContext.RolePermissions
                    .Any(rp => roleIds.Contains(rp.RoleId.ToString()) && rp.Permission.Endpoint == requestedEndpoint);

                if (!permissionExists)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return;
                }
            }

            await _next(context);
        }
    }
}
