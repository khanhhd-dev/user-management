using System.Text.Json.Serialization;
using DigitalPlatform.UserService.Entity._base;
using DigitalPlatform.UserService.Entity.Identity;
using DigitalPlatform.UserService.Share;

namespace DigitalPlatform.UserService.Entity
{
    public class JobTitle : MdEntityBase
    {
        [JsonIgnore]
        public virtual ICollection<ApplicationUser> Users { get; set; }

        public CommonEnum.UserType UserType { get; set; }
    }
}
