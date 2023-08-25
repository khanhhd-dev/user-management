using DigitalPlatform.UserService.Domain._base.ResultBase;

namespace DigitalPlatform.UserService.Domain._base.RequestBase.Query
{
    public interface IQueryListBase<TResultType> : IQueryBase<PaginatedResult<TResultType>>
    {
        bool IsPaged { get; set; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
        string OrderBy { get; set; }
        string OrderByDirection { get; set; }
        string ThenOrderBy { get; set; }
        string ThenOrderByDirection { get; set; }
        string SearchText { get; set; }
    }
}
