using DigitalPlatform.UserService.Domain._base.ResultBase;
using MediatR;

namespace DigitalPlatform.UserService.Domain._base.RequestBase
{
    public interface IRequestBase<TResultType> : IRequest<IResultBase<TResultType>>
    {
        // Validate the request.
        bool IsValid();
        //This property will contain validation/error message(s) during handle a request.
        List<string> Messages { get; set; }
        //Request context.
        RequestContextBase Context { get; set; }
    }
}
