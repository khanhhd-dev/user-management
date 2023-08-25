namespace DigitalPlatform.UserService.Domain._base.RequestBase.Command
{
    public interface ICommandDeleteBase : ICommandBase<int> 
    {
         Guid Id { get; set; }
    }
}