using System.Runtime.Serialization;
using DigitalPlatform.UserService.Domain._base.ResultBase;

namespace DigitalPlatform.UserService.Domain._base.RequestBase.Query
{
    [DataContract]
    public abstract class QueryListBase<TResultType> : QueryBase<PaginatedResult<TResultType>>, IQueryListBase<TResultType>
    {
        [DataMember] public bool IsPaged { get; set; } = true;
        [DataMember] public int PageNumber { get; set; }
        [DataMember] public int PageSize { get; set; } = 10;
        [DataMember] public string OrderBy { get; set; }
        [DataMember] public string OrderByDirection { get; set; }
        [DataMember] public string SearchText { get; set; }
        public string ThenOrderBy { get; set; }
        public string ThenOrderByDirection { get; set; }
    }
}
