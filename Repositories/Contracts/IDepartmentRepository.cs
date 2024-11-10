using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IDepartmentRepository : IRepositoryBase<Department>
    {
        Task CreateOneDepartmentAsync(Department department);
        Task UpdateOneDepartmentAsync(Department entity);
        Task DeleteOneDepartmentAsync(Department department);
        Task<IQueryable<Department>> GetAllDepartmentsAsync(bool trackChanges);
 
        Task<Department?> GetOneDepartmentAsync(int id, bool trackChanges);


        Task<Department> GetDepartmentWithUsersAsync(int departmentId, bool trackChanges);
        Task<Department> GetDepartmentWithQuizzesAsync(int departmentId, bool trackChanges);

    }
}
