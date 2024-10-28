using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{

    public class DepartmentManager : IDepartmentService
        {
            private readonly IRepositoryManager _manager;

            public DepartmentManager(IRepositoryManager  manager)
            {
                _manager =  manager;
            }

        public IEnumerable<Department> GetAllDepartments(bool trackChanges)
        {
            return _manager.Department.GetAllDepartments(trackChanges);
        }

        public Department GetDepartmentWithUsers(int departmentId, bool trackChanges)
        {
            return _manager.Department.GetDepartmentWithUsers(departmentId, trackChanges);
        }

        public Department? GetOneDepartment(int id, bool trackChanges)
        {
            var department = _manager.Department.GetOneDepartment(id, trackChanges);
            if (department is null)
                throw new Exception("department not found!");
            return department;
        }


        public Department GetDepartmentWithQuizzes(int departmentId, bool trackChanges)
        {
            var department = _manager.Department.GetDepartmentWithQuizzes(departmentId, trackChanges);
            if (department == null)
            {
                throw new Exception("Department not found!");
            }

            return department;
        }

         
         
    }
}
