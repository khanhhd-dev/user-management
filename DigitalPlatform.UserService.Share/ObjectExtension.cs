namespace DigitalPlatform.UserService.Share
{
    public static class ObjectExtension
    {
        public static string EmptyIfNull(this object value)
        {
            if (value == null)
                return "";
            return value.ToString();
        }

        public static bool IsNumber(this object val)
        {
            return val is sbyte
                   || val is byte
                   || val is short
                   || val is ushort
                   || val is int
                   || val is uint
                   || val is long
                   || val is ulong
                   || val is float
                   || val is double
                   || val is decimal;
        }

        public static bool IsDateTime(this object val)
        {
            return val is DateTime;
        }

        public static string ToStringWithFormat(this object val)
        {
            if (val != null)
                return IsNumber(val) ? $"{val:#,##}" : IsDateTime(val) ? $"{val:dd/MM/yyyy}" : val.ToString();
            return string.Empty;
        }
    }
}
