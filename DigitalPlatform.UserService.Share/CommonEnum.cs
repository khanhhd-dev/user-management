using System.ComponentModel;

namespace DigitalPlatform.UserService.Share
{
    public class CommonEnum
    {
        public enum UserType
        {
            [Description("Cms")]
            Cms = 1,
            [Description("Khách hàng")]
            Customer
        }

        public enum Gender
        {
            [Description("Nam")]
            Male = 1,

            [Description("Nữ")]
            Female
        }
        public enum RoleType
        {
            [Description("Full")]
            FullAccess = 1,

            [Description("Cms")]
            Cms,

            [Description("Khách hàng")]
            Customer
        }
    }
}
