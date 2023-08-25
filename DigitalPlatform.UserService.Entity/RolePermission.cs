using DigitalPlatform.UserService.Entity.Identity;

namespace DigitalPlatform.UserService.Entity
{
    public class RolePermission
    {
        public Guid PermissionId { get; set; }
        public virtual Permission Permission { get; set; }

        public Guid RoleId { get; set; }
        public virtual ApplicationRole Role { get; set; }
    }
}
