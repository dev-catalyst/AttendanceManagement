#nullable disable
using AttendanceManagement.Data;
using AttendanceManagement.Models.TimeTable;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Controllers;

public class BatchesController : Controller
{
    private readonly ApplicationDbContext _context;

    public BatchesController(ApplicationDbContext context)
    {
        _context = context;
    }


    [HttpPost]
    public async Task<IActionResult> PageData(IDataTablesRequest request)
    {
        var data = _context.Batches.Include(t => t.Department);

        var filteredData = string.IsNullOrWhiteSpace(request.Search.Value)
            ? data
            : data.Where(_item => _item.Name.Contains(request.Search.Value)
                                  | _item.Description.Contains(request.Search.Value)
                                  | _item.Semester.ToString().Contains(request.Search.Value)
                                  | _item.Code.Contains(request.Search.Value)
                                  | _item.Department.Name.Contains(request.Search.Value)
            );

        var dataPage = filteredData.Skip(request.Start).Take(request.Length);

        var response = DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

        return new DataTablesJsonResult(response, true);
    }


    // GET: Batches
    public async Task<IActionResult> Index()
    {
        return View();
    }

    // GET: Batches/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null) return NotFound();

        var batch = await _context.Batches
            .Include(b => b.Department)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (batch == null) return NotFound();

        return View(batch);
    }

    // GET: Batches/Create
    public async Task<IActionResult> Create()
    {
        ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Semester,Name,Description,Code,DepartmentId")] Batch batch)
    {
        batch.Id = Guid.NewGuid().ToString();
        ModelState.Remove("Id");
        if (ModelState.IsValid)
        {
            _context.Add(batch);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", batch.DepartmentId);
        return View(batch);
    }

    public async Task<IActionResult> Edit(string id)
    {
        if (id == null) return NotFound();

        var batch = await _context.Batches.FindAsync(id);
        if (batch == null) return NotFound();

        ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", batch.DepartmentId);
        return View(batch);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id,
        [Bind("Id,Semester,Name,Description,Code,DepartmentId")] Batch batch)
    {
        if (id != batch.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(batch);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BatchExists(batch.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", batch.DepartmentId);
        return View(batch);
    }


    // POST: Batches/Delete/5
    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        var batch = await _context.Batches.FindAsync(id);
        _context.Batches.Remove(batch);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BatchExists(string id)
    {
        return _context.Batches.Any(e => e.Id == id);
    }
}