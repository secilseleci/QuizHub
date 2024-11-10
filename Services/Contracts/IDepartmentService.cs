using Entities.Exeptions;
using Entities.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Services.Contracts
{
    public interface IDepartmentService
    {
        Task<ResultGeneric<Department>> CreateOneDepartment(Department department);
        Task<ResultGeneric<Department>> UpdateOneDepartment(Department department);
        Task<Result> DeleteOneDepartment(int id);
        Task<ResultGeneric<IEnumerable<Department>>> GetAllDepartments(bool trackChanges);
        Task<ResultGeneric<Department>> GetOneDepartment(int id, bool trackChanges);
        Task<ResultGeneric<List<SelectListItem>>> GetAllDepartmentsWithSelection(int quizId, bool trackChanges);
        Task<ResultGeneric<IEnumerable<Department>>> GetDepartmentsByQuizId(int quizId, bool trackChanges);
        Task<ResultGeneric<IEnumerable<Quiz>>> GetQuizzesByDepartmentId(int departmentId, bool trackChanges);

        Task<ResultGeneric<Department>> GetDepartmentWithUsers(int departmentId, bool trackChanges);

        Task<ResultGeneric<Department>> GetDepartmentWithQuizzes(int departmentId, bool trackChanges);

    }
}
