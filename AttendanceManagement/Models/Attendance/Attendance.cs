using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManagement.Models.Attendance;

public class Attendance
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public string Id { get; set; }

    [ForeignKey("ClassAttendance")] public string ClassAttendanceId { get; set; }

    public ClassAttendance ClassAttendance { get; set; }
}