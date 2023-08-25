using System.Runtime.Serialization;

namespace DigitalPlatform.UserService.Domain._base.RequestBase.Query
{
    [DataContract]
    public abstract class QueryKeyValueBase<TResultType> : QueryListBase<TResultType>, IQueryKeyValueBase<TResultType>
    {
        [DataMember]
        public List<Guid> ExcludedIds { get; set; }

        [DataMember]
        public string Keyword { get; set; }
    }
}
