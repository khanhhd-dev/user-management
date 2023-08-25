using DigitalPlatform.UserService.Entity._base;
using DigitalPlatform.UserService.Share;

namespace DigitalPlatform.UserService.Entity
{
    public class Permission : MdEntityBase
    {
        public Permission()
        {
            IsActive = true;
            IsDeleted = false;
            InsertedById = CommonConstants.SystemId;
            InsertedAt = DateTime.UtcNow;
            UpdatedById = CommonConstants.SystemId;
            UpdatedAt = DateTime.UtcNow;
        }

        public string Endpoint { get; set; }
    }
}
