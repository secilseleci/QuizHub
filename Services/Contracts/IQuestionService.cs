using Entities.Dtos;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IQuestionService
    {
        IEnumerable<Question> GetAllQuestions(bool trackChanges);
        IEnumerable<Question> GetQuestionsByQuizId(int quizId, bool trackChanges);
        Question? GetOneQuestion(int id, bool trackChanges);
        Question? GetOneQuestionWithOptions(int id, bool trackChanges);
        void CreateOneQuestion(QuestionDto questionDto);
        void UpdateOneQuestion(QuestionDto questionDto);
        void DeleteOneQuestion(int id);
        
    }
}
