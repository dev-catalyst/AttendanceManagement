using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManagement.Models.Attendance;

public class ClassStudentAttendacne
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public string Id { get; set; }

    public string IPAddress { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public DateTime? TimeStamp { get; set; } = DateTime.Now;

    [ForeignKey("Student")] public string? StudentId { get; set; }

    [ForeignKey("Class")] public string? ClassId { get; set; }

    [ForeignKey("Attendance")] public string? AttendanceId { get; set; }

    public ApplicationUser? Student { get; set; }
    public ClassAttendance? Class { get; set; }
    public Attendance? Attendance { get; set; }
}