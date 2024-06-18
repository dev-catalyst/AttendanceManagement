using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagement.Controllers;

public class ErrorController : Controller
{
    [Route("/404")]
    public async Task<IActionResult> PageNotFound()
    {
        return View();
    }
}