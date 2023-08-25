using System.Runtime.Serialization;
using DigitalPlatform.UserService.Entity._base;

namespace DigitalPlatform.UserService.Domain._base.RequestBase.Command
{
    [DataContract]
    public class CommandCreateBase<TModel> : CommandBase<int>, ICommandCreateBase<TModel>
        where TModel : IEntityBase
    {
        [DataMember]
        public TModel Payload { get; set; }
    }
}
