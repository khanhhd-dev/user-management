using DigitalPlatform.UserService.Entity.Identity;
using DigitalPlatform.UserService.Share;
using Microsoft.AspNetCore.Identity;
using static DigitalPlatform.UserService.Share.CommonEnum;

namespace DigitalPlatform.UserService.Database.Seeder
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<ApplicationRole> roleManager)
        {
            await CreateRoleByNameAsync(roleManager, CommonConstants.RoleSystemAdministrator);
        }

        private static async Task CreateRoleByNameAsync(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            if (roleManager.Roles.All(c => c.NormalizedName.ToUpper() != roleName.ToString().ToUpper()))
            {
                var role = new ApplicationRole
                {
                    Id = Guid.NewGuid(),
                    Name = roleName,
                    NormalizedName = roleName.ToUpper(),
                    Description = roleName.GetDescription(),
                    RoleType = RoleType.FullAccess
                };

                await roleManager.CreateAsync(role);
            }
        }
    }
}
