namespace DigitalPlatform.UserService.Entity._base
{
    public class MdEntityBase : EntityBase
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
