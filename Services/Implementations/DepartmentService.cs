using Entities.Exeptions;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Services.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IRepositoryManager _manager;

        public DepartmentService(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public async Task<ResultGeneric<Department>> CreateOneDepartment(Department department)
        {
            await _manager.Department.CreateOneDepartmentAsync(department);
            var saveResult = await _manager.SaveAsync();
            if (!saveResult)
            {
                return ResultGeneric<Department>.Fail("Departman kaydedilemedi.", "Bir şeyler ters gitti.");
            }
            return ResultGeneric<Department>.Ok(department);
        }

        public async Task<ResultGeneric<Department>> UpdateOneDepartment(Department department)
        {
            await _manager.Department.UpdateOneDepartmentAsync(department);
            var saveResult = await _manager.SaveAsync();
            if (!saveResult)
            {
                return ResultGeneric<Department>.Fail("Departman güncellenemedi.", "Bir şeyler ters gitti.");
            }
            return ResultGeneric<Department>.Ok(department);
        }

        public async Task<Result> DeleteOneDepartment(int id)
        {
            var department = await _manager.Department.GetOneDepartmentAsync(id, trackChanges: false);
            if (department == null)
            {
                return Result.Fail("Departman bulunamadı.", "Silmek istediğiniz departman mevcut değil.");
            }
            await _manager.Department.DeleteOneDepartmentAsync(department);
            var saveResult = await _manager.SaveAsync();
            if (!saveResult)
            {
                return Result.Fail("Departman silinemedi.", "Bir şeyler ters gitti.");
            }
            return Result.Ok();
        }

        public async Task<ResultGeneric<IEnumerable<Department>>> GetAllDepartments(bool trackChanges)
        {
            var departments = await _manager.Department.GetAllDepartmentsAsync(trackChanges);
            if (departments == null || !departments.Any())
            {
                return ResultGeneric<IEnumerable<Department>>.Fail("Departman bulunamadı.");
            }
            return ResultGeneric<IEnumerable<Department>>.Ok(departments);
        }

        public async Task<ResultGeneric<Department>> GetOneDepartment(int id, bool trackChanges)
        {
            var department = await _manager.Department.GetOneDepartmentAsync(id, trackChanges);
            if (department == null)
            {
                return ResultGeneric<Department>.Fail("Departman bulunamadı.", "İlgili departman mevcut değil.");
            }
            return ResultGeneric<Department>.Ok(department);
        }

        public async Task<ResultGeneric<List<SelectListItem>>> GetAllDepartmentsWithSelection(int quizId, bool trackChanges)
        {
            var allDepartments = await GetAllDepartments(trackChanges);
            if (!allDepartments.IsSuccess)
            {
                return ResultGeneric<List<SelectListItem>>.Fail("Departmanlar yüklenemedi.");
            }

            var assignedDepartmentIds = (await GetDepartmentsByQuizId(quizId, trackChanges)).Data
                .Select(d => d.DepartmentId)
                .ToList();

            var departments = allDepartments.Data.Select(d => new SelectListItem
            {
                Value = d.DepartmentId.ToString(),
                Text = d.DepartmentName,
                Selected = assignedDepartmentIds.Contains(d.DepartmentId)
            }).ToList();

            return ResultGeneric<List<SelectListItem>>.Ok(departments);
        }

        public async Task<ResultGeneric<IEnumerable<Department>>> GetDepartmentsByQuizId(int quizId, bool trackChanges)
        {
            var quiz = await _manager.Quiz.GetQuizWithDepartmentsAsync(quizId, trackChanges);
            if (quiz == null)
            {
                return ResultGeneric<IEnumerable<Department>>.Fail("Quiz bulunamadı.", "İlgili quiz mevcut değil.");
            }
            return ResultGeneric<IEnumerable<Department>>.Ok(quiz.Departments);
        }

        public async Task<ResultGeneric<IEnumerable<Quiz>>> GetQuizzesByDepartmentId(int departmentId, bool trackChanges)
        {
            var department = await _manager.Department.GetDepartmentWithQuizzesAsync(departmentId, trackChanges);
            if (department == null)
            {
                return ResultGeneric<IEnumerable<Quiz>>.Fail("Departman bulunamadı.", "İlgili departman mevcut değil.");
            }
            return ResultGeneric<IEnumerable<Quiz>>.Ok(department.Quizzes);
        }

        public async Task<ResultGeneric<Department>> GetDepartmentWithUsers(int departmentId, bool trackChanges)
        {
            var department = await _manager.Department.GetDepartmentWithUsersAsync(departmentId, trackChanges);
            if (department == null)
            {
                return ResultGeneric<Department>.Fail("Departman bulunamadı.", "İlgili departman mevcut değil.");
            }

            return ResultGeneric<Department>.Ok(department);
        }

        public async Task<ResultGeneric<Department>> GetDepartmentWithQuizzes(int departmentId, bool trackChanges)
        {
            var department = await _manager.Department.GetDepartmentWithQuizzesAsync(departmentId, trackChanges);
            if (department == null)
            {
                return ResultGeneric<Department>.Fail("Departman bulunamadı.", "İlgili departman mevcut değil.");
            }

            return ResultGeneric<Department>.Ok(department);
        }

    }
}
