using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record ResetPasswordDto
    {
        public string? UserName { get; init; }  // Admin kullanımı için

        public string? Email { get; init; } // Kullanıcı e-postası, şifre sıfırlama isteği için

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; init; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirm Password is required.")]
        [Compare("Password", ErrorMessage = "Password and Confirm Password must match.")]
        public string? ConfirmPassword { get; init; }

        public string? Token { get; init; }  // Şifre sıfırlama tokeni
    }
}
