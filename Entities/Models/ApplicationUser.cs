using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required] 
        public int DepartmentId { get; set; }  


        public Department Department { get; set; }
    }
}
