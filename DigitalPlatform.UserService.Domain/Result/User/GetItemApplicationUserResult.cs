using DigitalPlatform.UserService.Domain._base.ResultBase;
using DigitalPlatform.UserService.Share;

namespace DigitalPlatform.UserService.Domain.Result.User
{
    public class GetItemApplicationUserResult : BaseGetViewModel
    {
        public CommonEnum.UserType UserType { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public CommonEnum.Gender Gender { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Department { get; set; }
        public Guid? DepartmentId { get; set; }
        public string JobTitle { get; set; }
        public Guid? JobTitleId { get; set; }
        public string IdNo { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<BaseGetKeyValueViewModel> Roles { get; set; }
    }
}
