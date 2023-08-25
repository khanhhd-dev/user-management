using System.Runtime.Serialization;

namespace DigitalPlatform.UserService.Domain._base.RequestBase.Query
{
    [DataContract]
    public abstract class QuerySingleBase<TResultType> : QueryBase<TResultType>, IQuerySingleBase<TResultType>
    {
    }
}
