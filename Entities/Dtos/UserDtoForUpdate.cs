using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record UserDtoForUpdate : UserDto
    {
        public List<string> UserRoles { get; set; } = new List<string>(); // Roller

    }
}