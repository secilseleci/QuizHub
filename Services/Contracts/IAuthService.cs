using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
 


namespace Services.Contracts
{
    public interface IAuthService
    {
        IEnumerable<IdentityRole> Roles { get; }
        IEnumerable<ApplicationUser> GetAllUsers();
        Task<IdentityResult> CreateUser(UserDtoForCreation userDto);
        Task<ApplicationUser> GetOneUser(string userName);
        Task UpdateUser(UserDtoForUpdate userDto);
        Task<UserDtoForUpdate> GetOneUserForUpdate(string userName);
        Task<IdentityResult> ResetPassword(ResetPasswordDto model);
        Task<IdentityResult> DeleteOneUser(string userName);
        Task AssignDepartmentToUser(string userId, int departmentId);  // Departman atama metodu
        Task<List<UserDto>> GetAllUsersWithRolesAsync();



    }
}