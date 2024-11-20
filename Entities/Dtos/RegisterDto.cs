using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record RegisterDto
    {
        [Required(ErrorMessage = "Username is required.")]
        public String? UserName { get; init; }
        [Required(ErrorMessage = "Email is required.")]
        public String? Email { get; init; }
        [Required(ErrorMessage = "Password is required.")]
        public String? Password { get; init; }

        [Required(ErrorMessage = "Department is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid department.")]

        public int DepartmentId { get; set; }
    }
}