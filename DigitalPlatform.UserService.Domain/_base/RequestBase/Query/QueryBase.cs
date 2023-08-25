using System.Runtime.Serialization;

namespace DigitalPlatform.UserService.Domain._base.RequestBase.Query
{
    [DataContract]
    public abstract class QueryBase<TResultType> : RequestBase<TResultType>, IQueryBase<TResultType>
    {

    }
}
