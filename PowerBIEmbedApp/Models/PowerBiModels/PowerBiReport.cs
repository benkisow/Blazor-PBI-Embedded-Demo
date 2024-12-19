/* Model that is used for Report objects returned from Power BI API */
using Microsoft.Graph;

namespace PowerBIEmbedApp.Models.PowerBiModels;

public class PowerBIReport
{
    public string Id { get; set; }
    public string? ReportType { get; set; }
    public string? Name { get; set; }
    public string? WebUrl { get; set; }
    public string? EmbedUrl { get; set; }
    public bool IsOwnedByMe { get; set; }
    public string? DatasetId { get; set; }
    public string? AppId { get; set; }
    public string? DatasetWorkspaceId { get; set; }
}