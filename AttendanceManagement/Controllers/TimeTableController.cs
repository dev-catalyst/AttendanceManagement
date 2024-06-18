using AttendanceManagement.Data;
using AttendanceManagement.Models.TimeTable;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Controllers;

public class TimeTableController : Controller
{
    private readonly ApplicationDbContext _context;

    public TimeTableController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET
    public async Task<IActionResult> Index(string id, string teacher, string room)
    {
        ViewBag.allBatches = await _context.Batches.ToListAsync();
        ViewBag.allTeachers = await _context.Teachers.Include(x => x.User).ToListAsync();
        ViewBag.allRooms = await _context.Rooms.ToListAsync();
        var todayDateInt = InverseDayOfTheWeek(DateTime.Today.DayOfWeek.ToString());
        if (id != null)
        {
            ViewBag.Id = id;
            var slots = await GetByBatch(id);
            ViewBag.slots = slots;
            var todayLectures = slots.Where(x => x.Day == todayDateInt).ToList();
            ViewBag.todayLectures = todayLectures;
        }
        else if (teacher != null)
        {
            ViewBag.Id = teacher;
            var slots = await GetByTeacher(teacher);
            ViewBag.slots = slots;
            var todayLectures = slots.Where(x => x.Day == todayDateInt).ToList();
            ViewBag.todayLectures = todayLectures;
        }
        else if (room != null)
        {
            ViewBag.Id = room;
            var slots = await GetByRoom(room);
            ViewBag.slots = slots;
            var todayLectures = slots.Where(x => x.Day == todayDateInt).ToList();
            ViewBag.todayLectures = todayLectures;
        }

        return View();
    }


    private async Task<List<Slot>> GetByRoom(string roomId)
    {
        var slots = await _context.Slots
            .Where(x => x.RoomId == roomId)
            .OrderBy(x => x.Day)
            .Include(x => x.Course)
            .Include(x => x.Room)
            .Include(x => x.Teacher.User)
            .ToListAsync();

        return slots;
    }


    private async Task<List<Slot>> GetByBatch(string id)
    {
        List<SlotBatch> slot_Batches = await _context.SlotBatches.Where(x => x.BatchId == id).ToListAsync();
        ViewBag.slot_Batches = slot_Batches;

        List<string> _slotIds = slot_Batches.Select(x => x.SlotId).ToList();

        // get slots data from slot_batches
        List<Slot> slots = await _context.Slots
            .Where(x => _slotIds.Contains(x.Id))
            .OrderBy(x => x.Day)
            .Include(x => x.Course)
            .Include(x => x.Room)
            .Include(x => x.Teacher.User)
            .ToListAsync();

        return slots;
    }


    private async Task<List<Slot>> GetByTeacher(string teacherId)
    {
        var slots = await _context.Slots
            .Where(x => x.TeacherId == teacherId)
            .OrderBy(x => x.Day)
            .Include(x => x.Course)
            .Include(x => x.Room)
            .Include(x => x.Teacher.User)
            .ToListAsync();
        return slots;
    }

    private string FindDayOfTheWeek(int day)
    {
        if (day == 1)
            return "Monday";
        if (day == 2)
            return "Tuesday";
        if (day == 3)
            return "Wednesday";
        if (day == 4)
            return "Thursday";
        if (day == 5)
            return "Friday";
        if (day == 6)
            return "Saturday";
        if (day == 7)
            return "Sunday";
        return "Invalid Day";
    }

    private int InverseDayOfTheWeek(string day)
    {
        if (day == "Monday")
            return 1;
        if (day == "Tuesday")
            return 2;
        if (day == "Wednesday")
            return 3;
        if (day == "Thursday")
            return 4;
        if (day == "Friday")
            return 5;
        if (day == "Saturday")
            return 6;
        if (day == "Sunday")
            return 7;
        return 0;
    }
}