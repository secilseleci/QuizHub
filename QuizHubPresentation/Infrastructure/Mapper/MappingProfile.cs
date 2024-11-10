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
        CreateMap<Quiz, QuizDtoForUpdate>()
                   .ForMember(dest => dest.Questions, opt => opt.MapFrom(src => src.Questions))
                   .ReverseMap(); CreateMap<QuizDto, Quiz>().ReverseMap();
        CreateMap<QuizDtoForList, Quiz>().ReverseMap();
        CreateMap<QuizDtoForUser, Quiz>().ReverseMap();

        // Question Mapping

        CreateMap<Question, QuestionDto>()
                   .ForMember(dest => dest.Options, opt => opt.MapFrom(src => src.Options))
                   .ReverseMap();
        // Option Mapping
        CreateMap<Option, OptionDto>().ReverseMap();

        // User Mapping
        CreateMap<UserDtoForCreation, ApplicationUser>().ReverseMap(); ;
        CreateMap<UserDtoForUpdate, ApplicationUser>().ReverseMap();

        // Temp tablolardan ViewModel'e mapping
        CreateMap<UserQuizInfoTemp, QuizResultViewModel>();
        // Kalıcı tablodan ViewModel'e mapping
        CreateMap<UserQuizInfo, QuizResultViewModel>();

        //Card Mapleri
        // UserQuizInfo to UserQuizCardDto mapping
        CreateMap<UserQuizInfo, UserQuizCardDto>()
            .ForMember(dest => dest.QuizId, opt => opt.MapFrom(src => src.Quiz.QuizId))
            .ForMember(dest => dest.QuizTitle, opt => opt.MapFrom(src => src.Quiz.Title))
            .ForMember(dest => dest.QuestionCount, opt => opt.MapFrom(src => src.Quiz.QuestionCount))
            .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
            .ForMember(dest => dest.CompletedAt, opt => opt.MapFrom(src => src.CompletedAt))
            .ForMember(dest => dest.CanRetake, opt => opt.MapFrom(src => src.Score < 100))
            .ForMember(dest => dest.Status, opt => opt.Ignore()); // Manuel olarak status ayarlanacak


        // UserQuizInfoTemp to UserQuizCardDto mapping
        CreateMap<UserQuizInfoTemp, UserQuizCardDto>()
            .ForMember(dest => dest.QuizId, opt => opt.MapFrom(src => src.Quiz.QuizId))
            .ForMember(dest => dest.QuizTitle, opt => opt.MapFrom(src => src.Quiz.Title))
            .ForMember(dest => dest.QuestionCount, opt => opt.MapFrom(src => src.Quiz.QuestionCount))
            .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
            .ForMember(dest => dest.Status, opt => opt.Ignore()); // Manuel olarak status ayarlanacak

        // Quiz to UserQuizCardDto mapping
        CreateMap<Quiz, UserQuizCardDto>()
            .ForMember(dest => dest.QuizId, opt => opt.MapFrom(src => src.QuizId))
            .ForMember(dest => dest.QuizTitle, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.QuestionCount, opt => opt.MapFrom(src => src.QuestionCount))
            .ForMember(dest => dest.Status, opt => opt.Ignore()); // Manuel olarak status ayarlanacak
                                                                  // Temp'den kalıcı tabloya geçici dönüşüm için map
        CreateMap<UserQuizInfoTemp, UserQuizInfo>()
            .ForMember(dest => dest.UserQuizInfoId, opt => opt.Ignore()); // ID değişmemeli

    }
}
