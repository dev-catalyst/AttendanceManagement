#nullable disable
using AttendanceManagement.Data;
using AttendanceManagement.Models.Attendance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Controllers;

[Route("ClassStudentsAttendance/[action]")]
public class ClassStudentAttendance : Controller
{
    private readonly ApplicationDbContext _context;

    public ClassStudentAttendance(ApplicationDbContext context)
    {
        _context = context;
    }

    [Route("/ClassStudentsAttendance")]
    // GET: ClassStudentAttendance
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.ClassStudentAttendances.Include(c => c.Attendance).Include(c => c.Class)
            .Include(c => c.Student);
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: ClassStudentAttendance/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null) return NotFound();

        var classStudentAttendacne = await _context.ClassStudentAttendances
            .Include(c => c.Attendance)
            .Include(c => c.Class)
            .Include(c => c.Student)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (classStudentAttendacne == null) return NotFound();

        return View(classStudentAttendacne);
    }

    // GET: ClassStudentAttendance/Create
    public async Task<IActionResult> Create()
    {
        ViewData["AttendanceId"] = new SelectList(_context.Attendances, "Id", "Id");
        ViewData["ClassId"] = new SelectList(_context.ClassAttendances, "Id", "Id");
        ViewData["StudentId"] = new SelectList(_context.Users, "Id", "Id");
        return View();
    }

    // POST: ClassStudentAttendance/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,IPAddress,Latitude,Longitude,TimeStamp,StudentId,ClassId,AttendanceId")]
        ClassStudentAttendacne classStudentAttendacne)
    {
        if (ModelState.IsValid)
        {
            _context.Add(classStudentAttendacne);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["AttendanceId"] =
            new SelectList(_context.Attendances, "Id", "Id", classStudentAttendacne.AttendanceId);
        ViewData["ClassId"] = new SelectList(_context.ClassAttendances, "Id", "Id", classStudentAttendacne.ClassId);
        ViewData["StudentId"] = new SelectList(_context.Users, "Id", "Id", classStudentAttendacne.StudentId);
        return View(classStudentAttendacne);
    }

    // GET: ClassStudentAttendance/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null) return NotFound();

        var classStudentAttendacne = await _context.ClassStudentAttendances.FindAsync(id);
        if (classStudentAttendacne == null) return NotFound();
        ViewData["AttendanceId"] =
            new SelectList(_context.Attendances, "Id", "Id", classStudentAttendacne.AttendanceId);
        ViewData["ClassId"] = new SelectList(_context.ClassAttendances, "Id", "Id", classStudentAttendacne.ClassId);
        ViewData["StudentId"] = new SelectList(_context.Users, "Id", "Id", classStudentAttendacne.StudentId);
        return View(classStudentAttendacne);
    }

    // POST: ClassStudentAttendance/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id,
        [Bind("Id,IPAddress,Latitude,Longitude,TimeStamp,StudentId,ClassId,AttendanceId")]
        ClassStudentAttendacne classStudentAttendacne)
    {
        if (id != classStudentAttendacne.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(classStudentAttendacne);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassStudentAttendacneExists(classStudentAttendacne.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["AttendanceId"] =
            new SelectList(_context.Attendances, "Id", "Id", classStudentAttendacne.AttendanceId);
        ViewData["ClassId"] = new SelectList(_context.ClassAttendances, "Id", "Id", classStudentAttendacne.ClassId);
        ViewData["StudentId"] = new SelectList(_context.Users, "Id", "Id", classStudentAttendacne.StudentId);
        return View(classStudentAttendacne);
    }

    // GET: ClassStudentAttendance/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null) return NotFound();

        var classStudentAttendacne = await _context.ClassStudentAttendances
            .Include(c => c.Attendance)
            .Include(c => c.Class)
            .Include(c => c.Student)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (classStudentAttendacne == null) return NotFound();

        return View(classStudentAttendacne);
    }

    // POST: ClassStudentAttendance/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var classStudentAttendacne = await _context.ClassStudentAttendances.FindAsync(id);
        _context.ClassStudentAttendances.Remove(classStudentAttendacne);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ClassStudentAttendacneExists(string id)
    {
        return _context.ClassStudentAttendances.Any(e => e.Id == id);
    }
}