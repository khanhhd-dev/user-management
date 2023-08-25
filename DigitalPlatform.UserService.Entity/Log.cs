using DigitalPlatform.UserService.Entity._base;
using DigitalPlatform.UserService.Share.Logging;

namespace DigitalPlatform.UserService.Entity
{
    public class Log : EntityBaseNoAudit
    {
        /// <summary>
        /// Gets or sets the log level identifier
        /// </summary>
        public LogLevel LogLevelId { get; set; }

        /// <summary>
        /// Gets or sets the short message
        /// </summary>
        public string ShortMessage { get; set; }

        /// <summary>
        /// Gets or sets the full exception
        /// </summary>
        public string FullMessage { get; set; }

        public Guid? RequestId { get; set; }

        public string RequestContent { get; set; }

        ///// <summary>
        ///// Gets or sets the IP address
        ///// </summary>
        //public string IpAddress { get; set; }

        /// <summary>
        /// Gets or sets the API URL
        /// </summary>
        public string ApiUrl { get; set; }

        public Guid? UserId { get; set; }

        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the date and time of instance creation
        /// </summary>
        public DateTime CreatedOnUtc { get; set; }
    }
}
