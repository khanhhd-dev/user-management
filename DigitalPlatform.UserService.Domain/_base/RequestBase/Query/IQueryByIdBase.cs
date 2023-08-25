namespace DigitalPlatform.UserService.Domain._base.RequestBase.Query
{
    public interface IQueryByIdBase<TResultType> : IQuerySingleBase<TResultType>
    {
        Guid Id { get; set; }
    }
}
