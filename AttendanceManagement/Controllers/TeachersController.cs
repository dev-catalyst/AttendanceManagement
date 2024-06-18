#nullable disable
using AttendanceManagement.Data;
using AttendanceManagement.Models;
using AttendanceManagement.Models.TimeTable;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Controllers;

public class TeachersController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public TeachersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // GET: Teachers
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> PageData(IDataTablesRequest request)
    {
        var data = _context.Teachers.Include(t => t.User);

        var filteredData = string.IsNullOrWhiteSpace(request.Search.Value)
            ? data
            : data.Where(_item => _item.User.FirstName.Contains(request.Search.Value)
                                  | _item.User.LastName.Contains(request.Search.Value)
                                  | _item.User.Email.Contains(request.Search.Value)
                                  | _item.User.PhoneNumber.Contains(request.Search.Value)
            );

        var dataPage = filteredData.Skip(request.Start).Take(request.Length);

        var response = DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

        return new DataTablesJsonResult(response, true);
    }


    // GET: Teachers/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null) return NotFound();

        var teacher = await _context.Teachers
            .Include(t => t.User)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (teacher == null) return NotFound();

        return View(teacher);
    }

    // GET: Teachers/Create
    public async Task<IActionResult> Create()
    {
        ViewData["Id"] = new SelectList(_context.Users, "Id", "UserName");
        return View();
    }

    // POST: Teachers/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,IsPhoneNumberVisible,IsEmailVisible")] Teacher teacher)
    {
        teacher.User = _context.Users.Find(teacher.Id);
        ModelState.Remove("User");
        if (ModelState.IsValid && teacher.User != null)
        {
            _context.Add(teacher);
            await _userManager.AddToRoleAsync(teacher.User, "Faculty");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        ViewData["Id"] = new SelectList(_context.Users, "Id", "UserName", teacher.Id);
        return View(teacher);
    }

    // GET: Teachers/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null) return NotFound();

        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null) return NotFound();
        ViewData["Id"] = new SelectList(_context.Users, "Id", "Id", teacher.Id);
        return View(teacher);
    }

    // POST: Teachers/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("Id,IsPhoneNumberVisible,IsEmailVisible")] Teacher teacher)
    {
        if (id != teacher.Id) return NotFound();
        ModelState.Remove("User");
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(teacher);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeacherExists(teacher.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["Id"] = new SelectList(_context.Users, "Id", "UserName", teacher.Id);
        return View(teacher);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string Email)
    {
        var teacher = await _context.Teachers.Where(x => x.User.Email == Email).FirstOrDefaultAsync();
        var department = await _context.Departments.Where(x => x.HOD == teacher.Id).FirstOrDefaultAsync();
        if (department != null)
        {
            var slot = await _context.Slots.Where(x => x.TeacherId == teacher.Id).FirstOrDefaultAsync();
            if (slot != null) _context.Slots.Remove(slot);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }

        // get user
        var user = await _userManager.FindByIdAsync(teacher.Id);
        await _userManager.RemoveFromRoleAsync(user, "Faculty");
        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    private bool TeacherExists(string id)
    {
        return _context.Teachers.Any(e => e.Id == id);
    }
}