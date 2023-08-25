using DigitalPlatform.UserService.Entity._base;

namespace DigitalPlatform.UserService.Domain._base.RequestBase.Command
{
    public interface ICommandUpdateBase<TModel> : ICommandBase<int>
        where TModel : class, IEntityBase
    {
        TModel Payload { get; set; }
    }
}
