using DigitalPlatform.UserService.Domain._base.ResultBase;
using DigitalPlatform.UserService.Share;

namespace DigitalPlatform.UserService.Domain.Result.User
{
    public class GetListApplicationUserResult : BaseGetViewModel
    {
        public CommonEnum.UserType UserType { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
        public string IdNo { get; set; }
        public string RoleNames { get; set; }
        public string? Department { get; set; }
        public IEnumerable<string> Role { get; set; }
        public bool IsActive { get; set; }
    }
}
