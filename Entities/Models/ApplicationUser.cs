using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? DepartmentId { get; set; } // Kullanıcının departmanı için ID

        // Navigasyon özelliği
        public Department Department { get; set; }
    }
}
