using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManagement.Models.TimeTable;

public class BatchCourse
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public string Id { get; set; }

    [ForeignKey("Batch")] [Required] public string BatchId { get; set; }

    public Batch Batch { get; set; }


    [ForeignKey("Course")] [Required] public string CourseId { get; set; }

    public Course Course { get; set; }
}