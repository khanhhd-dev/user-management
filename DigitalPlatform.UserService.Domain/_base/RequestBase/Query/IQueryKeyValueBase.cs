namespace DigitalPlatform.UserService.Domain._base.RequestBase.Query
{
    public interface IQueryKeyValueBase<TResultType> : IQueryListBase<TResultType>
    {
        List<Guid> ExcludedIds { get; set; }
        string Keyword { get; set; }
    }
}
