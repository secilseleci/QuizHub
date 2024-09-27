using Entities.Dtos;
using Microsoft.AspNetCore.Identity;
using Services.Contracts;
using AutoMapper;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace Services
{
    public class AuthManager : IAuthService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;  // IdentityUser yerine ApplicationUser
        private readonly IMapper _mapper;
         

        public AuthManager(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IMapper mapper  )
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
        }
      
            public async Task<List<UserDtoForList>> GetAllUsersWithRolesAsync()
            {
            var users = await _userManager.Users.Include(u => u.Department).ToListAsync();

            var userWithRolesList = new List<UserDtoForList>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                // Kullanıcıyı DTO'ya map ediyoruz ve departman adını ekliyoruz
                userWithRolesList.Add(new UserDtoForList
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    DepartmentName = user.Department?.DepartmentName, 
                    DepartmentId=user.Department.DepartmentId,
                    Roles = roles.ToList()
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

        public async Task<UserDtoForUpdate> GetOneUserForUpdate(string id)
        {
            // Kullanıcıyı bul, Departman ve Roller dahil
            var user = await _userManager.Users
                .Include(u => u.Department) // Departman bilgisi dahil
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                throw new Exception("User not found.");

            // Kullanıcıyı DTO'ya map ediyoruz
            var userDto = _mapper.Map<UserDtoForUpdate>(user);

            var userRoles = await _userManager.GetRolesAsync(user);
            userDto.UserRoles = userRoles.ToList();  // Kullanıcıya atanmış rolleri dto'ya ekle
            var allRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            userDto.Roles = allRoles;  // Sistem'deki tüm rolleri dto'ya ekle
            return userDto;
        }


        public async Task UpdateUser(UserDtoForUpdate userDto)
        {
             var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.UserName == userDto.UserName);

            if (user == null)
                throw new Exception("User not found.");

            // Kullanıcının emailini ve departmanını güncelle
            user.Email = userDto.Email;
            if (user is ApplicationUser appUser)
            {
                appUser.DepartmentId = userDto.DepartmentId;
            }

            // Kullanıcıyı güncelle
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                throw new Exception("User could not be updated.");

            // Kullanıcının mevcut rollerini al ve güncelle
            var userRoles = await _userManager.GetRolesAsync(user);
            var rolesToRemove = userRoles.Except(userDto.UserRoles).ToList();
            var rolesToAdd = userDto.UserRoles.Except(userRoles).ToList();

            // Eski rolleri kaldır
            if (rolesToRemove.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                    throw new Exception("Roles could not be removed.");
            }

            // Yeni rolleri ekle
            if (rolesToAdd.Any())
            {
                var addResult = await _userManager.AddToRolesAsync(user, rolesToAdd);
                if (!addResult.Succeeded)
                    throw new Exception("Roles could not be added.");
            }
        }

    }
}
