namespace AttendanceManagement.Models.ViewModel;

public class DashboardCard
{
    public DashboardCard(string text, string url, string icon)
    {
        Heading = text;
        Link = url;
        Icon = icon;
    }
    public string Heading { get; set; }
    public string Icon { get; set; }
    public string Link  { get; set; }
}