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

        public IQueryable<Department> GetAllDepartments(bool trackChanges) => FindAll(trackChanges);
       

        public Department GetDepartmentWithUsers(int departmentId, bool trackChanges)
        {
            var query = _context.Departments
               .Include(d => d.Users)   // İlişkili Users varlıklarını dahil ediyoruz
               .Where(d => d.DepartmentId == departmentId);  // Sadece ilgili Department kaydını alıyoruz

            return trackChanges ? query.SingleOrDefault() : query.AsNoTracking().SingleOrDefault();
        }

        public Department? GetOneDepartment(int id, bool trackChanges)
        {
            return FindByCondition(d => d.DepartmentId.Equals(id), trackChanges);
        }

        public Department GetDepartmentWithQuizzes(int departmentId, bool trackChanges)
        {
            var query = _context.Departments
               .Include(d => d.Quizzes)  // İlişkili Quizzes varlıklarını dahil ediyoruz
               .Where(d => d.DepartmentId == departmentId);  // Yalnızca belirli bir Department kaydını alıyoruz

            return trackChanges ? query.SingleOrDefault() : query.AsNoTracking().SingleOrDefault();
        }
    }
}
