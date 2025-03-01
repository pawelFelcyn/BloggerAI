namespace BloggerAI.Core.Dtos;

public sealed class ChangePasswordDto
{
    public required string Email { get; set; }
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
    public required string ConfirmNewPassword { get; set; }
}