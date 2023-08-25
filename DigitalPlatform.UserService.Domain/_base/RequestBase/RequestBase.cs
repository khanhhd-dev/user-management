using System.Runtime.Serialization;

namespace DigitalPlatform.UserService.Domain._base.RequestBase
{
    [DataContract]
    public abstract class RequestBase<TResultType> : IRequestBase<TResultType>
    {
        protected RequestBase()
        {
            Context = new RequestContextBase();
            Messages = new List<string>();
        }

        //Request context.
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public RequestContextBase Context { get; set; }

        //This property will contain validation/error message(s) during handle a request.
        [System.Text.Json.Serialization.JsonIgnore]
        [Newtonsoft.Json.JsonIgnore]
        public List<string> Messages { get; set; }

        // Validate the request.
        public virtual bool IsValid()
        {
            return true;
        }
    }
}
