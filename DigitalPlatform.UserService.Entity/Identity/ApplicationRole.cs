using System.ComponentModel.DataAnnotations.Schema;
using DigitalPlatform.UserService.Entity._base;
using DigitalPlatform.UserService.Share;
using Microsoft.AspNetCore.Identity;

namespace DigitalPlatform.UserService.Entity.Identity
{
    [Table("ApplicationRole")]
    public class ApplicationRole : IdentityRole<Guid>, IEntityBase
    {
        public ApplicationRole()
        {
            IsActive = true;
            IsDeleted = false;
            InsertedById = CommonConstants.SystemId;
            InsertedAt = DateTime.UtcNow;
            UpdatedById = CommonConstants.SystemId;
            UpdatedAt = DateTime.UtcNow;
        }


        [Column(TypeName = "varchar(255)")] 
        public string RoleType { get; set; }


        [Column(TypeName = "varchar(255)")] 
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public Guid InsertedById { get; set; }

        public string InsertedBy { get; set; }

        public DateTime InsertedAt { get; set; }

        public Guid UpdatedById { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
