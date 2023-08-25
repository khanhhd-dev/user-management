namespace DigitalPlatform.UserService.Share
{
    public static class DateTimeExtension
    {
        public static bool IsValid(this DateTime? dateTime)
        {
            if (dateTime is null) return false;
            return true;
        }
    }
}
