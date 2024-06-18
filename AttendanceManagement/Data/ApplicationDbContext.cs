using AttendanceManagement.Models;
using AttendanceManagement.Models.Attendance;
using AttendanceManagement.Models.TimeTable;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;   
    }

    public DbSet<Batch> Batches { get; set; }
    public DbSet<BatchStudent> BatchStudents { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<Slot> Slots { get; set; }
    public DbSet<SlotBatch> SlotBatches { get; set; }
    public DbSet<Teacher> Teachers { get; set; }
    
    public DbSet<Visitor> Visitors { get; set; }

    public DbSet<Attendance> Attendances { get; set; }

    public DbSet<ClassAttendance> ClassAttendances { get; set; }

    public DbSet<ClassStudentAttendacne> ClassStudentAttendances { get; set; }

    public DbSet<BatchCourse> BatchCourses { get; set; }

    public DbSet<StudentBatch> StudentBatches { get; set; }
}