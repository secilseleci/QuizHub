using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public record UserDto
{   public string Id { get; init; }
    
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "UserName is required.")]
    public string? UserName { get; init; }
   
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Email is required.")]
    public string? Email { get; init; }


    [Required(ErrorMessage = "Department is required.")]  // Eğer department zorunlu ise
    public int DepartmentId { get; set; }

     public SelectList Departments { get; set; }  // Departmanların seçilebilmesi için SelectList
    public string Department { get; set; } // Department adı

    public List<string> Roles { get; set; } = new List<string>(); // Roller
}
