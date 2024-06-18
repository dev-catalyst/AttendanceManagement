#nullable disable
using AttendanceManagement.Data;
using AttendanceManagement.Models.Attendance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Controllers;

public class AttendanceController : Controller
{
    private readonly ApplicationDbContext _context;

    public AttendanceController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Attendance
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.Attendances.Include(a => a.ClassAttendance);
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: Attendance/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null) return NotFound();

        var attendance = await _context.Attendances
            .Include(a => a.ClassAttendance)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (attendance == null) return NotFound();

        return View(attendance);
    }

    // GET: Attendance/Create
    public async Task<IActionResult> Create()
    {
        ViewData["ClassAttendanceId"] = new SelectList(_context.ClassAttendances, "Id", "Id");
        return View();
    }

    // POST: Attendance/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,ClassAttendanceId")] Attendance attendance)
    {
        if (ModelState.IsValid)
        {
            _context.Add(attendance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["ClassAttendanceId"] =
            new SelectList(_context.ClassAttendances, "Id", "Id", attendance.ClassAttendanceId);
        return View(attendance);
    }

    // GET: Attendance/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null) return NotFound();

        var attendance = await _context.Attendances.FindAsync(id);
        if (attendance == null) return NotFound();
        ViewData["ClassAttendanceId"] =
            new SelectList(_context.ClassAttendances, "Id", "Id", attendance.ClassAttendanceId);
        return View(attendance);
    }

    // POST: Attendance/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("Id,ClassAttendanceId")] Attendance attendance)
    {
        if (id != attendance.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(attendance);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendanceExists(attendance.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["ClassAttendanceId"] =
            new SelectList(_context.ClassAttendances, "Id", "Id", attendance.ClassAttendanceId);
        return View(attendance);
    }

    // GET: Attendance/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null) return NotFound();

        var attendance = await _context.Attendances
            .Include(a => a.ClassAttendance)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (attendance == null) return NotFound();

        return View(attendance);
    }

    // POST: Attendance/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var attendance = await _context.Attendances.FindAsync(id);
        _context.Attendances.Remove(attendance);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AttendanceExists(string id)
    {
        return _context.Attendances.Any(e => e.Id == id);
    }
}