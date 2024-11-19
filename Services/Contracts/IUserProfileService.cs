using Entities.Exeptions;
using Entities.Dtos;

namespace Services.Contracts
{
    public interface IUserProfileService
    {        
        Task<ResultGeneric<ProfileUpdateDto>> GetProfileAsync(string userId);
        Task<Result> UpdateProfileAsync(ProfileUpdateDto profileUpdateDto, string userId);
        Task<Result> UpdatePasswordAsync(string userId, string currentPassword, string newPassword);
    }

}
