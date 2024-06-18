using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManagement.Models.TimeTable;

public class Teacher
{
    [ForeignKey("User")]
    [Key]
    [Display(Name = "Select Teacher")]
    public string Id { get; set; }

    public ApplicationUser User { get; set; }
    public bool IsPhoneNumberVisible { get; set; } = false;
    public bool IsEmailVisible { get; set; } = false;
}