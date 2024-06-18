using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManagement.Models.TimeTable;

public class BatchStudent
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public string Id { get; set; }

    [ForeignKey("Batch")] [Required] public string BatchId { get; set; }

    public Batch Batch { get; set; }


    [ForeignKey("Student")] [Required] public string StudentId { get; set; }

    public ApplicationUser Student { get; set; }
}