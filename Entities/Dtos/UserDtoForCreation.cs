﻿using System.ComponentModel.DataAnnotations;
 
namespace Entities.Dtos
{
    public record UserDtoForCreation
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; init; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "UserName is required.")]
        public string? UserName { get; init; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; init; }

        [Required(ErrorMessage = "Department is required.")]
        public int DepartmentId { get; init; }   

        public List<string> Roles { get; set; } = new List<string>();   

    }

}