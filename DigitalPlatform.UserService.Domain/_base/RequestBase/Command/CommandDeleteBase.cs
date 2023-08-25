using System.Runtime.Serialization;

namespace DigitalPlatform.UserService.Domain._base.RequestBase.Command
{
    [DataContract]
    public class CommandDeleteBase : CommandBase<int>, ICommandDeleteBase
    {
        [DataMember]
        public Guid Id { get; set; }
    }
}
