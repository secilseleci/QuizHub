using Entities.Models;

namespace Services.Contracts
{
    public interface IDepartmentService
    {
        IEnumerable<Department> GetAllDepartments(bool trackChanges);
        Department GetDepartmentWithUsers(int departmentId, bool trackChanges);
        Department? GetOneDepartment(int id, bool trackChanges);

    }
}
