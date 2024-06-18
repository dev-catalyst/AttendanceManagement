using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManagement.Models.TimeTable;

public class Room
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public string Id { get; set; }

    public string Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? BuildingName { get; set; }
    public int? Floor { get; set; }
}