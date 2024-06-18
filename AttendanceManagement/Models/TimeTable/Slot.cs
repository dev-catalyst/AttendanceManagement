using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AttendanceManagement.Models.TimeTable;

public class Slot
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public string Id { get; set; }

    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Link { get; set; }
    public int Day { get; set; } = 1;

    [ForeignKey("Teacher")] [JsonIgnore] public string? TeacherId { get; set; }

    [JsonIgnore] public Teacher? Teacher { get; set; }

    [ForeignKey("Course")] [JsonIgnore] public string? CourseId { get; set; }

    [JsonIgnore] public Course? Course { get; set; }

    [ForeignKey("Room")] [JsonIgnore] public string? RoomId { get; set; }

    [JsonIgnore] public Room? Room { get; set; }
}