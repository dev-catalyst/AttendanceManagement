using AttendanceManagement.Data;
using AttendanceManagement.Models.TimeTable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Controllers;

public class BatchCoursesController : Controller
{
    private readonly ApplicationDbContext _context;

    public BatchCoursesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET
    public async Task<IActionResult> Index(string id)
    {
        var batch = _context.Batches.Where(x => x.Id == id).FirstOrDefault();
        ViewBag.selectedBatches = await _context.BatchCourses.Where(x => x.BatchId == id).ToListAsync();

        var courses = await _context.Courses
            .Where(x => x.Semester == batch.Semester && x.DepartmentId == batch.DepartmentId).ToListAsync();

        ViewBag.Courses = courses;
        ViewBag.BatchId = id;

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Update(string[] Courses, string Batch)
    {
        var allCourses = await _context.BatchCourses.Where(x => x.BatchId == Batch).ToListAsync();
        for (var i = 0; i < allCourses.Count; i++)
        for (var j = 0; j < Courses.Length; j++)
            if (allCourses[i].CourseId == Courses[j])
            {
                allCourses.Remove(allCourses[i]);
                break;
            }

        var coursesToBeRemoved = allCourses;
        _context.BatchCourses.RemoveRange(coursesToBeRemoved);
        await _context.SaveChangesAsync();
        for (var i = 0; i < Courses.Length; i++)
            if (_context.BatchCourses.Where(x => x.BatchId == Batch && x.CourseId == Courses[i]).FirstOrDefault() ==
                null)
            {
                var batchCourse = new BatchCourse
                {
                    BatchId = Batch,
                    CourseId = Courses[i]
                };
                _context.BatchCourses.Add(batchCourse);
            }

        await _context.SaveChangesAsync();
        return Ok("Success");
    }
}