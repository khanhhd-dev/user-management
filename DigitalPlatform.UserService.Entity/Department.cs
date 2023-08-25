using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DigitalPlatform.UserService.Entity._base;
using DigitalPlatform.UserService.Entity.Identity;
using DigitalPlatform.UserService.Share;

namespace DigitalPlatform.UserService.Entity
{
    public class Department : MdEntityBase
    {
        public Department()
        {
            IsActive = true;
            IsDeleted = false;
            InsertedById = CommonConstants.SystemId;
            InsertedAt = DateTime.UtcNow;
            UpdatedById = CommonConstants.SystemId;
            UpdatedAt = DateTime.UtcNow;
        }

        public Guid? ParentId { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(ParentId))]
        public virtual Department ParentDepartment { get; set; }

        [JsonIgnore]
        public virtual ICollection<Department> ChildrenDepartments { get; set; }

        [JsonIgnore]
        public virtual ICollection<ApplicationUser> Users { get; set; }
    }
}
