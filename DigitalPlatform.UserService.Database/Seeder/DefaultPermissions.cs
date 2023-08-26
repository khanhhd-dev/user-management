using DigitalPlatform.UserService.Entity;
using DigitalPlatform.UserService.Share;

namespace DigitalPlatform.UserService.Database.Seeder
{
    public static class DefaultPermissions
    {
        public static async Task SeedAsync(DatabaseContext databaseContext)
        {
            if (databaseContext.Permissions.All(c => c.Name != CommonConstants.RoleSystemAdministrator))
            {
                databaseContext.Permissions.Add(new Permission
                {
                    Name = CommonConstants.RoleSystemAdministrator,
                    Endpoint = string.Empty,
                    Description = CommonConstants.RoleSystemAdministrator,
                    IsDeleted = false,
                    InsertedAt = DateTime.Now,
                    InsertedById = CommonConstants.SystemId,
                    UpdatedAt = DateTime.Now,
                    UpdatedById = CommonConstants.SystemId
                });
                await databaseContext.SaveChangesAsync();
            }
        }
    }
}