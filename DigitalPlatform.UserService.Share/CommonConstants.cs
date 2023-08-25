namespace DigitalPlatform.UserService.Share
{
    public static class CommonConstants
    {
        public static readonly Guid SystemId = new("00000000-0000-0000-0000-000000000001");
        public static readonly string SystemUser = "system@gmail.com";
        public const string DefaultPassword = "Abcde12345-";

        public struct JwtRegisteredClaimNames
        {
            public const string UserInfo = "UserInfo";
            public const string DisplayName = "DisplayName";
            public const string DepartmentId = "DepartmentId";
            public const string UserType = "UserType";
            public const string RoleIds = "RoleIds";
        }
    }
}
