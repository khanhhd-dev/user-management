namespace DigitalPlatform.UserService.Share.Logging
{
    public interface ILogger
    {
        Task InsertLog(
            LogLevel logLevel,
            Guid? requestId,
            string requestContent,
            string shortMessage,
            string fullMessage = "");
    }
}
