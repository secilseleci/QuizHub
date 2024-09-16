using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class OptionConfig : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> builder)
        {
            // Option için temel yapılandırma
            builder.HasKey(o => o.OptionId);
            builder.Property(o => o.OptionText).IsRequired();

            // Seed data - Options
            builder.HasData(
                //Quiz1
               
                new Option
                {
                    OptionId = 1,
                    QuestionId = 1,
                    OptionText = "Tokyo",
                    IsCorrect = true
                },
                new Option
                {
                    OptionId = 2,
                    QuestionId = 1,
                    OptionText = "Osaka",
                    IsCorrect = false
                },
                new Option
                  {
                    OptionId = 3,
                    QuestionId = 1,
                    OptionText = "Ankara",
                    IsCorrect = false
                  },
                new Option
                {
                    OptionId = 4,
                    QuestionId = 1,
                    OptionText = "Paris",
                    IsCorrect = false
                },

                new Option
                {
                    OptionId = 5,
                    QuestionId = 2,
                    OptionText = "Paris",
                    IsCorrect = true
                },
                new Option
                {
                    OptionId = 6,
                    QuestionId = 2,
                    OptionText = "Berlin",
                    IsCorrect = false
                },
                new Option
                 {
                     OptionId = 7,
                     QuestionId = 2,
                     OptionText = "Tokyo",
                     IsCorrect = false
                 }, 
                new Option
                 {
                     OptionId = 8,
                     QuestionId = 2,
                     OptionText = "İstanbul",
                     IsCorrect = false
                 },

                new Option
                {
                    OptionId = 9,
                    QuestionId = 3,
                    OptionText = "New York",
                    IsCorrect = false
                },
                new Option
                {
                    OptionId =10,
                    QuestionId = 3,
                    OptionText = "Washington",
                    IsCorrect = true
                },
                new Option
                 {
                     OptionId = 11,
                     QuestionId = 3,
                     OptionText = "Tokyo",
                     IsCorrect = false
                 }, 
                new Option
                 {
                     OptionId = 12,
                     QuestionId = 3,
                     OptionText = "İstanbul",
                     IsCorrect = false
                 },

                new Option
                 {
                    OptionId = 13,
                    QuestionId = 4,
                    OptionText = "Adana",
                    IsCorrect = false
                 },
                new Option
                {
                    OptionId = 14,
                    QuestionId = 4,
                    OptionText = "Manisa",
                    IsCorrect = false,
                },
                new Option
                {
                    OptionId = 15,
                    QuestionId = 4,
                    OptionText = "Ottava",
                    IsCorrect = true,
                },
                new Option
                 {
                    OptionId = 16,
                    QuestionId = 4,
                    OptionText = "Moskova",
                    IsCorrect = false
                 },

                new Option
                {
                    OptionId = 17,
                    QuestionId = 5,
                    OptionText = "Berlin",
                    IsCorrect = false
                },
                new Option
                {
                    OptionId = 18,
                    QuestionId = 5,
                    OptionText = "Ankara",
                    IsCorrect = true
                },
                new Option
                {
                    OptionId = 19,
                    QuestionId = 5,
                    OptionText = "Tokyo",
                    IsCorrect = false
                },
                new Option
                 {
                    OptionId = 20,
                    QuestionId = 5,
                    OptionText = "Paris",
                    IsCorrect = false
                },

                new Option
                 {
                    OptionId = 21,
                    QuestionId = 6,
                    OptionText = "Tokyo",
                    IsCorrect = false
                 },
                new Option
                {
                    OptionId = 22,
                    QuestionId = 6,
                    OptionText = "Osaka",
                    IsCorrect = false
                },
                new Option
                {
                    OptionId = 23,
                    QuestionId = 6,
                    OptionText = "Madrid",
                    IsCorrect = true
                },
                new Option
                 {
                    OptionId = 24,
                    QuestionId = 6,
                    OptionText = "Paris",
                    IsCorrect = false
                 },

                new Option
                {
                    OptionId = 25,
                    QuestionId = 7,
                    OptionText = "Tokyo",
                    IsCorrect = false
                },
                new Option
                {
                    OptionId = 26,
                    QuestionId = 7,
                    OptionText = "Rome",
                    IsCorrect = true
                },
                new Option
                {
                    OptionId = 27,
                    QuestionId = 7,
                    OptionText = "Tokyo",
                    IsCorrect = false,
                },
                new Option
                {
                    OptionId = 28,
                    QuestionId = 7,
                    OptionText = "Berlin",
                    IsCorrect = false
                },

                new Option
                {
                    OptionId = 29,
                    QuestionId = 8,
                    OptionText = "Melbourne",
                    IsCorrect = false
                },
                new Option
                 {
                     OptionId = 30,
                     QuestionId = 8,
                     OptionText = "Sydney",
                     IsCorrect = false,
                 },
                new Option
                {
                    OptionId = 31,
                    QuestionId = 8,
                    OptionText = "Ankara",
                    IsCorrect = false
                },
                new Option
                 {
                    OptionId = 32,
                    QuestionId = 8,
                    OptionText = "London",
                    IsCorrect = true
                 },

                //quiz2
                new Option
                {
                    OptionId = 33,
                    QuestionId = 9,
                    OptionText = "44",
                    IsCorrect = true
                },
                new Option
                {
                    OptionId = 34,
                    QuestionId = 9,
                    OptionText = "45",
                    IsCorrect = false
                },
                new Option
                {
                    OptionId = 35,
                    QuestionId = 9,
                    OptionText = "46",
                    IsCorrect = false
                },
                new Option
                 {
                    OptionId = 36,
                    QuestionId = 9,
                    OptionText = "47",
                    IsCorrect = false
                 },  
                 
                new Option
                {
                    OptionId = 37,
                    QuestionId = 10,
                    OptionText = "Slovakia",
                    IsCorrect = false
                },
                new Option
                {
                    OptionId = 38,
                    QuestionId = 10,
                    OptionText = "Poland",
                    IsCorrect = false
                },
                new Option
                {
                    OptionId = 39,
                    QuestionId = 10,
                    OptionText = "Turkey",
                    IsCorrect = true,
                },
                new Option
                 {
                    OptionId = 40,
                    QuestionId = 10,
                    OptionText = "Estonia",
                    IsCorrect = false,
                 },

                new Option
                {
                    OptionId = 41,
                    QuestionId = 11,
                    OptionText = "Yemen",
                    IsCorrect = false
                },
                new Option
                {
                    OptionId = 42,
                    QuestionId = 11,
                    OptionText = "Ireland",
                    IsCorrect = true,
                },
                new Option
                {
                    OptionId = 43,
                    QuestionId = 11,
                    OptionText = "Qatar",
                    IsCorrect = false,
                },
                new Option
                 {
                    OptionId = 44,
                    QuestionId = 11,
                    OptionText = "India",
                    IsCorrect = false,
                 },

                //Quiz3
                new Option
                  {
                      OptionId = 45,
                      QuestionId = 12,
                      OptionText = "Gold",
                      IsCorrect = false,
                  },
                new Option
                  {
                      OptionId = 46,
                      QuestionId = 12,
                      OptionText = "Oxygen",
                      IsCorrect = true,
                  },
                new Option
                  {
                      OptionId = 47,
                      QuestionId = 12,
                      OptionText = "Osmium",
                      IsCorrect = false,
                  },
                new Option
                  {
                      OptionId = 48,
                      QuestionId = 12,
                      OptionText = "Ozone",
                      IsCorrect = false,
                  },

                new Option
                  {
                      OptionId = 49,
                      QuestionId = 13,
                      OptionText = "Gold",
                      IsCorrect =  false,
                  },
                new Option
                  {
                      OptionId = 50,
                      QuestionId = 13,
                      OptionText = "Iron",
                      IsCorrect =  false,
                  },
                new Option
                  {
                      OptionId = 51,
                      QuestionId = 13,
                      OptionText = "Diamond",
                      IsCorrect =  true,
                  },
                new Option
                  {
                      OptionId = 52,
                      QuestionId = 13,
                      OptionText = "Quartz",
                      IsCorrect =  false,
                  },

                new Option
                   {
                       OptionId = 53,
                       QuestionId = 14,
                       OptionText = "Venus",
                       IsCorrect =  false,
                   },
                new Option
                   {
                       OptionId = 54,
                       QuestionId = 14,
                       OptionText = "Mars",
                       IsCorrect =  true,
                   },
                new Option
                   {
                       OptionId = 55,
                       QuestionId = 14,
                       OptionText = "Jupiter",
                       IsCorrect = false,
                   },
                new Option
                   {
                       OptionId = 56,
                       QuestionId = 14,
                       OptionText = "Saturn",
                       IsCorrect =  false,
                   }
                     



            );
        }
    }
}
