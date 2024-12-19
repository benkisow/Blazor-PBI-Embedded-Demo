/* Model that is used for Tile objects returned from Power BI API */
namespace PowerBIEmbedApp.Models.PowerBiModels;

public class PowerBIWorkspace
{
    public string Id { get; set; }
    public bool IsReadOnly { get; set; }
    public bool IsOnDedicatedCapacity { get; set; }
    public string? CapacityId { get; set; }
    public string? DefaultDatasetStorageFormat { get; set; }
    public string? Type { get; set; }
    public string? Name { get; set; }
}