using AutoMapper;
using Entities.Models;
using Entities.Dtos;
using QuizHubPresentation.Models;
using Microsoft.AspNetCore.Identity;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<QuizDtoForInsertion, Quiz>().ReverseMap();

        CreateMap<QuizDtoForUpdate, Quiz>().ReverseMap();

        CreateMap<UserDtoForCreation, IdentityUser>().ReverseMap(); ;
        CreateMap<UserDtoForUpdate, IdentityUser>().ReverseMap();
    }
}
