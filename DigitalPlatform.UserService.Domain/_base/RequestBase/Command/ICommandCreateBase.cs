using DigitalPlatform.UserService.Entity._base;

namespace DigitalPlatform.UserService.Domain._base.RequestBase.Command
{
    public interface ICommandCreateBase<TModel> : ICommandBase<int>
        where TModel : IEntityBase
    {
        TModel Payload { get; set; }
    }
}
