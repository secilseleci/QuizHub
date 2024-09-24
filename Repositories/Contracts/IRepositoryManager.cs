using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IRepositoryManager
    {
        IQuizRepository Quiz { get; }
        IQuestionRepository Question { get; }
        IOptionRepository Option { get; }
        IUserQuizInfoRepository UserQuizInfo { get; }
        IUserAnswerRepository UserAnswer { get; }
        IDepartmentRepository Department { get; } // Departman repository eklendi

        void Save();
    }
}
