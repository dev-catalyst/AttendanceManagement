#nullable disable
using AttendanceManagement.Data;
using AttendanceManagement.Models.Attendance;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Controllers;

public class ClassAttendanceController : Controller
{
    private readonly ApplicationDbContext _context;

    public ClassAttendanceController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: ClassAttendance
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.ClassAttendances.Include(c => c.Slot);
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: ClassAttendance/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null) return NotFound();

        var classAttendance = await _context.ClassAttendances
            .Include(c => c.Slot)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (classAttendance == null) return NotFound();

        return View(classAttendance);
    }

    // GET: ClassAttendance/Create
    public async Task<IActionResult> Create()
    {
        ViewData["SlotId"] = new SelectList(_context.Slots, "Id", "Id");
        return View();
    }

    // POST: ClassAttendance/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,SlotId,Date")] ClassAttendance classAttendance)
    {
        if (ModelState.IsValid)
        {
            _context.Add(classAttendance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["SlotId"] = new SelectList(_context.Slots, "Id", "Id", classAttendance.SlotId);
        return View(classAttendance);
    }

    // GET: ClassAttendance/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null) return NotFound();

        var classAttendance = await _context.ClassAttendances.FindAsync(id);
        if (classAttendance == null) return NotFound();
        ViewData["SlotId"] = new SelectList(_context.Slots, "Id", "Id", classAttendance.SlotId);
        return View(classAttendance);
    }

    // POST: ClassAttendance/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("Id,SlotId,Date")] ClassAttendance classAttendance)
    {
        if (id != classAttendance.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(classAttendance);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClassAttendanceExists(classAttendance.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["SlotId"] = new SelectList(_context.Slots, "Id", "Id", classAttendance.SlotId);
        return View(classAttendance);
    }

    // GET: ClassAttendance/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null) return NotFound();

        var classAttendance = await _context.ClassAttendances
            .Include(c => c.Slot)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (classAttendance == null) return NotFound();

        return View(classAttendance);
    }

    // POST: ClassAttendance/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var classAttendance = await _context.ClassAttendances.FindAsync(id);
        _context.ClassAttendances.Remove(classAttendance);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ClassAttendanceExists(string id)
    {
        return _context.ClassAttendances.Any(e => e.Id == id);
    }
}