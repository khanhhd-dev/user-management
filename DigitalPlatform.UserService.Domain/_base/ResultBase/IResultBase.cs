namespace DigitalPlatform.UserService.Domain._base.ResultBase
{
    public interface IResultBase<T>
    {
        int StatusCode { get; set; }
        bool Success { get; set; }
        List<string> ErrorMessages { get; set; }
        T Result { get; set; }
        Guid? TraceId { get; set; }
    }
}
