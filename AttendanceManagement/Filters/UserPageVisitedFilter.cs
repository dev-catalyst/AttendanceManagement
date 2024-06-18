using AttendanceManagement.Data;
using AttendanceManagement.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace AttendanceManagement.Filters
{
    public class UserPageVisitedFilter : Attribute, IActionFilter
    {
        private readonly ApplicationDbContext _dbcontext;
        private readonly UserManager<ApplicationUser> _userManager;
        private TimeZoneInfo INDIAN_ZONE = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

        public UserPageVisitedFilter(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _dbcontext = context;
            _userManager = userManager;
        }


        public void OnActionExecuting(ActionExecutingContext context)
        {
            
        }
        
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Visitor visitor = new Visitor();
            //
            // visitor.IpAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            //
            // visitor.time = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, INDIAN_ZONE);
            //
            // visitor.VisitedId = context.HttpContext.Connection.Id.ToString();
            //
            // visitor.Path = context.HttpContext.Request.Path;
            //
            // if (context.HttpContext.User.Identity?.Name != null)
            // {
            //     visitor.users = _userManager.FindByNameAsync(context.HttpContext.User.Identity?.Name).Result;
            // }
            // else
            // {
            //     visitor.users = null;
            // }
            //
            // var result =  _dbcontext.Visitors.AddAsync(visitor).Result;
            // if (result != null)
            // {
            //     _dbcontext.SaveChangesAsync();
            // }
        }
    }
}