using Entities.Dtos;
using Entities.Exeptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace Services.Contracts
{
    public interface IAuthService
    {
        Task<ResultGeneric<ApplicationUser>> CreateUser(UserDtoForCreation userDto);
        Task<Result> DeleteOneUser(string userName);
        Task<Result> UpdateUser(UserDtoForUpdate userDto);
        Task<ResultGeneric<IEnumerable<ApplicationUser>>> GetAllUsers();
        Task<ResultGeneric<ApplicationUser>> GetOneUser(string userName);
        Task<ResultGeneric<UserDtoForUpdate>> GetOneUserForUpdate(string id);
        IEnumerable<IdentityRole> Roles { get; }
        Task<Result> ResetPassword(ResetPasswordDto model);
        Task<Result> AssignDepartmentToUser(string userId, int departmentId);
        Task<ResultGeneric<List<UserDtoForList>>> GetAllUsersWithRolesAsync();
    }
}
