#nullable disable
using AttendanceManagement.Data;
using AttendanceManagement.Models.TimeTable;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Controllers;

public class RoomsController : Controller
{
    private readonly ApplicationDbContext _context;

    public RoomsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Rooms
    public async Task<IActionResult> Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> PageData(IDataTablesRequest request)
    {
        var data = _context.Rooms;

        var filteredData = string.IsNullOrWhiteSpace(request.Search.Value)
            ? data
            : data.Where(_item => _item.Name.Contains(request.Search.Value)
                                  | _item.Description.Contains(request.Search.Value)
                                  | _item.Code.Contains(request.Search.Value)
                                  | _item.Floor.ToString().Contains(request.Search.Value)
                                  | _item.BuildingName.Contains(request.Search.Value)
            );

        var dataPage = filteredData.Skip(request.Start).Take(request.Length);

        var response = DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

        return new DataTablesJsonResult(response, true);
    }


    // GET: Rooms/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null) return NotFound();

        var room = await _context.Rooms
            .FirstOrDefaultAsync(m => m.Id == id);
        if (room == null) return NotFound();

        return View(room);
    }

    // GET: Rooms/Create
    public async Task<IActionResult> Create()
    {
        return View();
    }

    // POST: Rooms/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Code,Name,Description,BuildingName,Floor")] Room room)
    {
        room.Id = Guid.NewGuid().ToString();
        ModelState.Remove("Id");
        if (ModelState.IsValid)
        {
            _context.Add(room);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(room);
    }

    // GET: Rooms/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null) return NotFound();

        var room = await _context.Rooms.FindAsync(id);
        if (room == null) return NotFound();

        return View(room);
    }

    // POST: Rooms/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id,
        [Bind("Id,Code,Name,Description,BuildingName,Floor")]
        Room room)
    {
        if (id != room.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(room);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoomExists(room.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(room);
    }


    // POST: Rooms/Delete/5
    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        var room = await _context.Rooms.FindAsync(id);
        _context.Rooms.Remove(room);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool RoomExists(string id)
    {
        return _context.Rooms.Any(e => e.Id == id);
    }
}