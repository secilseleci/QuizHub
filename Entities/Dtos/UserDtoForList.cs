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

        public string Department { get; set; }  // Departman adı buraya set ediliyor



        [Required(ErrorMessage = "Department is required.")]  // Eğer department zorunlu ise
        public int DepartmentId { get; set; }

        public List<string> Roles { get; set; } = new List<string>(); // Roller
    }
}
