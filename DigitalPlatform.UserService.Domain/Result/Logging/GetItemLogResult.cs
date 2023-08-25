#nullable enable
using DigitalPlatform.UserService.Domain._base.ResultBase;
using DigitalPlatform.UserService.Share.Logging;

namespace DigitalPlatform.UserService.Domain.Result.Logging
{
    public class GetItemLogResult : NoAuditBaseGetViewModel
    {
        /// <summary>
        /// Gets or sets the log level identifier
        /// </summary>
        public LogLevel LogLevelId { get; set; }

        public string? LogLevelName { get; set; }

        /// <summary>
        /// Gets or sets the short message
        /// </summary>
        public string? ShortMessage { get; set; }

        /// <summary>
        /// Gets or sets the full exception
        /// </summary>
        public object? FullMessage { get; set; }

        public Guid? RequestId { get; set; }

        public object? RequestContent { get; set; }

        ///// <summary>
        ///// Gets or sets the IP address
        ///// </summary>
        //public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the API URL
        /// </summary>
        public string? ApiUrl { get; set; }

        public Guid? UserId { get; set; }

        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }
    }
}
