using System.Runtime.Serialization;
using DigitalPlatform.UserService.Entity._base;

namespace DigitalPlatform.UserService.Domain._base.RequestBase.Command
{
    [DataContract]
    public class CommandUpdateBase<TModel> : CommandBase<int>, ICommandUpdateBase<TModel>
        where TModel : class, IEntityBase
    {
        [DataMember]
       public TModel Payload { get; set; }
    }
}
