#nullable disable
using AttendanceManagement.Data;
using AttendanceManagement.Models.TimeTable;
using AttendanceManagement.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Controllers;

public class SlotsController : Controller
{
    private readonly ApplicationDbContext _context;

    public SlotsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Slots
    public async Task<IActionResult> Index()
    {
        return View();
    }

    // GET: Slots/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null) return NotFound();

        var slot = await _context.Slots
            .Include(s => s.Course)
            .Include(s => s.Room)
            .Include(s => s.Teacher)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (slot == null) return NotFound();

        return View(slot);
    }

    // GET: Slots/Create
    public async Task<IActionResult> Create()
    {
        IList<CourseViewModel> courses = await _context.Courses.Select(c => new CourseViewModel
        {
            Id = c.Id,
            Name = c.Name + " ( " + c.Semester + "Sem )"
        }).ToListAsync();
        ViewData["CourseId"] = new SelectList(courses, "Id", "Name");
        ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name");
        ViewData["TeacherId"] = new SelectList(_context.Teachers.Include(x => x.User), "Id", "User.Email");
        return View();
    }

    // POST: Slots/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [Bind("Id,StartTime,EndTime,Link,Day,TeacherId,CourseId,RoomId")]
        Slot slot)
    {
        slot.Id = Guid.NewGuid().ToString();
        slot.Course = _context.Courses.FirstOrDefault(x => x.Id == slot.CourseId);
        slot.Room = _context.Rooms.FirstOrDefault(x => x.Id == slot.RoomId);
        slot.Teacher = _context.Teachers.FirstOrDefault(x => x.Id == slot.TeacherId);
        ModelState.Remove("Id");
        ModelState.Remove("StartTime");
        ModelState.Remove("EndTime");
        if (ModelState.IsValid && slot.StartTime < slot.EndTime && slot.StartTime != null && slot.EndTime != null &&
            slot.Id != null)
        {
            _context.Add(slot);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", slot.CourseId);
        ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", slot.RoomId);
        ViewData["TeacherId"] =
            new SelectList(_context.Teachers.Include(x => x.User), "Id", "User.Email", slot.TeacherId);
        return View(slot);
    }

    // GET: Slots/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null) return NotFound();

        var slot = await _context.Slots.FindAsync(id);
        if (slot == null) return NotFound();

        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", slot.CourseId);
        ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", slot.RoomId);
        ViewData["TeacherId"] =
            new SelectList(_context.Teachers.Include(x => x.User), "Id", "User.Email", slot.TeacherId);
        return View(slot);
    }

    [HttpPost]
    public async Task<IActionResult> GetBatchesBySlotId(string slotId)
    {
        if (slotId == null)
            return NotFound();

        var batches = await _context.SlotBatches.Where(x => x.SlotId == slotId).ToListAsync();
        return Json(batches);
    }

    // POST: Slots/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id,
        [Bind("Id,StartTime,EndTime,Link,Day,TeacherId,CourseId,RoomId")]
        Slot slot)
    {
        if (id != slot.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(slot);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SlotExists(slot.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Name", slot.CourseId);
        ViewData["RoomId"] = new SelectList(_context.Rooms, "Id", "Name", slot.RoomId);
        ViewData["TeacherId"] =
            new SelectList(_context.Teachers.Include(x => x.User), "Id", "User.Email", slot.TeacherId);
        return View(slot);
    }


    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        var slot = await _context.Slots.FindAsync(id);
        _context.Slots.Remove(slot);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SlotExists(string id)
    {
        return _context.Slots.Any(e => e.Id == id);
    }
}