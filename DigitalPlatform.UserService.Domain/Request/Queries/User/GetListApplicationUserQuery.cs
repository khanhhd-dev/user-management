using DigitalPlatform.UserService.Domain._base.RequestBase.Query;
using DigitalPlatform.UserService.Domain.Result.User;
using DigitalPlatform.UserService.Share;
using System.Runtime.Serialization;

namespace DigitalPlatform.UserService.Domain.Request.Queries.User
{
    [DataContract]
    public class GetListApplicationUserQuery : QueryListBase<GetListApplicationUserResult>
    {

        [DataMember]
        public bool? IsActive { get; set; }

        [DataMember]
        public Guid? RoleId { get; set; }

        [DataMember]
        public Guid? DepartmentId { get; set; }

        [DataMember]
        public CommonEnum.UserType? UserType { get; set; }
    }
}
