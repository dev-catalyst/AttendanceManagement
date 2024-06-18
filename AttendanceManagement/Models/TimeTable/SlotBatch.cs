using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManagement.Models.TimeTable;

public class SlotBatch
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public string Id { get; set; }

    [ForeignKey("Slot")] [Required] public string SlotId { get; set; }

    public Slot Slot { get; set; }

    [ForeignKey("Batch")] [Required] public string BatchId { get; set; }

    public Batch Batch { get; set; }
}