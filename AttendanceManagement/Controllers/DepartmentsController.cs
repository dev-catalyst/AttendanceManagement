#nullable disable
using AttendanceManagement.Data;
using AttendanceManagement.Models.TimeTable;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Controllers;

public class DepartmentsController : Controller
{
    private readonly ApplicationDbContext _context;

    public DepartmentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Departments
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> PageData(IDataTablesRequest request)
    {
        var data = _context.Departments.Include(t => t.Teacher).Include(x => x.Teacher.User);

        var filteredData = string.IsNullOrWhiteSpace(request.Search.Value)
            ? data
            : data.Where(_item => _item.Teacher.User.FirstName.Contains(request.Search.Value)
                                  | _item.Teacher.User.LastName.Contains(request.Search.Value)
                                  | _item.Name.Contains(request.Search.Value)
                                  | _item.Description.Contains(request.Search.Value)
                                  | _item.Teacher.User.Email.Contains(request.Search.Value)
                                  | _item.Teacher.User.PhoneNumber.Contains(request.Search.Value)
            );

        var dataPage = filteredData.Skip(request.Start).Take(request.Length);

        var response = DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

        return new DataTablesJsonResult(response, true);
    }


    // GET: Departments/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null) return NotFound();

        var department = await _context.Departments
            .Include(d => d.Teacher)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (department == null) return NotFound();

        return View(department);
    }

    // GET: Departments/Create
    public async Task<IActionResult> Create()
    {
        ViewData["HOD"] = new SelectList(_context.Teachers.Include(x => x.User), "Id", "User.Email");
        return View();
    }

    // POST: Departments/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Description,HOD")] Department department)
    {
        department.Id = Guid.NewGuid().ToString();
        ModelState.Remove("Id");
        if (ModelState.IsValid && department.Id != null)
        {
            _context.Add(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        {
            _context.Add(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["HOD"] = new SelectList(_context.Teachers.Include(x => x.User), "Id", "User.Email", department.HOD);
        return View(department);
    }

    // GET: Departments/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null) return NotFound();

        var department = await _context.Departments.FindAsync(id);
        if (department == null) return NotFound();
        ViewData["HOD"] = new SelectList(_context.Teachers.Include(x => x.User), "Id", "User.Email", department.HOD);
        return View(department);
    }

    // POST: Departments/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Description,HOD")] Department department)
    {
        if (id != department.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(department);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DepartmentExists(department.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["HOD"] = new SelectList(_context.Teachers.Include(x => x.User), "Id", "User.Email", department.HOD);
        return View(department);
    }


    // POST: Departments/Delete/5
    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        var department = await _context.Departments.FindAsync(id);
        _context.Departments.Remove(department);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DepartmentExists(string id)
    {
        return _context.Departments.Any(e => e.Id == id);
    }
}