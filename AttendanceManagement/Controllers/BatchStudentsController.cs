#nullable disable
using AttendanceManagement.Data;
using AttendanceManagement.Models.TimeTable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Controllers;

public class BatchStudentsController : Controller
{
    private readonly ApplicationDbContext _context;

    public BatchStudentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: BatchStudents
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = _context.BatchStudents.Include(b => b.Batch).Include(b => b.Student);
        return View(await applicationDbContext.ToListAsync());
    }

    // GET: BatchStudents/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null) return NotFound();

        var batchStudent = await _context.BatchStudents
            .Include(b => b.Batch)
            .Include(b => b.Student)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (batchStudent == null) return NotFound();

        return View(batchStudent);
    }

    // GET: BatchStudents/Create
    public async Task<IActionResult> Create()
    {
        ViewData["BatchId"] = new SelectList(_context.Batches, "Id", "Id");
        ViewData["StudentId"] = new SelectList(_context.Users, "Id", "Id");
        return View();
    }

    // POST: BatchStudents/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,BatchId,StudentId")] BatchStudent batchStudent)
    {
        if (ModelState.IsValid)
        {
            _context.Add(batchStudent);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["BatchId"] = new SelectList(_context.Batches, "Id", "Id", batchStudent.BatchId);
        ViewData["StudentId"] = new SelectList(_context.Users, "Id", "Id", batchStudent.StudentId);
        return View(batchStudent);
    }

    // GET: BatchStudents/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null) return NotFound();

        var batchStudent = await _context.BatchStudents.FindAsync(id);
        if (batchStudent == null) return NotFound();
        ViewData["BatchId"] = new SelectList(_context.Batches, "Id", "Id", batchStudent.BatchId);
        ViewData["StudentId"] = new SelectList(_context.Users, "Id", "Id", batchStudent.StudentId);
        return View(batchStudent);
    }

    // POST: BatchStudents/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("Id,BatchId,StudentId")] BatchStudent batchStudent)
    {
        if (id != batchStudent.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(batchStudent);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BatchStudentExists(batchStudent.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["BatchId"] = new SelectList(_context.Batches, "Id", "Id", batchStudent.BatchId);
        ViewData["StudentId"] = new SelectList(_context.Users, "Id", "Id", batchStudent.StudentId);
        return View(batchStudent);
    }

    // GET: BatchStudents/Delete/5
    public async Task<IActionResult> Delete(string id)
    {
        if (id == null) return NotFound();

        var batchStudent = await _context.BatchStudents
            .Include(b => b.Batch)
            .Include(b => b.Student)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (batchStudent == null) return NotFound();

        return View(batchStudent);
    }

    // POST: BatchStudents/Delete/5
    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var batchStudent = await _context.BatchStudents.FindAsync(id);
        _context.BatchStudents.Remove(batchStudent);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BatchStudentExists(string id)
    {
        return _context.BatchStudents.Any(e => e.Id == id);
    }
}