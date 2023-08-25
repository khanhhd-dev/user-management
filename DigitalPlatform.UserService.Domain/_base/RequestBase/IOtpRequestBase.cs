namespace DigitalPlatform.UserService.Domain._base.RequestBase
{
    public interface IOtpRequestBase
    {
        OtpRequestModelBase OtpRequest { get; set; }
    }

    public class OtpRequestModelBase
    {
        public string ConcurrencyStamp { get; set; }
        public string PassCode { get; set; }
        public int OtpType { get; set; }
    }
}
