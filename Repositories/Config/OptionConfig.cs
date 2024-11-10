using Microsoft.EntityFrameworkCore;
using Entities.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repositories.Config
{
    public class OptionConfig : IEntityTypeConfiguration<Option>
    {
        public void Configure(EntityTypeBuilder<Option> builder)
        {
            builder.HasKey(o => o.OptionId);
            builder.Property(o => o.OptionText).IsRequired();

            builder.HasData(
                // HR Basics Quiz Options
                new Option { OptionId = 1, QuestionId = 1, OptionText = "Human Resources", IsCorrect = true },
                new Option { OptionId = 2, QuestionId = 1, OptionText = "Health Resources", IsCorrect = false },
                new Option { OptionId = 3, QuestionId = 1, OptionText = "Home Resources", IsCorrect = false },
                new Option { OptionId = 4, QuestionId = 1, OptionText = "Hotel Resources", IsCorrect = false },

                new Option { OptionId = 5, QuestionId = 2, OptionText = "Recruitment and Employee Welfare", IsCorrect = true },
                new Option { OptionId = 6, QuestionId = 2, OptionText = "Product Development", IsCorrect = false },
                new Option { OptionId = 7, QuestionId = 2, OptionText = "Financial Management", IsCorrect = false },
                new Option { OptionId = 8, QuestionId = 2, OptionText = "IT Support", IsCorrect = false },

                new Option { OptionId = 9, QuestionId = 3, OptionText = "Payroll Processing", IsCorrect = false },
                new Option { OptionId = 10, QuestionId = 3, OptionText = "Recruitment", IsCorrect = false },
                new Option { OptionId = 11, QuestionId = 3, OptionText = "Employee Wellness", IsCorrect = false },
                new Option { OptionId = 12, QuestionId = 3, OptionText = "Product Marketing", IsCorrect = true },

                new Option { OptionId = 13, QuestionId = 4, OptionText = "Hiring New Employees", IsCorrect = true },
                new Option { OptionId = 14, QuestionId = 4, OptionText = "Selling Products", IsCorrect = false },
                new Option { OptionId = 15, QuestionId = 4, OptionText = "Market Research", IsCorrect = false },
                new Option { OptionId = 16, QuestionId = 4, OptionText = "Financial Management", IsCorrect = false },

                new Option { OptionId = 17, QuestionId = 5, OptionText = "Employee Relations", IsCorrect = true },
                new Option { OptionId = 18, QuestionId = 5, OptionText = "Sales", IsCorrect = false },
                new Option { OptionId = 19, QuestionId = 5, OptionText = "Product Development", IsCorrect = false },
                new Option { OptionId = 20, QuestionId = 5, OptionText = "IT Support", IsCorrect = false },

                // IT Fundamentals Quiz Options
                new Option { OptionId = 21, QuestionId = 6, OptionText = "JavaScript", IsCorrect = true },
                new Option { OptionId = 22, QuestionId = 6, OptionText = "Python", IsCorrect = false },
                new Option { OptionId = 23, QuestionId = 6, OptionText = "Java", IsCorrect = false },
                new Option { OptionId = 24, QuestionId = 6, OptionText = "C#", IsCorrect = false },

                new Option { OptionId = 25, QuestionId = 7, OptionText = "Random Access Memory", IsCorrect = true },
                new Option { OptionId = 26, QuestionId = 7, OptionText = "Read Access Memory", IsCorrect = false },
                new Option { OptionId = 27, QuestionId = 7, OptionText = "Readily Accessible Memory", IsCorrect = false },
                new Option { OptionId = 28, QuestionId = 7, OptionText = "Randomized Access Memory", IsCorrect = false },

                new Option { OptionId = 29, QuestionId = 8, OptionText = "Internet Protocol", IsCorrect = true },
                new Option { OptionId = 30, QuestionId = 8, OptionText = "Internal Process", IsCorrect = false },
                new Option { OptionId = 31, QuestionId = 8, OptionText = "Input Procedure", IsCorrect = false },
                new Option { OptionId = 32, QuestionId = 8, OptionText = "Inter Process", IsCorrect = false },

                new Option { OptionId = 33, QuestionId = 9, OptionText = "Linux", IsCorrect = true },
                new Option { OptionId = 34, QuestionId = 9, OptionText = "HTML", IsCorrect = false },
                new Option { OptionId = 35, QuestionId = 9, OptionText = "CSS", IsCorrect = false },
                new Option { OptionId = 36, QuestionId = 9, OptionText = "SQL", IsCorrect = false },

                new Option { OptionId = 37, QuestionId = 10, OptionText = "Protecting data and systems", IsCorrect = true },
                new Option { OptionId = 38, QuestionId = 10, OptionText = "Building websites", IsCorrect = false },
                new Option { OptionId = 39, QuestionId = 10, OptionText = "Designing databases", IsCorrect = false },
                new Option { OptionId = 40, QuestionId = 10, OptionText = "Creating applications", IsCorrect = false },

                // Design Principles Quiz Options
                new Option { OptionId = 41, QuestionId = 11, OptionText = "A tool for organizing colors", IsCorrect = true },
                new Option { OptionId = 42, QuestionId = 11, OptionText = "A type of fabric", IsCorrect = false },
                new Option { OptionId = 43, QuestionId = 11, OptionText = "A software", IsCorrect = false },
                new Option { OptionId = 44, QuestionId = 11, OptionText = "A type of file format", IsCorrect = false },

                new Option { OptionId = 45, QuestionId = 12, OptionText = "Contrast", IsCorrect = true },
                new Option { OptionId = 46, QuestionId = 12, OptionText = "Coding", IsCorrect = false },
                new Option { OptionId = 47, QuestionId = 12, OptionText = "Encryption", IsCorrect = false },
                new Option { OptionId = 48, QuestionId = 12, OptionText = "Virtualization", IsCorrect = false },

                new Option { OptionId = 49, QuestionId = 13, OptionText = "User Experience", IsCorrect = true },
                new Option { OptionId = 50, QuestionId = 13, OptionText = "User Extraction", IsCorrect = false },
                new Option { OptionId = 51, QuestionId = 13, OptionText = "Utility Examination", IsCorrect = false },
                new Option { OptionId = 52, QuestionId = 13, OptionText = "Unit Exchange", IsCorrect = false },

                new Option { OptionId = 53, QuestionId = 14, OptionText = "Space between elements", IsCorrect = true },
                new Option { OptionId = 54, QuestionId = 14, OptionText = "A type of font", IsCorrect = false },
                new Option { OptionId = 55, QuestionId = 14, OptionText = "A color palette", IsCorrect = false },
                new Option { OptionId = 56, QuestionId = 14, OptionText = "A type of design software", IsCorrect = false },

                new Option { OptionId = 57, QuestionId = 15, OptionText = "Adobe Illustrator", IsCorrect = true },
                new Option { OptionId = 58, QuestionId = 15, OptionText = "Microsoft Word", IsCorrect = false },
                new Option { OptionId = 59, QuestionId = 15, OptionText = "Oracle", IsCorrect = false },
                new Option { OptionId = 60, QuestionId = 15, OptionText = "AutoCAD", IsCorrect = false },

                // Geography Trivia Quiz Options
                new Option { OptionId = 61, QuestionId = 16, OptionText = "Asia", IsCorrect = true },
                new Option { OptionId = 62, QuestionId = 16, OptionText = "Africa", IsCorrect = false },
                new Option { OptionId = 63, QuestionId = 16, OptionText = "Europe", IsCorrect = false },
                new Option { OptionId = 64, QuestionId = 16, OptionText = "North America", IsCorrect = false },

                new Option { OptionId = 65, QuestionId = 17, OptionText = "Vatican City", IsCorrect = true },
                new Option { OptionId = 66, QuestionId = 17, OptionText = "Monaco", IsCorrect = false },
                new Option { OptionId = 67, QuestionId = 17, OptionText = "Liechtenstein", IsCorrect = false },
                new Option { OptionId = 68, QuestionId = 17, OptionText = "Malta", IsCorrect = false },

                new Option { OptionId = 69, QuestionId = 18, OptionText = "Amazon", IsCorrect = false },
                new Option { OptionId = 70, QuestionId = 18, OptionText = "Mississippi", IsCorrect = false },
                new Option { OptionId = 71, QuestionId = 18, OptionText = "Nile", IsCorrect = true },
                new Option { OptionId = 72, QuestionId = 18, OptionText = "Yangtze", IsCorrect = false },

                new Option { OptionId = 73, QuestionId = 19, OptionText = "Sahara", IsCorrect = true },
                new Option { OptionId = 74, QuestionId = 19, OptionText = "Gobi", IsCorrect = false },
                new Option { OptionId = 75, QuestionId = 19, OptionText = "Kalahari", IsCorrect = false },
                new Option { OptionId = 76, QuestionId = 19, OptionText = "Arabian", IsCorrect = false },

                new Option { OptionId = 77, QuestionId = 20, OptionText = "Canberra", IsCorrect = true },
                new Option { OptionId = 78, QuestionId = 20, OptionText = "Sydney", IsCorrect = false },
                new Option { OptionId = 79, QuestionId = 20, OptionText = "Melbourne", IsCorrect = false },
                new Option { OptionId = 80, QuestionId = 20, OptionText = "Brisbane", IsCorrect = false },

                // Historical Events Quiz Options
                new Option { OptionId = 81, QuestionId = 21, OptionText = "1945", IsCorrect = true },
                new Option { OptionId = 82, QuestionId = 21, OptionText = "1918", IsCorrect = false },
                new Option { OptionId = 83, QuestionId = 21, OptionText = "1939", IsCorrect = false },
                new Option { OptionId = 84, QuestionId = 21, OptionText = "1963", IsCorrect = false },

                new Option { OptionId = 85, QuestionId = 22, OptionText = "Christopher Columbus", IsCorrect = true },
                new Option { OptionId = 86, QuestionId = 22, OptionText = "Marco Polo", IsCorrect = false },
                new Option { OptionId = 87, QuestionId = 22, OptionText = "Leif Erikson", IsCorrect = false },
                new Option { OptionId = 88, QuestionId = 22, OptionText = "Vasco da Gama", IsCorrect = false },

                new Option { OptionId = 89, QuestionId = 23, OptionText = "A political tension without direct conflict", IsCorrect = true },
                new Option { OptionId = 90, QuestionId = 23, OptionText = "A world war", IsCorrect = false },
                new Option { OptionId = 91, QuestionId = 23, OptionText = "An economic agreement", IsCorrect = false },
                new Option { OptionId = 92, QuestionId = 23, OptionText = "A military alliance", IsCorrect = false },

                new Option { OptionId = 93, QuestionId = 24, OptionText = "George Washington", IsCorrect = true },
                new Option { OptionId = 94, QuestionId = 24, OptionText = "Abraham Lincoln", IsCorrect = false },
                new Option { OptionId = 95, QuestionId = 24, OptionText = "Thomas Jefferson", IsCorrect = false },
                new Option { OptionId = 96, QuestionId = 24, OptionText = "John Adams", IsCorrect = false },

                new Option { OptionId = 97, QuestionId = 25, OptionText = "1989", IsCorrect = true },
                new Option { OptionId = 98, QuestionId = 25, OptionText = "1961", IsCorrect = false },
                new Option { OptionId = 99, QuestionId = 25, OptionText = "1990", IsCorrect = false },
                new Option { OptionId = 100, QuestionId = 25, OptionText = "1975", IsCorrect = false },

                // Türk Sineması Quiz Seçenekleri

                // Soru 1
                new Option { OptionId = 121, QuestionId = 31, OptionText = "İlyas Salman", IsCorrect = true },
                new Option { OptionId = 122, QuestionId = 31, OptionText = "Kemal Sunal", IsCorrect = false },
                new Option { OptionId = 123, QuestionId = 31, OptionText = "Tarık Akan", IsCorrect = false },
                new Option { OptionId = 124, QuestionId = 31, OptionText = "Şener Şen", IsCorrect = false },

                // Soru 2
                new Option { OptionId = 125, QuestionId = 32, OptionText = "Türkan Şoray", IsCorrect = true },
                new Option { OptionId = 126, QuestionId = 32, OptionText = "Hülya Koçyiğit", IsCorrect = false },
                new Option { OptionId = 127, QuestionId = 32, OptionText = "Filiz Akın", IsCorrect = false },
                new Option { OptionId = 128, QuestionId = 32, OptionText = "Fatma Girik", IsCorrect = false },

                // Soru 3
                new Option { OptionId = 129, QuestionId = 33, OptionText = "Recep İvedik 5", IsCorrect = true },
                new Option { OptionId = 130, QuestionId = 33, OptionText = "Düğün Dernek", IsCorrect = false },
                new Option { OptionId = 131, QuestionId = 33, OptionText = "Ayla", IsCorrect = false },
                new Option { OptionId = 132, QuestionId = 33, OptionText = "Kış Uykusu", IsCorrect = false },

                // Soru 4
                new Option { OptionId = 133, QuestionId = 34, OptionText = "Rıfat Ilgaz", IsCorrect = true },
                new Option { OptionId = 134, QuestionId = 34, OptionText = "Kemal Tahir", IsCorrect = false },
                new Option { OptionId = 135, QuestionId = 34, OptionText = "Yaşar Kemal", IsCorrect = false },
                new Option { OptionId = 136, QuestionId = 34, OptionText = "Orhan Kemal", IsCorrect = false },

                // Soru 5
                new Option { OptionId = 137, QuestionId = 35, OptionText = "1996", IsCorrect = true },
                new Option { OptionId = 138, QuestionId = 35, OptionText = "1998", IsCorrect = false },
                new Option { OptionId = 139, QuestionId = 35, OptionText = "2000", IsCorrect = false },
                new Option { OptionId = 140, QuestionId = 35, OptionText = "2001", IsCorrect = false }

            );
        }
    }
}

