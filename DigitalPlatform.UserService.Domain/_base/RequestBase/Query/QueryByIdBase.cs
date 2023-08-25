using System.Runtime.Serialization;

namespace DigitalPlatform.UserService.Domain._base.RequestBase.Query
{
    [DataContract]
    public class QueryByIdBase<TResultType> : QuerySingleBase<TResultType>, IQueryByIdBase<TResultType>
    {
        [DataMember]
        public Guid Id { get; set; }
    }
}
