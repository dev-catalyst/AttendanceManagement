namespace AttendanceManagement.Models.ViewModel;

public class SidebarLink
{
    public SidebarLink(string text, string url, string icon)
    {
        Title = text;
        Url = url;
        Icon = icon;
    }
    public string Title { get; set; }
    public string Url { get; set; }
    public string Icon { get; set; }
}