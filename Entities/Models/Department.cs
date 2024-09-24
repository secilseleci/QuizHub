using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }

        // Kullanıcılarla ilişki
        public ICollection<ApplicationUser> Users { get; set; }  // IdentityUser yerine ApplicationUser kullanıyoruz
    }
}
