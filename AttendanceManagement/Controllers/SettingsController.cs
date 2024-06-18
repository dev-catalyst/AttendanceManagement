using AttendanceManagement.Data;
using AttendanceManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagement.Controllers;

public class SettingsController : Controller
{
    private readonly ApplicationDbContext _context;

    // _usermanager
    private readonly UserManager<ApplicationUser> _userManager;


    public SettingsController(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager
    )
    {
        _context = context;
        _userManager = userManager;
    }

    // GET
    public async Task<IActionResult> Basic()
    {
        // get the current user
        var user = _userManager.GetUserAsync(User).Result;
        // get role
        ViewBag.role = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
        return View(user);
    }


    [HttpPost]
    public async Task<IActionResult> UpdateBasic(ApplicationUser user)
    {
        var oldUser = _userManager.GetUserAsync(User).Result;
        oldUser.FirstName = user.FirstName;
        oldUser.LastName = user.LastName;
        oldUser.PhoneNumber = user.PhoneNumber;
        var role = _userManager.GetRolesAsync(oldUser).Result.FirstOrDefault();
        if (role == "Student")
        {
            oldUser.Semester = user.Semester;
            oldUser.EnrollmentNumber = user.EnrollmentNumber;
        }

        await _userManager.UpdateAsync(oldUser);
        return Ok("success");
    }
}