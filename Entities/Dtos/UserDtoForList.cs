using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class UserDtoForList
    {
        public string Id { get; init; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "UserName is required.")]
        public string? UserName { get; init; }

        public string DepartmentName { get; set; }   



        [Required(ErrorMessage = "Department is required.")]  
        public int DepartmentId { get; init; }

        public List<string> Roles { get; set; } = new List<string>();  
    }
}
