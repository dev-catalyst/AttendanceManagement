using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AttendanceManagement.Models.TimeTable;

namespace AttendanceManagement.Models;

public class StudentBatch
{
    [Key] public string Id { get; set; }

    [ForeignKey("User")] public string UserId { get; set; }

    public ApplicationUser User { get; set; }

    [ForeignKey("Batch")] public string BatchId { get; set; }

    public Batch? Batch { get; set; }
}