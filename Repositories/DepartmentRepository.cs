using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories
{
    public class DepartmentRepository : RepositoryBase<Department>, IDepartmentRepository
    {
        public DepartmentRepository(RepositoryContext context) : base(context)
        {
        }

        public Department GetDepartmentWithUsers(int departmentId, bool trackChanges)
        {
            return FindAll(trackChanges)
                   .Where(d => d.DepartmentId == departmentId)
                   .Include(d => d.Users)  
                   .SingleOrDefault();
        }

    }
}
