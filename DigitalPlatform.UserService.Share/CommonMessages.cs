namespace DigitalPlatform.UserService.Share
{
    public static class CommonMessages
    {
        public const string RequestInvalid = "Request invalid!";
        public const string TheItemAlreadyExists = "Dữ liệu đã tồn tại!";
        public const string TheItemDoesNotExist = "Dữ liệu không tồn tại!";
        public const string Code404NotFound = "Không có dữ liệu hợp lệ.";
        public const string DoNotHadPermission = "Không có quyền.";

        public struct UserMessage
        {
            public const string EmailExisted = "Email đã tồn tại.";
            public const string PhoneNumberExisted = "Số điện thoại đã tồn tại.";
            public const string IdNoExisted = "Số CMND/CCCD đã tồn tại.";
            public const string DepartmentOfUserInActive = "Đơn vị của user đang ngừng kích hoạt.";
            public const string CreateUserFail = "Tạo mới người dùng không thành công.";
            public const string UpdateUserFail = "Cập nhật người dùng không thành công.";
        }
    }
}
