using DigitalPlatform.UserService.Entity.Identity;
using DigitalPlatform.UserService.Share;
using Microsoft.AspNetCore.Identity;

namespace DigitalPlatform.UserService.Database.Seeder
{
    public static class DefaultUsers
    {
        public static async Task SeedApplicationUserAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser
            {
                Id = CommonConstants.SystemId,
                UserName = CommonConstants.SystemUser,
                NormalizedUserName = CommonConstants.SystemUser,
                Email = CommonConstants.SystemUser,
                NormalizedEmail = CommonConstants.SystemUser,
                SecurityStamp = Guid.NewGuid().ToString(),
                EmailConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, CommonConstants.DefaultPassword);
                    await userManager.AddToRoleAsync(defaultUser, CommonConstants.RoleSystemAdministrator);
                }
            }
        }
    }
}
