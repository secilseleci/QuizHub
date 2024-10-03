using AutoMapper;
using Entities.Models;
using Entities.Dtos;
using QuizHubPresentation.Models;
using Microsoft.AspNetCore.Identity;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Quiz Mapping
        CreateMap<QuizDtoForInsertion, Quiz>().ReverseMap();
        CreateMap<QuizDtoForUpdate, Quiz>().ReverseMap();
        CreateMap<QuizDto, Quiz>().ReverseMap();
        CreateMap<QuizDtoForList, Quiz>().ReverseMap();
        CreateMap<QuizDtoForUser, Quiz>().ReverseMap();
        
        // Question Mapping
     
        CreateMap<QuestionDto, Question>().ReverseMap();
 
        // Option Mapping
        CreateMap<OptionDto, Option>().ReverseMap();
 
        // User Mapping
        CreateMap<UserDtoForCreation, ApplicationUser>().ReverseMap(); ;
        CreateMap<UserDtoForUpdate, ApplicationUser>().ReverseMap();

         
 
     }
}
