using System.ComponentModel.DataAnnotations.Schema;
using DigitalPlatform.UserService.Entity._base;
using DigitalPlatform.UserService.Share;
using Microsoft.AspNetCore.Identity;

namespace DigitalPlatform.UserService.Entity.Identity
{
    [Table("ApplicationUser")]
    public class ApplicationUser : IdentityUser<Guid>, IEntityBase
    {
        public ApplicationUser()
        {
            IsActive = true;
            IsDeleted = false;
            InsertedById = CommonConstants.SystemId;
            InsertedAt = DateTime.UtcNow;
            UpdatedById = CommonConstants.SystemId;
            UpdatedAt = DateTime.UtcNow;
        }

        public CommonEnum.UserType UserType { get; set; }
        [Column(TypeName = "varchar(255)")] public string IdNo { get; set; }
        [Column(TypeName = "varchar(255)")] public string FullName { get; set; }
        public CommonEnum.Gender Gender { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public Guid InsertedById { get; set; }

        public string InsertedBy { get; set; }

        public DateTime InsertedAt { get; set; }

        public Guid UpdatedById { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }

        public Guid? JobTitleId { get; set; }
        public virtual JobTitle JobTitle { get; set; }

        public Guid? DepartmentId { get; set; }
        public virtual Department Department { get; set; }
    }
}
