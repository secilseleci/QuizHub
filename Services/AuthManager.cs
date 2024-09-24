using Entities.Dtos;
using Microsoft.AspNetCore.Identity;
using Services.Contracts;
using AutoMapper;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class AuthManager : IAuthService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;  // IdentityUser yerine ApplicationUser
        private readonly IMapper _mapper;

        public AuthManager(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<List<UserDto>> GetAllUsersWithRolesAsync()
        {
            var users = await _userManager.Users.Include(u => u.Department).ToListAsync();

            var userWithRolesList = new List<UserDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user); // Kullanıcının rollerini al
                userWithRolesList.Add(new UserDto
                {
                   
                    UserName = user.UserName,
                    Email = user.Email,
                    Department = user.Department?.Name,  // Departman adı (DepartmentId yerine)
                    Roles = roles.ToList()  // Roller
                });
            }

            return userWithRolesList;
        }

        public IEnumerable<IdentityRole> Roles => _roleManager.Roles;

        public IEnumerable<ApplicationUser> GetAllUsers()  // IdentityUser yerine ApplicationUser
        {
            return _userManager.Users.ToList();
        }

        public async Task<ApplicationUser> GetOneUser(string userName)  // IdentityUser yerine ApplicationUser
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null)
            {
                return user;
            }
            throw new Exception("User could not be found.");
        }

        public async Task<UserDtoForUpdate> GetOneUserForUpdate(string userName)
        {
            var user = await GetOneUser(userName);
            var userDto = _mapper.Map<UserDtoForUpdate>(user);
            userDto.Roles = new List<string>(Roles.Select(r => r.Name).ToList());
            userDto.UserRoles = new HashSet<string>(await _userManager.GetRolesAsync(user));
            return userDto;
        }

        public async Task UpdateUser(UserDtoForUpdate userDto)
        {
            var user = await GetOneUser(userDto.UserName);

            
            user.Email = userDto.Email;
            var appUser = user as ApplicationUser;
            if (appUser != null)
            {
                appUser.DepartmentId = userDto.DepartmentId;
            }
            var result = await _userManager.UpdateAsync(user);

            if (userDto.Roles.Count > 0)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);
                await _userManager.AddToRolesAsync(user, userDto.Roles);
            }
        }

        public async Task<IdentityResult> DeleteOneUser(string userName)
        {
            var user = await GetOneUser(userName);
            return await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordDto model)
        {
            var user = await GetOneUser(model.UserName);
            await _userManager.RemovePasswordAsync(user);
            return await _userManager.AddPasswordAsync(user, model.Password);
        }

        public async Task AssignDepartmentToUser(string userId, int departmentId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            user.DepartmentId = departmentId;  // Kullanıcının DepartmentId'sini ayarlıyoruz
            await _userManager.UpdateAsync(user);  // Güncellenen kullanıcıyı kaydediyoruz
        }

        public async Task<IdentityResult> CreateUser(UserDtoForCreation userDto)
        {
            var user = _mapper.Map<ApplicationUser>(userDto);  // IdentityUser yerine ApplicationUser kullanıyoruz
            var result = await _userManager.CreateAsync(user, userDto.Password);

            if (!result.Succeeded)
                throw new Exception("User could not be created.");

            if (userDto.Roles.Count > 0)
            {
                var roleResult = await _userManager.AddToRolesAsync(user, userDto.Roles);
                if (!roleResult.Succeeded)
                    throw new Exception("System has problems with roles.");
            }

            return result;
        }

    }
}
