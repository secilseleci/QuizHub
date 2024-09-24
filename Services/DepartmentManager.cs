using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    
        public class DepartmentManager : IDepartmentService
        {
            private readonly IRepositoryManager _repository;

            public DepartmentManager(IRepositoryManager repository)
            {
                _repository = repository;
            }

        public IEnumerable<Department> GetAllDepartments(bool trackChanges)
        {
            return _repository.Department.FindAll(trackChanges).ToList();
        }

        public Department GetDepartmentWithUsers(int departmentId, bool trackChanges)
        {
            return _repository.Department.GetDepartmentWithUsers(departmentId, trackChanges);
        }
    }
}
