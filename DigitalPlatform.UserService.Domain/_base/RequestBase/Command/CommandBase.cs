using System.Runtime.Serialization;

namespace DigitalPlatform.UserService.Domain._base.RequestBase.Command
{
    [DataContract]
    public abstract class CommandBase<TResultType> : RequestBase<TResultType>, ICommandBase<TResultType>
    {
    }
}
