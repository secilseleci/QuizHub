using AutoMapper;
using Entities.Dtos;
using Entities.Exeptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;

namespace Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IDepartmentService _departmentService;
        public AuthService(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IDepartmentService departmentService)
                 
            {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
            _departmentService = departmentService;
            }

        public async Task<ResultGeneric<ApplicationUser>> CreateUser(UserDtoForCreation userDto)
        {
            var user = _mapper.Map<ApplicationUser>(userDto);
            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
            {
                var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
                return ResultGeneric<ApplicationUser>.Fail(errorMessages, "User could not be created.");
            }

            if (userDto.Roles.Any())
            {
                var roleResult = await _userManager.AddToRolesAsync(user, userDto.Roles);
                if (!roleResult.Succeeded)
                    return ResultGeneric<ApplicationUser>.Fail("System has problems with roles.");
            }

            return ResultGeneric<ApplicationUser>.Ok(user);
        }

        public async Task<Result> DeleteOneUser(string userName)
        {
            var user = await GetOneUser(userName);
            if (!user.IsSuccess)
                return Result.Fail(user.UserMessage);

            var result = await _userManager.DeleteAsync(user.Data);
            return result.Succeeded ? Result.Ok() : Result.Fail("User could not be deleted.");
        }

        public async Task<Result> UpdateUser(UserDtoForUpdate userDto)
        {
            var user = await _userManager.FindByNameAsync(userDto.UserName);
            if (user == null)
                return Result.Fail("User not found.");

            user.Email = userDto.Email;
            if (user is ApplicationUser appUser)
            {
                appUser.DepartmentId = userDto.DepartmentId;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return Result.Fail("User could not be updated.");

            var userRoles = await _userManager.GetRolesAsync(user);
            var rolesToRemove = userRoles.Except(userDto.UserRoles).ToList();
            var rolesToAdd = userDto.UserRoles.Except(userRoles).ToList();

            if (rolesToRemove.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                    return Result.Fail("Roles could not be removed.");
            }

            if (rolesToAdd.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!addResult.Succeeded)
                    return Result.Fail("Roles could not be added.");
            }

            return Result.Ok();
        }

        public async Task<ResultGeneric<ApplicationUser>> GetOneUser(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user != null
                ? ResultGeneric<ApplicationUser>.Ok(user)
                : ResultGeneric<ApplicationUser>.Fail("User could not be found.");
        }

        public async Task<ResultGeneric<UserDtoForUpdate>> GetOneUserForUpdate(string id)
        {
            var user = await _userManager.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return ResultGeneric<UserDtoForUpdate>.Fail("User not found.");

            var userDto = _mapper.Map<UserDtoForUpdate>(user);
            var userRoles = await _userManager.GetRolesAsync(user);
            userDto.UserRoles = userRoles.ToList();
            userDto.Roles = _roleManager.Roles.Select(r => r.Name).ToList();

            return ResultGeneric<UserDtoForUpdate>.Ok(userDto);
        }

        public async Task<Result> ResetPassword(ResetPasswordDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
                return Result.Fail("User not found.");

            await _userManager.RemovePasswordAsync(user);
            var resetResult = await _userManager.AddPasswordAsync(user, model.Password);

            return resetResult.Succeeded ? Result.Ok() : Result.Fail("Password could not be reset.");
        }

        public async Task<Result> AssignDepartmentToUser(string userId, int departmentId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Result.Fail("User not found.");

            var departmentResult = await _departmentService.GetOneDepartment(departmentId, false);
            if (!departmentResult.IsSuccess)
                return Result.Fail("Department not found.");

            user.DepartmentId = departmentId;
            var updateResult = await _userManager.UpdateAsync(user);

            return updateResult.Succeeded
                ? Result.Ok()
                : Result.Fail("Failed to update the user's department.");
        }

        public async Task<ResultGeneric<List<UserDtoForList>>> GetAllUsersWithRolesAsync()
        {
            var users = await _userManager.Users.Include(u => u.Department).ToListAsync();
            var userWithRolesList = new List<UserDtoForList>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userWithRolesList.Add(new UserDtoForList
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    DepartmentName = user.Department?.DepartmentName,
                    DepartmentId = user.Department?.DepartmentId ?? 0,
                    Roles = roles.ToList()
                });
            }

            return userWithRolesList.Any()
                ? ResultGeneric<List<UserDtoForList>>.Ok(userWithRolesList)
                : ResultGeneric<List<UserDtoForList>>.Fail("No users found.");
        }

        public IEnumerable<IdentityRole> Roles => _roleManager.Roles;

        public async Task<ResultGeneric<IEnumerable<ApplicationUser>>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            return users.Any()
                ? ResultGeneric<IEnumerable<ApplicationUser>>.Ok(users)
                : ResultGeneric<IEnumerable<ApplicationUser>>.Fail("No users found.");
        }
    }
}
