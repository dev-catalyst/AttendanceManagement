using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManagement.Models
{
    public class Visitor
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public string Id { get; set; }
        public string? IpAddress { get; set; }
        public DateTime time {get; set; }
        public string? VisitedId { get; set; }
        public string? Path { get; set; }
        public string? FormData { get; set; }
        public virtual string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? users { get; set; }
    }
}
