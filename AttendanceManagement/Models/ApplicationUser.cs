using System.ComponentModel.DataAnnotations.Schema;
using AttendanceManagement.Models.TimeTable;
using Microsoft.AspNetCore.Identity;

namespace AttendanceManagement.Models;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Image { get; set; }

    [ForeignKey("Department")] public string? DepartmentId { get; set; }

    public Department? Department { get; set; }

    public string? EnrollmentNumber { get; set; }
    public int? Semester { get; set; }
}