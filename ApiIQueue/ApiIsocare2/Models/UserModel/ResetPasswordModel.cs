namespace ApiIsocare2.Models.UserModel
{
    public class ResetPasswordModel
    {
        public string otp { get; set; }
        public string? newPassword { get; set; }
    }
}
