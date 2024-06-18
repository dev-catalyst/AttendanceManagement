using System.Diagnostics;
using AttendanceManagement.Data;
using AttendanceManagement.Models;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    private readonly ILogger<HomeController> _logger;

    // _usermanager
    private readonly UserManager<ApplicationUser> _userManager;

    public HomeController(ILogger<HomeController> logger,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
        ViewBag.User = user;
        var role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
        ViewBag.Role = role;
        if (role == "Student")
        {
            var studentEnrolledBatches = _context.StudentBatches.Where(s => s.UserId == user.Id).ToList();
            if (studentEnrolledBatches.Count == 0)
            {
                var batches = _context.Batches
                    .Where(x => x.DepartmentId == user.DepartmentId && x.Semester == user.Semester).ToList();
                ViewBag.Batches = batches;
            }
        }

        if (role == "Faculty")
        {
            var facultySlots = _context.Slots.Where(x => x.TeacherId == user.Id)
                .Include(x => x.Course)
                .Include(x => x.Room)
                .Include(x => x.Teacher.User)
                .ToList();
            var today = InverseDayOfTheWeek(DateTime.Now.DayOfWeek.ToString());
            var TodayFacultySlots = facultySlots.Where(x => x.Day == today).ToList();
            ViewBag.TodayFacultySlots = TodayFacultySlots;
            ViewBag.facultySlots = facultySlots;
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddEnrollment(string EnrollmentNumber)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
        user.EnrollmentNumber = EnrollmentNumber;
        _context.Update(user);
        await _context.SaveChangesAsync();
        return Ok("success");
    }

    [HttpPost]
    public async Task<IActionResult> AddBatch(string[] content)
    {
        var user = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
        var studentBatch = new List<StudentBatch>();
        foreach (var item in content)
            studentBatch.Add(new StudentBatch
            {
                Id = Guid.NewGuid().ToString(),
                UserId = user.Id,
                BatchId = item
            });
        _context.StudentBatches.AddRange(studentBatch);
        await _context.SaveChangesAsync();
        return Ok("success");
    }


    [Route("/Privacy")]
    public async Task<IActionResult> Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public async Task<IActionResult> Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpPost]
    public async Task<IActionResult> SlotPageData(IDataTablesRequest request)
    {
        var data = _context.Slots
                .Include(s => s.Course)
                .Include(s => s.Room)
                .Include(s => s.Teacher.User)
            ;

        var filteredData = string.IsNullOrWhiteSpace(request.Search.Value)
            ? data
            : data.Where(_item => _item.StartTime.ToString().Contains(request.Search.Value)
                                  | _item.EndTime.ToString().Contains(request.Search.Value)
                                  | _item.Course.Name.ToString().Contains(request.Search.Value)
                                  | _item.Room.Name.ToString().Contains(request.Search.Value)
                                  | _item.Link.Contains(request.Search.Value)
                                  | _item.Day.ToString().Contains(request.Search.Value)
                                  | _item.Teacher.User.FirstName.Contains(request.Search.Value)
                                  | _item.Teacher.User.LastName.Contains(request.Search.Value)
            );

        var dataPage = filteredData.Skip(request.Start).Take(request.Length);

        var response = DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

        try
        {
            return new DataTablesJsonResult(response, true);
        }
        catch (Exception ex)
        {
            return Ok(ex.Message);
        }
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