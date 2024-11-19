using Entities.Dtos;
using Entities.Exeptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;

namespace Services.Implementations
{
    public class UserProfileService : IUserProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager; 
 
        public UserProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ResultGeneric<ProfileUpdateDto>> GetProfileAsync(string userId)
        {
            var user = await _userManager.Users
                  .Include(u => u.Department)  
                  .FirstOrDefaultAsync(u => u.Id == userId);
            
            if (user == null)
            {
                return ResultGeneric<ProfileUpdateDto>.Fail("User not found.");
            }

            var profile = new ProfileUpdateDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Department = user.Department != null ? user.Department.DepartmentName : "No Department Assigned"
            };

            return ResultGeneric<ProfileUpdateDto>.Ok(profile);
        }
        public async Task<Result> UpdateProfileAsync(ProfileUpdateDto profileUpdateDto, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result.Fail("User not found.");
            }

            if (user.UserName == profileUpdateDto.UserName)
            {
                return Result.Fail("No changes were made.");
            }

            user.UserName = profileUpdateDto.UserName;
            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded ? Result.Ok("Profile updated successfully.") : Result.Fail("Profile update failed.");
        }

        public async Task<Result> UpdatePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result.Fail("User not found.");
            }

            var passwordCheck = await _userManager.CheckPasswordAsync(user, currentPassword);
            if (!passwordCheck)
            {
                return Result.Fail("Current password is incorrect.");
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded ? Result.Ok("Password updated successfully.") : Result.Fail("Password update failed.");
        }



    }
}
