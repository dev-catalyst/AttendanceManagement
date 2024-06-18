using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManagement.Models.TimeTable;

public class Department
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public string Id { get; set; }

    public string Name { get; set; }
    public string? Description { get; set; }

    [ForeignKey("Teacher")] public string? HOD { get; set; }

    public Teacher? Teacher { get; set; }
}