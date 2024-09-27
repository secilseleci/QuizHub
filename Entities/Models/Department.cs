﻿namespace Entities.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public ICollection<Quiz> Quizzes { get; set; }
        public ICollection<ApplicationUser> Users { get; set; }  
    }
}
