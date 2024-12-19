/* Model that is used for Dashboard objects returned from Power BI API */
namespace PowerBIEmbedApp.Models.PowerBiModels;

public class PowerBiDashboard
{
    public string Id { get; set; }
    public string? DisplayName { get; set; }
    public bool IsReadOnly { get; set; }
    public string? WebUrl { get; set; }
    public string? EmbedUrl { get; set; }
}
