#nullable enable
using DigitalPlatform.UserService.Domain._base.RequestBase.Query;
using DigitalPlatform.UserService.Domain.Result.Logging;
using DigitalPlatform.UserService.Share.Logging;
using System.Runtime.Serialization;

namespace DigitalPlatform.UserService.Domain.Request.Queries.Logging
{
    [DataContract]
    public class GetListLogQuery : QueryListBase<GetListLogResult>
    {
        [DataMember]
        public LogLevel? LogLevelId { get; set; }

        [DataMember]
        public DateTime? FromDate { get; set; }

        [DataMember]
        public DateTime? ToDate { get; set; }

        [DataMember]
        public string? ShortMessage { get; set; }

        [DataMember]
        public string? FullMessage { get; set; }

        [DataMember]
        public Guid? RequestId { get; set; }

        [DataMember]
        public string? RequestContent { get; set; }

        [DataMember]
        public string? ApiUrl { get; set; }

        [DataMember]
        public Guid? UserId { get; set; }

        [DataMember]
        public string? Username { get; set; }
    }
}
