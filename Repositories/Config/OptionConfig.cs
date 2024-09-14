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
                 }, new Option
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
                 }, new Option
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
                    OptionText = "İstanbul",
                    IsCorrect = false
                },
                 new Option
                 {
                     OptionId = 28,
                     QuestionId = 7,
                     OptionText = "Edirne",
                     IsCorrect = false
                 },





                  new Option
                  {
                      OptionId = 29,
                      QuestionId = 8,
                      OptionText = "Tokyo",
                      IsCorrect = true
                  },
                new Option
                {
                    OptionId = 30,
                    QuestionId = 8,
                    OptionText = "Berlin",
                    IsCorrect = false
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
                 }


            );
        }
    }
}
