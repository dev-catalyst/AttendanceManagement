using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AttendanceManagement.Models.TimeTable;

namespace AttendanceManagement.Models.Attendance;

public class ClassAttendance
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public string Id { get; set; }

    public string SlotId { get; set; }
    public Slot Slot { get; set; }
    public DateTime Date { get; set; }
}