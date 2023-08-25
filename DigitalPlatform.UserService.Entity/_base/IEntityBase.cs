using System.ComponentModel.DataAnnotations;

namespace DigitalPlatform.UserService.Entity._base
{
    public interface IEntityBase
    {
        [Key]
        Guid Id { get; set; }

        bool IsDeleted { get; set; }

        Guid InsertedById { get; set; }

        string InsertedBy { get; set; }

        DateTime InsertedAt { get; set; }

        Guid UpdatedById { get; set; }

        string UpdatedBy { get; set; }

        DateTime UpdatedAt { get; set; }
    }
}
