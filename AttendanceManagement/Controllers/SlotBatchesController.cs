#nullable disable
using AttendanceManagement.Data;
using AttendanceManagement.Models.TimeTable;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Controllers;

public class SlotBatchesController : Controller
{
    private static string currentId = string.Empty;
    private readonly ApplicationDbContext _context;

    public SlotBatchesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: SlotBatches
    public async Task<IActionResult> Index(string id)
    {
        if (id != "")
            currentId = id;
        ViewBag.SlotId = id;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> PageData(IDataTablesRequest request)
    {
        var data = _context.SlotBatches
            .Include(x => x.Batch)
            .Include(x => x.Slot)
            .Where(x => x.SlotId == currentId);

        var filteredData = string.IsNullOrWhiteSpace(request.Search.Value)
            ? data
            : data.Where(_item => _item.Batch.Name.Contains(request.Search.Value) |
                                  _item.Slot.Teacher.User.Email.Contains(request.Search.Value)
                                  && _item.Slot.Id == currentId);

        var dataPage = filteredData.Skip(request.Start).Take(request.Length);

        var response = DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

        return new DataTablesJsonResult(response, true);
    }


    // GET: SlotBatches/Details/5
    public async Task<IActionResult> Details(string id)
    {
        if (id == null) return NotFound();

        var slotBatch = await _context.SlotBatches
            .Include(s => s.Batch)
            .Include(s => s.Slot)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (slotBatch == null) return NotFound();

        return View(slotBatch);
    }

    // GET: SlotBatches/Create
    public async Task<IActionResult> Create(string id)
    {
        var slot = _context.Slots.FirstOrDefault(x => x.Id == id);
        var course = _context.Courses.FirstOrDefault(x => x.Id == slot.CourseId);
        var batches = await _context.BatchCourses.Where(x => x.CourseId == course.Id).ToListAsync();
        var batchList = new List<BatchList>();
        foreach (var batch in batches)
            batchList.Add(new BatchList
            {
                Name = _context.Batches.FirstOrDefault(x => x.Id == batch.BatchId).Name,
                Id = batch.BatchId
            });
        ViewData["BatchId"] = new SelectList(batchList, "Id", "Name");
        ViewData["SlotId"] = new SelectList(_context.Slots, "Id", "Id", currentId);
        ViewBag.currentId = currentId;
        return View();
    }

    // POST: SlotBatches/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,SlotId,BatchId")] SlotBatch slotBatch)
    {
        slotBatch.Id = Guid.NewGuid().ToString();
        ModelState.Remove("Id");
        ModelState.Remove("Slot");
        ModelState.Remove("Batch");

        if (ModelState.IsValid)
        {
            slotBatch.Batch = _context.Batches.Find(slotBatch.BatchId);
            slotBatch.Slot = _context.Slots.Find(slotBatch.SlotId);
            if (slotBatch.Batch != null || slotBatch.Slot != null)
            {
                _context.Add(slotBatch);
                await _context.SaveChangesAsync();
                return Ok(slotBatch.Id);
            }

            ViewData["BatchId"] = new SelectList(_context.Batches, "Id", "Id", slotBatch.BatchId);
            ViewData["SlotId"] = new SelectList(_context.Slots, "Id", "Id", slotBatch.SlotId);
            return View(slotBatch);
        }

        ViewData["BatchId"] = new SelectList(_context.Batches, "Id", "Id", slotBatch.BatchId);
        ViewData["SlotId"] = new SelectList(_context.Slots, "Id", "Id", slotBatch.SlotId);
        return View(slotBatch);
    }

    // GET: SlotBatches/Edit/5
    public async Task<IActionResult> Edit(string id)
    {
        if (id == null) return NotFound();

        var slotBatch = await _context.SlotBatches.FindAsync(id);
        if (slotBatch == null) return NotFound();

        ViewData["BatchId"] = new SelectList(_context.Batches, "Id", "Id", slotBatch.BatchId);
        ViewData["SlotId"] = new SelectList(_context.Slots, "Id", "Id", slotBatch.SlotId);
        return View(slotBatch);
    }

    // POST: SlotBatches/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("Id,SlotId,BatchId")] SlotBatch slotBatch)
    {
        if (id != slotBatch.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(slotBatch);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SlotBatchExists(slotBatch.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        ViewData["BatchId"] = new SelectList(_context.Batches, "Id", "Id", slotBatch.BatchId);
        ViewData["SlotId"] = new SelectList(_context.Slots, "Id", "Id", slotBatch.SlotId);
        return View(slotBatch);
    }


    [HttpDelete]
    public async Task<IActionResult> DeleteSlotBatch(string id)
    {
        var slotBatch = await _context.SlotBatches.FindAsync(id);
        _context.SlotBatches.Remove(slotBatch);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool SlotBatchExists(string id)
    {
        return _context.SlotBatches.Any(e => e.Id == id);
    }
}