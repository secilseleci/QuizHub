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
        CreateMap<QuizDtoForUserShowcase,Quiz>().ReverseMap();
        // Question Mapping
        CreateMap<QuestionDtoForInsertion, Question>().ReverseMap();
        CreateMap<QuestionDtoForUpdate, Question>().ReverseMap();
        CreateMap<QuestionDto, Question>().ReverseMap();
        CreateMap<QuestionDtoForList, Question>().ReverseMap();   

        // Option Mapping
        CreateMap<OptionDtoForInsertion, Option>().ReverseMap();
        CreateMap<OptionDtoForUpdate, Option>().ReverseMap();
        CreateMap<OptionDto, Option>().ReverseMap();
        CreateMap<OptionDtoForList, Option>().ReverseMap();   

        // User Mapping
        CreateMap<UserDtoForCreation, IdentityUser>().ReverseMap(); ;
        CreateMap<UserDtoForUpdate, IdentityUser>().ReverseMap();

        // UserQuizInfo -> QuizDetailsViewModel mapping
        CreateMap<UserQuizInfo, QuizDetailsViewModel>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Quiz.Title))  // Quiz başlığı
            .ForMember(dest => dest.QuestionCount, opt => opt.MapFrom(src => src.Quiz.Questions.Count))  // Soru sayısı
            .ForMember(dest => dest.EstimatedTime, opt => opt.MapFrom(src => src.Quiz.Questions.Count * 1))  // Soru başına 1 dakika
            .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.IsCompleted));  // Tamamlanma durumu
    }
}
