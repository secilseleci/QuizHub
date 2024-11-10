using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories.Implementations
{
    public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(RepositoryContext context) : base(context)
        {
        }
        public async Task CreateOneDepartmentAsync(Department department) => await CreateAsync(department);
        public async Task UpdateOneDepartmentAsync(Department entity) => await UpdateAsync(entity);
        public async Task DeleteOneDepartmentAsync(Department department) => await RemoveAsync(department);


        public async Task<IQueryable<Department>> GetAllDepartmentsAsync(bool trackChanges)
            => await FindAllAsync(trackChanges);
        public async Task<Department?> GetOneDepartmentAsync(int id, bool trackChanges)
            => await FindByConditionAsync(d => d.DepartmentId.Equals(id), trackChanges);
      
        public async Task<Department> GetDepartmentWithUsersAsync(int departmentId, bool trackChanges)
        {
            var query = await FindAllAsync(trackChanges);
            return await query
               .Include(d => d.Users)
               .Where(d => d.DepartmentId == departmentId)
               .SingleOrDefaultAsync();
        }

       

        public async Task<Department> GetDepartmentWithQuizzesAsync(int departmentId, bool trackChanges)
        {
            var query = await FindAllAsync(trackChanges);
            return await query
               .Include(d => d.Quizzes)  
               .Where(d => d.DepartmentId == departmentId)  
               .SingleOrDefaultAsync();
        }


    
    }
}
