namespace AttendanceManagement.Models.ViewModel;

public class DisplayUser : ApplicationUser
{
    public IList<string> Roles { get; set; }
}