using System.Runtime.Serialization;
using DigitalPlatform.UserService.Share;

namespace DigitalPlatform.UserService.Domain._base.RequestBase
{
    [DataContract]
    public class RequestContextBase
    {
        public RequestContextBase()
        {
            UserId = CommonConstants.SystemId;
            RequestId = Guid.NewGuid();
        }

        [DataMember]
        public Guid RequestId { get; set; }
        [DataMember]
        public Guid UserId { get; set; }
        [DataMember]
        public string Language { get; set; }
        [DataMember]
        public string LocalTimeZone { get; set; }
    }
}
