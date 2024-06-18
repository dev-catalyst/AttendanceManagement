using AttendanceManagement.Data;
using AttendanceManagement.Models;
using AttendanceManagement.Models.ViewModel;
using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagement.Controllers;

[AllowAnonymous]
public class AppController : Controller
{
    private readonly ApplicationDbContext _context;

    // Role Manager
    private readonly RoleManager<IdentityRole> _roleManager;

    // User Manager
    private readonly UserManager<ApplicationUser> _userManager;


    public AppController(ApplicationDbContext context
        , RoleManager<IdentityRole> roleManager
        , UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public async Task<IActionResult> Users()
    {
        int rolesCount = (await _roleManager.Roles.ToListAsync()).Count();
        ViewData["RolesCount"] = rolesCount;
        return View();
    }

    public Task<IActionResult> AddUser()
    {
        return Task.FromResult<IActionResult>(View());
    }

    public Task<IActionResult> BulkAdd()
    {
        ViewData["Department"] = new SelectList(_context.Departments, "Id", "Name");
        return Task.FromResult<IActionResult>(View());
    }


    [HttpPost]
    public async Task<IActionResult> BulkAdd(string? department, int semester, string? firstName, string? lastName, string? email, string? enrollmentNumber, string? phoneNumber, string? password)
    {
        if (department == null || semester == 0 || firstName == null || lastName == null || email == null ||
            enrollmentNumber == null || phoneNumber == null)
        {
            return BadRequest("Not valid");
        }

        if (password == null) password = "Student@123";

        // break the string into array
        string[] emails = email.Split("\r\n");
        string[] enrollmentNumbers = enrollmentNumber.Split("\r\n");
        string[] phoneNumbers = phoneNumber.Split("\r\n");
        string[] firstNames = firstName.Split("\r\n");
        string[] lastNames = lastName.Split("\r\n");

        // check if all the arrays are of same length
        if (emails.Length != enrollmentNumbers.Length || emails.Length != phoneNumbers.Length ||
            emails.Length != firstNames.Length || emails.Length != lastNames.Length)
        {
            return BadRequest("Not valid");
        }

        // loop through the array and add to the database
        for (int i = 0; i < emails.Length; i++)
        {
            ApplicationUser user = new ApplicationUser
            {
                DepartmentId = department,
                Semester = semester,
                Email = emails[i],
                EnrollmentNumber = enrollmentNumbers[i],
                PhoneNumber = phoneNumbers[i],
                FirstName = firstNames[i],
                LastName = lastNames[i],
                UserName = emails[i],
                NormalizedEmail = emails[i].ToUpper(),
                NormalizedUserName = emails[i].ToUpper(),
                EmailConfirmed = true,
            };
            Task<IdentityResult> result = _userManager.CreateAsync(user, password);
            if (result.Result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Student");
            }
        }

        return Ok("Success");
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser(ApplicationUser user, string password)
    {
        ModelState.Remove("Password");
        if (ModelState.IsValid)
        {
            if (password == null) password = "Admin@123";

            user.UserName = user.Email;
            user.NormalizedEmail = user.Email.ToUpper();
            user.NormalizedUserName = user.Email.ToUpper();
            user.EmailConfirmed = true;


            Task<IdentityResult> result = _userManager.CreateAsync(user, password);

            if (result.Result.Succeeded)
            {
                // add Admin role to user
                await _userManager.AddToRoleAsync(user, "Admin");
                return Ok("success");
            }

            return BadRequest("Error");
        }

        return BadRequest("Not valid");
    }

    public async Task<IActionResult> PageDataUser(IDataTablesRequest request)
    {
        DbSet<ApplicationUser>? crrData = _context.Users;
        IEnumerable<DisplayUser> data = crrData.ToList().Select(x => new DisplayUser
        {
            Id = x.Id,
            Email = x.Email,
            UserName = x.UserName,
            FirstName = x.FirstName,
            LastName = x.LastName,
            PhoneNumber = x.PhoneNumber,
            Roles = _userManager.GetRolesAsync(x).Result
        });

        string[]? searches = request.Search.Value == null ? null : request.Search.Value.ToString().Split(" | ");
        string searchedTerm = String.Empty;
        if (searches == null)
        {
            int a;
            // ReSharper disable once ComplexConditionExpression
            IEnumerable<DisplayUser> filteredData = string.IsNullOrWhiteSpace(request.Search.Value)
                ? data
                : data
                    .Where(_item => _item.FirstName.ToLower().Contains(request.Search.Value.ToLower())
                                    | _item.LastName.ToLower().Contains(request.Search.Value.ToLower())
                                    | _item.Email.ToLower().Contains(request.Search.Value.ToLower())
                                    | _item.Roles[0].ToString().ToLower().Contains(request.Search.Value.ToLower())
                    );

            IEnumerable<DisplayUser> dataPage = filteredData.Skip(request.Start).Take(request.Length);

            DataTablesResponse response =
                DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

            return new DataTablesJsonResult(response, true);
        }
        else
        {
            IEnumerable<DisplayUser> filteredData = string.IsNullOrWhiteSpace(request.Search.Value)
                ? data
                : data
                    .Where(_item => _item.FirstName.ToLower().Contains(request.Search.Value.ToLower())
                                    | _item.LastName.ToLower().Contains(request.Search.Value.ToLower())
                                    | _item.Email.ToLower().Contains(request.Search.Value.ToLower())
                                    | _item.Roles[0].ToString().ToLower().Contains(request.Search.Value.ToLower())
                    );
            // ReSharper disable once ComplexConditionExpression
            foreach (string search in searches)
            {
                searchedTerm = search;
                IEnumerable<DisplayUser> tempData = string.IsNullOrWhiteSpace(searchedTerm)
                    ? data
                    : data
                        .Where(_item => _item.FirstName.ToLower().Contains(searchedTerm.ToLower())
                                        | _item.LastName.ToLower().Contains(searchedTerm.ToLower())
                                        | _item.Email.ToLower().Contains(searchedTerm.ToLower())
                                        | _item.Roles[0].ToString().ToLower().Contains(searchedTerm.ToLower())
                        );

                filteredData = filteredData.Concat(tempData);
            }
            // select distinct data

            IEnumerable<DisplayUser> dataPage =
                filteredData.DistinctBy(x => x.Email).Skip(request.Start).Take(request.Length);

            DataTablesResponse response =
                DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

            return new DataTablesJsonResult(response, true);
        }
    }


    public async Task<IActionResult> PageDataRole(IDataTablesRequest request)
    {
        DbSet<IdentityRole> data = _context.Roles;

        IQueryable<IdentityRole> filteredData = string.IsNullOrWhiteSpace(request.Search.Value)
            ? data
            : data.Where(_item => _item.Name.Contains(request.Search.Value)
            );

        IQueryable<IdentityRole> dataPage = filteredData.Skip(request.Start).Take(request.Length);

        DataTablesResponse response = DataTablesResponse.Create(request, data.Count(), filteredData.Count(), dataPage);

        return new DataTablesJsonResult(response, true);
    }

    public async Task<IActionResult> EditUser(string id)
    {
        ApplicationUser? user = _context.Users.FirstOrDefault(x => x.Id == id);
        // get all roles
        List<IdentityRole> roles = _roleManager.Roles.ToList();
        // get user roles
        if (user != null)
        {
            IList<string> userRoles = _userManager.GetRolesAsync(user).Result;
            ViewBag.UserRoles = userRoles;
            SelectList listedRoles = new SelectList(roles);
            listedRoles.First(x => x.Selected = userRoles.Contains(x.Text)).Selected = true;
            ViewBag.roles = listedRoles;
            return View(user);
        }

        return NotFound();
    }


    [HttpPost]
    public async Task<IActionResult> EditUser(ApplicationUser user, string? role)
    {
        if (ModelState.IsValid)
        {
            ApplicationUser? userToUpdate = _context.Users.FirstOrDefault(x => x.Id == user.Id);
            userToUpdate.FirstName = user.FirstName;
            userToUpdate.LastName = user.LastName;
            userToUpdate.PhoneNumber = user.PhoneNumber;

            Task<IdentityResult> result = _userManager.UpdateAsync(userToUpdate);

            if (result.Result.Succeeded)
            {
                if (role is not null)
                {
                    // remove all roles from user
                    IList<string> userRoles = _userManager.GetRolesAsync(userToUpdate).Result;
                    Task<IdentityResult> removeResult = _userManager.RemoveFromRolesAsync(userToUpdate, userRoles);
                    if (removeResult.Result.Succeeded)
                    {
                        // add new role to user
                        Task<IdentityResult> addResult = _userManager.AddToRoleAsync(userToUpdate, role);
                        if (addResult.Result.Succeeded) return Ok("success");
                    }
                }
                else
                {
                    return Ok("success");
                }

                return Ok("Error");
            }

            return BadRequest("Error");
        }

        return BadRequest("Not valid");
    }


    [HttpDelete]
    public async Task<IActionResult> DeleteUser(string id)
    {
        ApplicationUser? user = _context.Users.FirstOrDefault(x => x.Id == id);
        _context.Visitors.RemoveRange(_context.Visitors.Where(x => x.UserId == id));
        _context.Teachers.RemoveRange(_context.Teachers.Where(x => x.Id == id));
        _context.Slots.RemoveRange(_context.Slots.Where(x => x.TeacherId == id));
        await _context.SaveChangesAsync();
        Task<IdentityResult> result = _userManager.DeleteAsync(user);
        if (result.Result.Succeeded)
            return Ok("success");
        return BadRequest("Error");
    }

    public async Task<IActionResult> Roles()
    {
        return View();
    }

    public async Task<IActionResult> CreateRole()
    {
        return View(new IdentityRole());
    }


    // edit role
    public async Task<IActionResult> EditRole(string id)
    {
        IdentityRole? role = await _roleManager.FindByIdAsync(id);
        return View(role);
    }

    [HttpPost]
    public async Task<IActionResult> EditRole(IdentityRole role)
    {
        await _roleManager.UpdateAsync(role);
        return StatusCode(200, "Success");
    }

    // POST: Create Role
    [HttpPost]
    public async Task<IActionResult> CreateRole(string roleName)
    {
        IdentityRole role = new IdentityRole(roleName);
        IdentityResult result = await _roleManager.CreateAsync(role);
        if (result.Succeeded) return Ok();

        return NotFound();
    }

    // POST: Delete Role
    [HttpPost]
    public async Task<IActionResult> DeleteRole(string roleName)
    {
        IdentityRole? role = await _roleManager.FindByNameAsync(roleName);
        if (role != null)
        {
            IdentityResult result = await _roleManager.DeleteAsync(
                role);
            if (result.Succeeded) return Ok();
        }

        return NotFound();
    }

    // POST: Add User to Role
    [HttpPost]
    public async Task<IActionResult> AddUserToRole(string userName, string roleName)
    {
        ApplicationUser? user = await _userManager.FindByNameAsync(userName);
        if (user != null)
        {
            IdentityResult result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded) return Ok();
        }

        return NotFound();
    }

    // POST: Remove User from Role
    [HttpPost]
    public async Task<IActionResult> RemoveUserFromRole(string userName, string roleName)
    {
        ApplicationUser? user = await _userManager.FindByNameAsync(userName);
        if (user != null)
        {
            IdentityResult result = await _userManager.RemoveFromRoleAsync(user, roleName);
            if (result.Succeeded) return Ok();
        }

        return NotFound();
    }

    // POST: Get All Roles
    [HttpPost]
    public async Task<IActionResult> GetAllRoles()
    {
        List<IdentityRole> roles = await _roleManager.Roles.ToListAsync();
        return Ok(roles);
    }
}